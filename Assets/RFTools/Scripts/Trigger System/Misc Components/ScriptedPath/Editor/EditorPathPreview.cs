using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Ravenfield.Trigger
{
	public class EditorPathPreview : IScriptedPathSeekerCallbackTarget
	{
		const float DEFAULT_SPEED = 0f;

		const float CHARACTER_HEAD_SIZE = 0.3f;

		const float CHARACTER_HEIGHT_STANDING = 1.5f;
		const float CHARACTER_DIAMETER = 0.55f;

		const float CHARACTER_HEIGHT_CROUCHED = 0.9f;

		const float CHARACTER_HEIGHT_PRONE = 0.5f;
		const float CHARACTER_DEPTH_PRONE = 1.5f;

		const float LEAN_AMOUNT = -10f;

		ScriptedPath path;
		public ScriptedPathSeeker seeker;

		public bool autoCompleteSyncs = false;

		public float cameraDistance;

		TimedAction displayAnimationAction = new TimedAction(1f, true);
		AnimationModifier.Animation animation;

		public EditorPathPreview(ScriptedPath path) {
			this.seeker = new ScriptedPathSeeker(this);
			SetPath(path);
		}

		public void SetPath(ScriptedPath path) {
			this.path = path;
		}

		public void Play(int startNode) {

			int nextNodeIndex = startNode + 1;

			Quaternion rotation;

			try {
				Vector3 prevNode = this.path.GetNodeWorldPosition(startNode);
				Vector3 nextNode = this.path.GetNodeWorldPosition(nextNodeIndex);

				rotation = Quaternion.LookRotation(nextNode - prevNode);
			}
			catch(System.Exception) {
				rotation = Quaternion.identity;
			}

			this.seeker.SetPath(this.path, this.path.GetNodeWorldPosition(startNode), rotation, true, startNode+1);
		}

		public void PlayFromSyncNumber(byte syncNumber) {
			for (int i = 0; i < this.path.GetNodeCount(); i++) {
				var node = this.path.pathNodes[i];
				if (node.synchronize && node.syncNumber == syncNumber) {
					Play(i);
					return;
				}
			}

			this.seeker.Stop();
		}

		public void Update(float dt) {

			if (this.autoCompleteSyncs && this.seeker.awaitingSync) {
				this.seeker.Synchronize(this.seeker.nextSyncNumber);
			}

			this.seeker.Update(dt);
			var matrix = Handles.matrix;

			float characterWidth = CHARACTER_DIAMETER;
			float characterDepth = CHARACTER_DIAMETER;
			float characterHeight = CHARACTER_HEIGHT_STANDING;

			if (this.seeker.stance == ScriptedPathNode.Stance.Crouched) {
				characterHeight = CHARACTER_HEIGHT_CROUCHED;
			}
			else if (this.seeker.stance == ScriptedPathNode.Stance.Prone) {
				characterHeight = CHARACTER_HEIGHT_PRONE;
				characterDepth = CHARACTER_DEPTH_PRONE;
			}

			var lookAtPoint = this.seeker.lookAtPoint;
			lookAtPoint.y += characterHeight / 2;

			Vector3 position = this.seeker.position;
			position.y += characterHeight / 2;

			Quaternion localLeanRotation = Quaternion.Euler(0f, 0f, this.seeker.modifierData.lean * LEAN_AMOUNT);

			Quaternion actorRotation = this.seeker.rotation * localLeanRotation;

			Quaternion headRotation = actorRotation;

			Vector3 localHeadOffset = new Vector3(0f, characterHeight / 2f, 0f);
			Vector3 weaponOffset = actorRotation * new Vector3(0f, 0f, characterDepth / 2);
			weaponOffset.y += 0.3f;

			if (this.seeker.stance == ScriptedPathNode.Stance.Prone) {
				localHeadOffset.z += CHARACTER_DEPTH_PRONE / 2;
				localHeadOffset.y -= 0.1f;
				weaponOffset.y -= 0.2f;
			}

			Vector3 worldHeadPosition = position + actorRotation * localHeadOffset;

			float headIkWeight = this.seeker.GetInterpolatedHeadIKWeight();

			if (headIkWeight > 0f) {
				var targetHeadLookPoint = this.seeker.GetInterpolatedHeadLookAtPoint();
				var headLookDirection = targetHeadLookPoint - worldHeadPosition;
				var targetHeadRotation = Quaternion.LookRotation(headLookDirection);

				headRotation = Quaternion.Slerp(actorRotation, targetHeadRotation, headIkWeight);

				Handles.color = Color.blue;
				Handles.DrawDottedLine(worldHeadPosition, targetHeadLookPoint, 5f);
			}

			Vector3 characterScale = new Vector3(characterWidth, characterHeight, characterDepth);
			Quaternion aimRotation = actorRotation * Quaternion.Euler(this.seeker.modifierData.isAlert ? 0f : 50f, 0f, 0f);

			Vector3 cameraForward = SceneView.lastActiveSceneView.camera.transform.forward;

			if(Vector3.Dot(actorRotation * Vector3.forward, cameraForward) < 0f) {
				RenderCharacter(position, actorRotation, headRotation, worldHeadPosition, characterScale);
				RenderWeapon(position, weaponOffset, aimRotation);
			}
			else {
				RenderWeapon(position, weaponOffset, aimRotation);
				RenderCharacter(position, actorRotation, headRotation, worldHeadPosition, characterScale);
			}

			Handles.matrix = matrix;

			Handles.color = Color.red;
			Handles.DrawDottedLine(position, lookAtPoint, 5f);
			Handles.DrawLine(lookAtPoint, this.seeker.lookAtPoint);

			
			if(!this.displayAnimationAction.TrueDone()) {
				var guiPoint = HandleUtility.WorldToGUIPoint(position);
				Rect rect = new Rect(guiPoint, new Vector2(200f, 30f));

				Handles.BeginGUI();

				GUI.Label(rect, this.animation.ToString(), ScriptedPath.Debug.BODY_GUI_STYLE);

				Handles.EndGUI();
			}
			

			this.cameraDistance = Vector3.Distance(position, SceneView.lastActiveSceneView.camera.transform.position);
		}

		private void RenderWeapon(Vector3 position, Vector3 weaponOffset, Quaternion aimRotation) {
			Handles.matrix = Matrix4x4.TRS(position + weaponOffset, aimRotation, new Vector3(0.3f, 0.3f, 0.7f));
			RenderCube(new Color(0.6f, 0.6f, 0.6f), new Vector3(0f, 0f, 0.5f));
		}

		private void RenderCharacter(Vector3 position, Quaternion actorRotation, Quaternion headRotation, Vector3 worldHeadPosition, Vector3 characterScale) {
			Handles.matrix = Matrix4x4.TRS(position, actorRotation, characterScale);
			RenderCube(new Color(0.8f, 0.5f, 0.8f), Vector3.zero);

			Handles.matrix = Matrix4x4.TRS(worldHeadPosition, headRotation, new Vector3(CHARACTER_HEAD_SIZE, CHARACTER_HEAD_SIZE, CHARACTER_HEAD_SIZE));
			RenderCube(new Color(0.8f, 0.5f, 0.8f), new Vector3(0f, 0.5f, 0f));
		}

		void RenderCube(Color c, Vector3 localPosition) {

			var behindColor = c * 0.7f;
			behindColor.a = 0.15f;

			Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
			Handles.color = behindColor;
			Handles.CubeHandleCap(-1, localPosition, Quaternion.identity, 1f, EventType.Repaint);

			Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
			Handles.color = c;
			Handles.CubeHandleCap(-1, localPosition, Quaternion.identity, 1f, EventType.Repaint);
		}

		public bool IsPlaying() {
			return this.seeker.isTraversingPath;
		}

		public void OnScriptedPathAnimationTriggered(AnimationModifier.Animation animation) {
			this.displayAnimationAction.Start();
			this.animation = animation;
		}

		public void OnScriptedPathCompleted(bool stayAtEnd) {
			
		}

		public void ForceTeleportToPosition(Vector3 position) {
			
		}

		public void OnScriptedPathFireWeapon(float holdFireTime) {
			
		}
	}
}
