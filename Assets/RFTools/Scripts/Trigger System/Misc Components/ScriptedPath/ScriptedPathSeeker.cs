using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	public class ScriptedPathSeeker
	{
		enum Phase
		{
			GotoStartNode,
			FollowingPath,
		}

		const int MIN_VALID_NODE_COUNT = 2;
		public const byte SYNC_NUMBER_NONE = byte.MaxValue;

		public const float MIN_SPEED = 0.5f;
		const float DEFAULT_SPEED = 2f;
		const float CHANGE_SPEED_EXTRAPOLATION_TIME = 0.5f;

		const float SLOW_DOWN_EXTRAPOLATION_TIME = 1f;
		const float SLOW_DOWN_SPEED_MULTIPLIER = 0.4f;

		const float AIM_LEAD_EXTRAPOLATION_TIME = 2f;
		const float MIN_AIM_LEAD_DISTANCE = 2f;

		const float LOOK_TOWARDS_TARGET_DAMP = AiActorController.LOOK_TOWARDS_TARGET_DAMP;

		Phase phase = Phase.GotoStartNode;

		public ScriptedPath path;

		Vector3 nodeStartLocalPosition;

		public Vector3 position {
			get;
			private set;
		}
		public Quaternion rotation;
		public Vector3 lookAtPoint;

		public Vector3 movementVelocity;
		public float movementSpeed;

		public int destinationNodeIndex;

		public bool isTraversingPath;
		public ScriptedPathNode.Stance stance = ScriptedPathNode.Stance.Standing;

		public byte nextSyncNumber;
		public bool awaitingSync;
		public bool awaitingTimer;

		public bool awaitingGotoPath;
		List<Vector3> gotoPath;
		int gotoPathNextNode;
		Vector3 gotoPathPreviousSegmentStartPosition;

		Vector3 agentPosition;

		bool applyMovingPlatformMovement = false;
		Vector3 lastFixedWorldPosition = Vector3.zero;
		Vector3 lastFixedLocalPosition = Vector3.zero;

		TimedAction waitAction = new TimedAction(1f, true);

		float currentSeekerSequenceValue;

		#region MODIFIER DATA

		// MODIFIERS //
		public IScriptedPathSeekerCallbackTarget callbackTarget;
		public ModifierData modifierData;

		public TimedAction interpolateHeadWeightAction = new TimedAction(0.3f, true);
		public TimedAction interpolateHeadPositionAction = new TimedAction(0.3f, true);

		Queue<ScriptedPathEdgeModifier> pendingModifiers;

		#endregion

		public ScriptedPathSeeker(IScriptedPathSeekerCallbackTarget callbackTarget) {
			this.callbackTarget = callbackTarget;
		}

		public void UpdateAgentPosition(Vector3 agentPosition) {
			this.agentPosition = agentPosition;
			this.position = agentPosition;
		}

		public void SetPath(ScriptedPath path, Vector3 position, Quaternion rotation, bool teleportToStart, int startIndex = 0) {
			this.path = path;

			CreateModifierQueue();

			this.applyMovingPlatformMovement = false;

			this.position = position;
			this.rotation = rotation;

			if (this.path.GetNodeCount() < MIN_VALID_NODE_COUNT) {
				this.isTraversingPath = false;
				return;
			}

			this.isTraversingPath = true;
			this.destinationNodeIndex = startIndex;

			this.modifierData = new ModifierData() {
				isAlert = this.path.startAlert,
			};

			this.lookAtPoint = this.path.StartPosition();

			this.phase = Phase.FollowingPath;
			NextNode(true);
			
			UpdateNextSyncNumber();
		}

		public bool CurrentWaypoint(out Vector3 waypoint) {
			if(this.awaitingGotoPath) {
				waypoint = Vector3.zero;
				return false;
			}

			if(this.phase == Phase.GotoStartNode) {
				waypoint = this.gotoPath[this.gotoPathNextNode];
				return true;
			}
			else {
				int wrappedDestinationNodeIndex = this.destinationNodeIndex % this.path.GetNodeCount();
				waypoint = this.path.GetNodeWorldPosition(wrappedDestinationNodeIndex);
				return true;
			}
		}

		void CreateModifierQueue() {
			this.path.SortModifiers();

			

			this.pendingModifiers = new Queue<ScriptedPathEdgeModifier>(this.path.modifiers.Count);
			ResetModifierQueue();
		}

		void ResetModifierQueue() {
			this.pendingModifiers.Clear();
			for (int i = 0; i < this.path.modifiers.Count; i++) {
				this.pendingModifiers.Enqueue(this.path.modifiers[i]);
			}
		}

		void UpdateNextSyncNumber() {
			this.nextSyncNumber = SYNC_NUMBER_NONE;

			int nNodes = this.path.GetNodeCount();

			if(this.path.loop) {
				for (int i = 0; i < nNodes; i++) {
					int nodeIndex = (this.destinationNodeIndex + i) % nNodes;
					var node = this.path.pathNodes[nodeIndex];
					if (node.synchronize) {
						this.nextSyncNumber = node.syncNumber;
						return;
					}
				}
			}
			else {
				for (int i = this.destinationNodeIndex; i < nNodes; i++) {
					var node = this.path.pathNodes[i];
					if (node.synchronize) {
						this.nextSyncNumber = node.syncNumber;
						return;
					}
				}
			}
		}

		void OnNodeReached() {
			var reachedNode = this.path.pathNodes[this.destinationNodeIndex % this.path.pathNodes.Count];

			if (reachedNode.synchronize) {
				this.awaitingSync = true;
				this.stance = this.path.pathNodes[this.destinationNodeIndex].stance;
				return;
			}
			else {
				NextNode(false);
			}
		}

		void NextNode(bool isStart) {

			this.nodeStartLocalPosition = this.path.worldToLocal.MultiplyPoint(this.position);
			this.awaitingSync = false;

			int nNodes = this.path.GetNodeCount();

			if (!isStart) {
				this.destinationNodeIndex++;
			}

			if(this.path.loop) {
				if(this.destinationNodeIndex > nNodes) {
					OnLoopCompleted();
					return;
				}
			}
			else {
				if (this.destinationNodeIndex == nNodes) {
					OnPathCompleted();
					return;
				}
			}
			
			if(!isStart) {

				var node = this.path.pathNodes[this.destinationNodeIndex - 1];
				this.stance = node.stance;

				float waitTime = node.waitTime;
				if (waitTime > 0f) {
					this.waitAction.StartLifetime(waitTime);
				}
			}
		}

		void OnLoopCompleted() {
			ResetModifierQueue();
			this.destinationNodeIndex = 0;
			this.currentSeekerSequenceValue = 0f;
			NextNode(false);
		}

		public void Synchronize(byte syncNumber) {
			if(this.awaitingSync && this.nextSyncNumber == syncNumber) {
				NextNode(false);
				UpdateNextSyncNumber();
			}
		}

		void OnPathCompleted() {
			this.callbackTarget.OnScriptedPathCompleted(this.path.stayAtEnd);
			Stop();
		}

		public void Stop() {
			this.isTraversingPath = false;
		}

		void UpdateGotoStart(float dt) {
			if(this.awaitingGotoPath) {

				Debug.DrawRay(this.position, new Vector3(0f, 5f, 0f), Color.magenta);
				UpdateStandStill();
				return;
			}

			

			Vector3 nextNode = this.gotoPath[this.gotoPathNextNode];
			Vector3 segment = (nextNode - this.gotoPathPreviousSegmentStartPosition).ToGround();
			Vector3 delta = (nextNode - this.position).ToGround();

			if(segment.magnitude < 0.1f) {
				NextGotoStartNode();
				return;
			}

			Debug.DrawLine(this.position, nextNode, Color.magenta);

			float t = SMath.LineVsPointClosestT(this.gotoPathPreviousSegmentStartPosition, segment, this.position);

			this.movementVelocity = delta.normalized * this.path.gotoStartMovementSpeed;
			this.movementSpeed = this.path.gotoStartMovementSpeed;
			

			if (t > 0.999f) {
				NextGotoStartNode();
			}

			this.rotation = SMath.DampSlerp(this.rotation, Quaternion.LookRotation(this.lookAtPoint - this.position), LOOK_TOWARDS_TARGET_DAMP, dt);
		}

		void NextGotoStartNode() {
			this.gotoPathNextNode++;
			this.gotoPathPreviousSegmentStartPosition = this.position;

			if (this.gotoPathNextNode >= this.gotoPath.Count) {
				// Start following the path
				OnReachedStart();
			}
		}

		void OnReachedStart() {
			this.phase = Phase.FollowingPath;
			NextNode(true);
		}

		public bool UpdateMovingPlatform(out Vector3 movingPlatformDelta) {
			movingPlatformDelta = Vector3.zero;
			if(!this.path.isStatic) {

				bool apply = this.applyMovingPlatformMovement;

				if(apply) {
					movingPlatformDelta = this.path.localToWorld.MultiplyPoint(this.lastFixedLocalPosition) - this.lastFixedWorldPosition;
				}

				this.applyMovingPlatformMovement = true;
				this.lastFixedWorldPosition = this.agentPosition;
				this.lastFixedLocalPosition = this.path.worldToLocal.MultiplyPoint(this.agentPosition);

				return apply;
			}

			return false;
		}

		public void Update(float dt) {

			if (this.path == null) return;

			if (dt <= Mathf.Epsilon) {
				this.movementVelocity = Vector3.zero;
				return;
			}

			if (this.phase == Phase.GotoStartNode) {
				UpdateGotoStart(dt);
			}
			else {
				if (!this.isTraversingPath || this.awaitingSync || !this.waitAction.TrueDone()) {
					// Stand still
					UpdateStandStill();
				}
				else {
					UpdateMovement(dt);
					UpdateModifiers(dt);
				}
			}
		}

		void UpdateStandStill() {
			this.movementVelocity = Vector3.zero;
			this.movementSpeed = 0f;
		}

		private void UpdateMovement(float dt) {

			int nNodes = this.path.GetNodeCount();
			int wrappedDestinationNodeIndex = this.destinationNodeIndex % nNodes;
			Vector3 destinationNodePosition = this.path.GetNodeWorldPosition(wrappedDestinationNodeIndex);

			Vector3 nodeStartWorldPosition = this.path.localToWorld.MultiplyPoint(this.nodeStartLocalPosition);

			Vector3 destinationNodePositionProjected = destinationNodePosition;
			Vector3 nodeStartWorldPositionProjected = nodeStartWorldPosition;

			Debug.DrawLine(destinationNodePositionProjected, nodeStartWorldPositionProjected, Color.magenta);
			Debug.DrawLine(this.position, destinationNodePositionProjected, Color.green);

			Vector3 deltaDestination = destinationNodePositionProjected - this.position;
			float destinationDistance = deltaDestination.magnitude;
			float startDistance = (this.position - nodeStartWorldPositionProjected).magnitude;
			float edgeLength = (destinationNodePositionProjected - nodeStartWorldPositionProjected).magnitude;
			float halfEdgeLength = edgeLength / 2;

			float prevEdgeSpeed = Mathf.Max(MIN_SPEED, GetNodeSpeed(this.destinationNodeIndex - 2, true));
			float currEdgeSpeed = Mathf.Max(MIN_SPEED, GetNodeSpeed(this.destinationNodeIndex - 1, false));

			float speedControlRange = Mathf.Min(halfEdgeLength, prevEdgeSpeed * CHANGE_SPEED_EXTRAPOLATION_TIME);

			float startRatio = Mathf.InverseLerp(0f, speedControlRange, startDistance);
			float speed = Mathf.Lerp(prevEdgeSpeed, currEdgeSpeed, startRatio);

			if (this.destinationNodeIndex > 0) {
				var currentEdgeInfo = this.path.pathNodes[this.destinationNodeIndex - 1];

				if (currentEdgeInfo.slowDownForNextTurn) {

					float extrapolationSpeed = (currEdgeSpeed + prevEdgeSpeed) / 2;

					float slowDownRange = Mathf.Min(halfEdgeLength, extrapolationSpeed * SLOW_DOWN_EXTRAPOLATION_TIME);
					float endRatio = Mathf.InverseLerp(slowDownRange, 0f, destinationDistance);
					float multiplier = Mathf.Lerp(1f, SLOW_DOWN_SPEED_MULTIPLIER, endRatio);

					speed *= multiplier;
				}
			}

			//Debug.LogFormat("Speeds: Prev:{0:0.00}, Curr:{1:0.00}, Final:{2:0.00}", prevEdgeSpeed, currEdgeSpeed, speed);

			var destinationNode = this.path.pathNodes[wrappedDestinationNodeIndex];
			bool waitAtDestinationNode = destinationNode.synchronize || destinationNode.waitTime > 0f;

			speed = Mathf.Max(MIN_SPEED, speed);
			this.movementSpeed = speed;

			bool atDestination = destinationDistance < 0.001f;

			Vector3 nextPosition;
			float t;

			if(atDestination) {
				nextPosition = this.position;
				t = 0.5f;
			}
			else {
				Vector3 deltaMovement = (deltaDestination / destinationDistance) * speed * dt;
				nextPosition = this.position + deltaMovement;
				t = SMath.LineSegmentVsPointClosestT(this.position, nextPosition, destinationNodePositionProjected);
			}

			Vector3 deltaPosition = nextPosition - this.position;

			Vector3 nextProjectedPosition = this.position + deltaPosition * t;

			float aimLeadDistance = Mathf.Max(MIN_AIM_LEAD_DISTANCE, currEdgeSpeed * AIM_LEAD_EXTRAPOLATION_TIME);

			bool lookAtFutureEdge = !waitAtDestinationNode && (this.path.loop || this.destinationNodeIndex < nNodes-1);

			if (this.modifierData.hasOverrideLookAtPoint) {
				this.lookAtPoint = modifierData.overrideLookAtPoint;
			}
			else if (destinationDistance > aimLeadDistance) {
				this.lookAtPoint = destinationNodePositionProjected;
			}
			else if (lookAtFutureEdge) {

				int nextNodeIndex = (this.destinationNodeIndex + 1) % nNodes;

				// Aim towards next edge.
				float lookAheadNextEdgeRange = aimLeadDistance - destinationDistance;

				// Scale the look ahead max distance by the startDistance range, which prevents aim from snapping when nodes are very close together.
				lookAheadNextEdgeRange = Mathf.Min(lookAheadNextEdgeRange, startDistance * 2);

				var nextDestinationNode = this.path.GetNodeWorldPosition(nextNodeIndex);
				var nextEdgeDelta = nextDestinationNode - destinationNodePositionProjected;
				float nextEdgeLength = nextEdgeDelta.magnitude;

				lookAheadNextEdgeRange = Mathf.Min(lookAheadNextEdgeRange, nextEdgeLength);

				this.lookAtPoint = destinationNodePositionProjected + (nextEdgeDelta / nextEdgeLength) * lookAheadNextEdgeRange;
			}
			else {
				// Just look ahead
				this.lookAtPoint = nextProjectedPosition;
			}

			var direction = this.lookAtPoint - this.position;
			var runDirectionRotation = Quaternion.LookRotation(direction);


			this.rotation = SMath.DampSlerp(this.rotation, runDirectionRotation, LOOK_TOWARDS_TARGET_DAMP, dt);

			this.movementVelocity = (nextProjectedPosition - this.position) / dt;

			this.position = nextProjectedPosition;

			this.currentSeekerSequenceValue = (this.destinationNodeIndex - 1) + startDistance / edgeLength;

			if (t < 1f) {
				OnNodeReached();
			}
		}

		void UpdateModifiers(float dt) {
			if (this.pendingModifiers.Count > 0) {
				var nextSequenceValue = this.pendingModifiers.Peek().GetSequenceValue();

				if(this.currentSeekerSequenceValue > nextSequenceValue) {
					OnPassModifier(this.pendingModifiers.Dequeue());
				}
			}
		}

		void OnPassModifier(ScriptedPathEdgeModifier scriptedPathEdgeModifier) {
			try {
				scriptedPathEdgeModifier.OnPassed(this);
			}
			catch(System.Exception e) {
				Debug.LogException(e);
			}
		}

		public void OverrideLookAtPoint(Vector3 lookAtPoint, LookAtModifier.Target target) {
			if (target == LookAtModifier.Target.Body) {
				this.modifierData.overrideLookAtPoint = lookAtPoint;
				this.modifierData.hasOverrideLookAtPoint = true;
			}
			else {
				if(this.modifierData.hasHeadLookAtPoint) {
					this.modifierData.prevHeadLookAtPoint = this.modifierData.headLookAtPoint;
					this.interpolateHeadPositionAction.Start();
				}
				else {
					this.interpolateHeadWeightAction.Start();
				}
				this.modifierData.headLookAtPoint = lookAtPoint;
				this.modifierData.hasHeadLookAtPoint = true;
			}
		}

		public void CancelOverrideLookAtPoint(LookAtModifier.Target target) {
			if (target == LookAtModifier.Target.Body) {
				this.modifierData.hasOverrideLookAtPoint = false;
			}
			else {
				this.modifierData.hasHeadLookAtPoint = false;
				this.interpolateHeadWeightAction.Start();
			}
		}

		public Vector3 GetInterpolatedHeadLookAtPoint() {
			if(this.interpolateHeadPositionAction.TrueDone()) {
				return this.modifierData.headLookAtPoint;
			}
			else {
				return Vector3.Lerp(this.modifierData.prevHeadLookAtPoint, this.modifierData.headLookAtPoint, this.interpolateHeadPositionAction.Ratio());
			}
		}

		public float GetInterpolatedHeadIKWeight() {
			if (this.interpolateHeadWeightAction.TrueDone()) {
				return this.modifierData.hasHeadLookAtPoint ? 1f : 0f;
			}
			else {
				return this.modifierData.hasHeadLookAtPoint ? this.interpolateHeadWeightAction.Ratio() : 1 - this.interpolateHeadWeightAction.Ratio();
			}

		}

		public float GetNodeSpeed(int index, bool isPrevious) {

			if(index < 0 || index >= this.path.GetNodeCount()-1) {
				return DEFAULT_SPEED;
			}

			var node = this.path.pathNodes[index];

			if(isPrevious && node.slowDownForNextTurn) {
				return node.speed * SLOW_DOWN_SPEED_MULTIPLIER;
			}
			return node.speed;
		}

		public struct ModifierData {
			public Vector3 overrideLookAtPoint;
			public bool hasOverrideLookAtPoint;

			public Vector3 prevHeadLookAtPoint;
			public Vector3 headLookAtPoint;
			public bool hasHeadLookAtPoint;

			public bool isAlert;

			public float lean;
		}
	}

}
