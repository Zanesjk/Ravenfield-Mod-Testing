using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Ravenfield.Trigger
{

	[CustomEditor(typeof(ScriptedPath))]
	public class ScriptedPathEditor : Editor
	{

		const float MIN_SPEED = 0f;
		const float MAX_SPEED = 10f;

		const float CROUCH_SPEED = 2f;
		const float PRONE_SPEED = 1f;

		const float PATROL_WALK_SPEED = 1f;
		const float NORMAL_WALK_SPEED = 3.2f;
		const float SPRINT_SPEED = 6.5f;

		const float MATCH_SPEED_DIFFERENCE = 0.2f;

		const float NODE_CAP_SIZE = 0.5f;
		const float MODIFIER_CAP_SIZE = 0.4f;

		const float CAP_SELECTION_EXTRA_SIZE = 0.1f;

		const float ADD_PANEL_WIDTH = 120f;
		const float ADD_PANEL_HEIGHT = 180f;
		const float ADD_PANEL_PADDING_BOTTOM = 15f;

		ScriptedPath path;
		ScriptedPathGroup parentGroup;

		int selectedNode = -1;
		int selectedModifier = -1;
		float prevTime;

		int scheduledDeleteNode = -1;
		int scheduledDeleteModifier = -1;

		EditorPathPreview preview;
		bool showAddPanel;

		void OnEnable() {
			this.path = this.target as ScriptedPath;

			this.selectedNode = -1;

			this.preview = new EditorPathPreview(this.path) {
				autoCompleteSyncs = true,
			};
			this.prevTime = Time.realtimeSinceStartup;

			this.parentGroup = this.path.GetComponentInParent<ScriptedPathGroup>();
			if (this.parentGroup != null) {
				this.parentGroup.FindPaths();
			}
		}

		void OnDisable() {
			Tools.hidden = false;
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
		}

		void DeselectNode() {
			this.selectedNode = -1;
		}

		void PlayPreview() {
			int startNodeIndex = this.selectedNode >= 0 ? this.selectedNode : 0;

			if(selectedModifier >= 0) {
				startNodeIndex = this.path.modifiers[selectedModifier].edgeIndex;
			}

			try {
				this.preview.Play(startNodeIndex);
			} catch (System.Exception e) {
				Debug.LogError("Could not play path preview: ");
				Debug.LogException(e);
			}
		}

		void PlayPreviewFromSyncNumber(byte syncNumber) {
			try {
				this.preview.PlayFromSyncNumber(syncNumber);
			} catch (System.Exception e) {
				Debug.LogError("Could not play path preview: ");
				Debug.LogException(e);
			}
		}

		public override bool RequiresConstantRepaint() {
			return this.preview.IsPlaying();
		}

		void OnSceneGUI() {

			Event e = Event.current;
			DrawParentGroup();

			bool anythingIsSelected = this.selectedModifier >= 0 || this.selectedNode >= 0;

			this.scheduledDeleteNode = -1;
			this.scheduledDeleteModifier = -1;

			if (e.type == EventType.KeyDown) {

				byte syncNumber;

				if (e.keyCode == KeyCode.Escape) {
					if (this.selectedNode >= 0) {
						DeselectNode();
					}
					else if (this.selectedModifier >= 0) {
						DeselectModifier();
					}
					else if (HasParentGroup()) {
						Selection.activeObject = this.parentGroup.gameObject;
					}

				}
				else if(e.keyCode == KeyCode.Tab) {
					SelectNextPathInGroup();
					e.Use();
				}
				else if (e.keyCode == KeyCode.Space) {
					PlayPreview();
					e.Use();
				}
				else if(ScriptedPathGroupEditor.GetPlayFromSyncNodeInput(e, out syncNumber)) {
					PlayPreviewFromSyncNumber(syncNumber);
				}
			}

			Tools.hidden = anythingIsSelected;

			this.path.UpdateTransformMatrix();

			int nNodes = this.path.GetNodeCount();
			for (int i = 0; i < nNodes; i++) {
				bool drawEdge = (this.path.loop && nNodes > 2) || i < nNodes - 1;
				DrawNode(i, drawEdge, i == this.selectedNode);
			}

			for (int i = 0; i < this.path.modifiers.Count; i++) {
				DrawModifier(i);
			}

			if (this.preview.IsPlaying() && e.type == EventType.Repaint) {
				float newTime = Time.realtimeSinceStartup;
				float dt = newTime - this.prevTime;
				this.prevTime = newTime;

				this.preview.Update(dt);
			}

			if (e.type == EventType.MouseUp && e.button == 0) {
				DeselectNode();
				DeselectModifier();
			}

			if (e.type == EventType.Layout && anythingIsSelected) {
				// Prevent deselecting this object while an object is selected.
				HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
			}

			if (this.scheduledDeleteNode >= 0) {
				this.path.pathNodes.RemoveAt(scheduledDeleteNode);
				DeselectNode();

				foreach (var modifier in this.path.modifiers) {
					if (modifier.edgeIndex >= scheduledDeleteNode) {
						modifier.edgeIndex--;
						modifier.edgeT = 1f;
					}
				}
			}
			if(this.scheduledDeleteModifier >= 0) {
				this.path.modifiers.RemoveAt(scheduledDeleteModifier);
				DeselectModifier();
			}
		}

		void SelectNextPathInGroup() {
			if(!HasParentGroup()) {
				return;
			}

			var currentIndex = this.parentGroup.paths.IndexOf(this.path);
			var nextIndex = (currentIndex + 1) % this.parentGroup.paths.Count;

			Selection.activeObject = this.parentGroup.paths[nextIndex].gameObject;
		}

		bool HasParentGroup() {
			return this.parentGroup != null;
		}

		private void DrawParentGroup() {
			if (HasParentGroup()) {
				foreach (var path in this.parentGroup.paths) {
					if (path != this.path) {
						ScriptedPathGroupEditor.DrawPath(path, Color.blue);
					}
				}
			}
		}

		void DrawNode(int index, bool drawEdge, bool isSelected) {
			bool isDirty = false;
			var nodePos = this.path.GetNodeWorldPosition(index);
			var node = this.path.pathNodes[index];

			bool isInFrontOfCamera = ScriptedPathGroupEditor.IsInFrontOfCamera(nodePos);

			Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;

			Handles.color = isSelected ? Color.red : Color.white;

			
			if (isInFrontOfCamera && Handles.Button(nodePos, Quaternion.identity, NODE_CAP_SIZE, NODE_CAP_SIZE + CAP_SELECTION_EXTRA_SIZE, Handles.CubeHandleCap)) {
				if (isInFrontOfCamera) {
					SelectNode(index);
				}
			}

			Vector3 nextNodePos;
			if (drawEdge) {
				Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
				int nextNodeIndex = (index + 1) % this.path.GetNodeCount();
				nextNodePos = this.path.GetNodeWorldPosition(nextNodeIndex);
				Handles.DrawLine(nodePos, nextNodePos);
			}
			else {
				var previousNode = this.path.GetNodeWorldPosition(index - 1);
				var delta = nodePos - previousNode;
				nextNodePos = nodePos + delta.normalized * 4;
			}

			Vector3 halfwayNodePos = (nodePos + nextNodePos) / 2;

			if (!isSelected) {
				if (HasParentGroup() && node.synchronize) {
					ScriptedPathGroupEditor.DrawSyncLabel(nodePos, node.syncNumber);
				}
				return;
			}

			Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
			Vector3 newNodePos = Handles.PositionHandle(nodePos, this.path.transform.rotation);

			if (newNodePos != nodePos) {
				isDirty = true;
				this.path.SetNodeWorldPosition(index, newNodePos);
			}

			var newNode = node;

			Handles.BeginGUI();

			if (isInFrontOfCamera && DrawNodeGuiPanel(index, drawEdge, nodePos, ref newNode)) {
				isDirty = true;
			}

			if (DrawAddGuiPanel(halfwayNodePos, drawEdge)) {
				isDirty = true;
			}

			Handles.EndGUI();

			if (!newNode.Equals(node)) {
				isDirty = true;
				this.path.pathNodes[index] = newNode;
			}

			if (isDirty) {
				MarkDirty();
			}
		}

		void MarkDirty() {
			//Debug.Log("Mark Dirty!");
			EditorUtility.SetDirty(this.path);
			EditorUtility.SetDirty(this.path.gameObject);
		}

		private bool DrawAddGuiPanel(Vector3 halfwayNodePos, bool drawEdge) {

			bool isDirty = false;

			try {
				var halfwayLabelGuiPoint = HandleUtility.WorldToGUIPoint(halfwayNodePos);

				var halfwayButtonAreaRect = new Rect(halfwayLabelGuiPoint + new Vector2(-10f, -10f), new Vector2(20f, 20f));
				GUILayout.BeginArea(halfwayButtonAreaRect);
				if (GUILayout.Button("+")) {
					this.showAddPanel = !this.showAddPanel;
				}
				GUILayout.EndArea();

				if (this.showAddPanel) {
					var halfwayAreaRect = new Rect(halfwayLabelGuiPoint + new Vector2(-ADD_PANEL_WIDTH / 2, -ADD_PANEL_HEIGHT - ADD_PANEL_PADDING_BOTTOM), new Vector2(ADD_PANEL_WIDTH, ADD_PANEL_HEIGHT));

					EditorGUI.DrawRect(halfwayAreaRect, new Color(0f, 0f, 0f, 0.7f));
					GUILayout.BeginArea(halfwayAreaRect);

					try {

						GUILayout.Label("ADD", ScriptedPath.Debug.TITLE_GUI_STYLE);
						GUILayout.Space(5f);

						if (GUILayout.Button("New Node")) {
							AddNode(this.selectedNode, halfwayNodePos);
							isDirty = true;
						}

						if (drawEdge) {
							GUILayout.FlexibleSpace();
							GUILayout.Label("Modifiers", ScriptedPath.Debug.BODY_GUI_STYLE);

							if (GUILayout.Button("LookAt")) {
								AddModifier<LookAtModifier>(this.selectedNode);
								isDirty = true;
							}

							if (GUILayout.Button("Alert")) {
								AddModifier<AlertModifier>(this.selectedNode);
								isDirty = true;
							}

							if (GUILayout.Button("Lean")) {
								AddModifier<LeanModifier>(this.selectedNode);
								isDirty = true;
							}

							if (GUILayout.Button("Animation")) {
								AddModifier<AnimationModifier>(this.selectedNode);
								isDirty = true;
							}

							if (GUILayout.Button("FireWeapon")) {
								AddModifier<FireWeaponModifier>(this.selectedNode);
								isDirty = true;
							}

							GUILayout.Space(5f);
						}

					} catch (System.Exception) {

					}

					GUILayout.EndArea();
				}
			}
			catch(System.Exception) { }

			return isDirty;
		}

		private bool DrawNodeGuiPanel(int index, bool drawEdge, Vector3 nodePos, ref ScriptedPathNode newNode) {

			try {
				// Can also mark dirty by changing newNode values.
				bool isDirty = false;

				var labelGuiPoint = HandleUtility.WorldToGUIPoint(nodePos);
				labelGuiPoint.x += 20f;
				labelGuiPoint.y += 20f;

				var areaRect = new Rect(labelGuiPoint, new Vector2(150f, 200f));

				EditorGUI.DrawRect(areaRect, new Color(0f, 0f, 0f, 0.7f));
				GUILayout.BeginArea(areaRect, ScriptedPath.Debug.TITLE_GUI_STYLE);

				string nodeStatusText = drawEdge ? "" : "(END)";

				GUILayout.Label(string.Format("Node #{0} {1}", index, nodeStatusText), ScriptedPath.Debug.TITLE_GUI_STYLE);
				GUILayout.Space(5f);

				GUILayout.BeginHorizontal();
				GUILayout.Label("Stance", ScriptedPath.Debug.BODY_GUI_STYLE);
				if (GUILayout.Button(newNode.stance.ToString())) {
					if (newNode.stance != ScriptedPathNode.Stance.Prone) {
						newNode.stance++;
					}
					else {
						newNode.stance = ScriptedPathNode.Stance.Standing;
					}
				}
				GUILayout.EndHorizontal();


				string speedLabel = "";

				if (newNode.stance == ScriptedPathNode.Stance.Standing) {
					if (MatchesSpeed(newNode.speed, PATROL_WALK_SPEED)) {
						speedLabel = "(PATROL)";
					}
					else if (MatchesSpeed(newNode.speed, NORMAL_WALK_SPEED)) {
						speedLabel = "(NORMAL)";
					}
					else if (MatchesSpeed(newNode.speed, SPRINT_SPEED)) {
						speedLabel = "(SPRINT)";
					}
				}
				else if (newNode.stance == ScriptedPathNode.Stance.Crouched) {
					if (MatchesSpeed(newNode.speed, CROUCH_SPEED)) {
						speedLabel = "(CROUCH)";
					}
				}
				else if (newNode.stance == ScriptedPathNode.Stance.Prone) {
					if (MatchesSpeed(newNode.speed, PRONE_SPEED)) {
						speedLabel = "(PRONE)";
					}
				}

				GUILayout.Label(string.Format("Speed: {0:0.0} {1}", newNode.speed, speedLabel), ScriptedPath.Debug.BODY_GUI_STYLE);

				float maxSpeed = MAX_SPEED;

				if (this.path.limitSpeed) {
					if (newNode.stance == ScriptedPathNode.Stance.Standing) {
						maxSpeed = SPRINT_SPEED;
					}
					else if (newNode.stance == ScriptedPathNode.Stance.Crouched) {
						maxSpeed = CROUCH_SPEED;
					}
					else if (newNode.stance == ScriptedPathNode.Stance.Prone) {
						maxSpeed = PRONE_SPEED;
					}
				}

				newNode.speed = GUILayout.HorizontalSlider(newNode.speed, MIN_SPEED, maxSpeed);
				GUILayout.Space(10f);
				newNode.slowDownForNextTurn = ScriptedPath.Debug.Toggle(newNode.slowDownForNextTurn, "Slow Down For Turn");

				GUILayout.Space(5f);

				string syncLabel = newNode.synchronize ? newNode.syncNumber.ToString() : "OFF";

				newNode.synchronize = ScriptedPath.Debug.Toggle(newNode.synchronize, string.Format("Sync ({0})", syncLabel));
				if (newNode.synchronize) {
					newNode.syncNumber = (byte)GUILayout.HorizontalSlider(newNode.syncNumber, 0f, 9f);
				}
				GUILayout.Space(15f);

				GUILayout.Label(string.Format("Wait time: {0:0.0}", newNode.waitTime), ScriptedPath.Debug.BODY_GUI_STYLE);
				newNode.waitTime = GUILayout.HorizontalSlider(newNode.waitTime, 0f, 10f);

				GUILayout.FlexibleSpace();

				if (GUILayout.Button("Delete")) {
					RemoveNode(index);
					isDirty = true;
				}

				GUILayout.EndArea();

				return isDirty;
			}
			catch(System.Exception) {

			}
			return false;
		}

		void DrawModifier(int index) {

			var modifier = this.path.modifiers[index];
			bool isSelected = index == this.selectedModifier;

			int nextNodeIndex = (modifier.edgeIndex + 1) % this.path.GetNodeCount();

			var prevNode = this.path.GetNodeWorldPosition(modifier.edgeIndex);
			var nextNode = this.path.GetNodeWorldPosition(nextNodeIndex);
			var delta = nextNode - prevNode;

			Vector3 handlePosition = prevNode + delta * modifier.edgeT;

			Handles.color = Color.white;

			bool isDirty = false;

			if (!isSelected) {
				if (Handles.Button(handlePosition, Quaternion.identity, MODIFIER_CAP_SIZE, MODIFIER_CAP_SIZE + CAP_SELECTION_EXTRA_SIZE, Handles.SphereHandleCap)) {
					SelectModifier(index);
				}
			}
			else {
				Handles.color = Color.yellow;
				Vector3 newHandlePosition = Handles.FreeMoveHandle(handlePosition, Quaternion.identity, MODIFIER_CAP_SIZE, Vector3.zero, Handles.SphereHandleCap);

				if (newHandlePosition != handlePosition) {
					var sceneCamera = SceneView.lastActiveSceneView.camera;

					Ray ray = new Ray(sceneCamera.transform.position, newHandlePosition - sceneCamera.transform.position);
					UpdateModifierPosition(modifier, ray);

					isDirty = true;
				}

				var result = modifier.DrawEditorSceneUI(handlePosition, this.path);
				if (result == ScriptedPathEdgeModifier.GuiEventResult.SetDirty) {
					isDirty = true;
				}
				else if(result == ScriptedPathEdgeModifier.GuiEventResult.Remove) {
					RemoveModifier(index);
				}
			}

			if(isDirty) {
				MarkDirty();
			}
		}

		void SelectModifier(int index) {
			this.selectedModifier = index;
			DeselectNode();
		}

		void DeselectModifier() {
			this.selectedModifier = -1;
		}

		void UpdateModifierPosition(ScriptedPathEdgeModifier modifier, Ray ray) {
			float closestDistance = float.MaxValue;
			int closestEdge = -1;
			float closestEdgeT = 0f;

			float distance;
			float segmentT;

			int nEdges = this.path.loop ? this.path.GetNodeCount() : this.path.GetNodeCount() - 1;

			for (int i = 0; i < nEdges; i++) {
				int nextIndex = (i + 1) % this.path.GetNodeCount();
				var n1 = this.path.GetNodeWorldPosition(i);
				var edge = this.path.GetNodeWorldPosition(nextIndex) - n1;

				if (ClosestPointsOnTwoLines(out distance, out segmentT, ray.origin, ray.direction, n1, edge)) {
					if (distance < closestDistance) {
						closestDistance = distance;
						closestEdge = i;
						closestEdgeT = segmentT;
					}
				}
			}

			if (closestEdge >= 0) {
				modifier.edgeIndex = closestEdge;
				modifier.edgeT = closestEdgeT;
			}
		}

		// Modified, based on http://wiki.unity3d.com/index.php/3d_Math_functions
		public static bool ClosestPointsOnTwoLines(out float distance, out float lineSegT, Vector3 linePoint, Vector3 lineVec, Vector3 lineSegPoint, Vector3 lineSegVec) {

			distance = float.MaxValue;
			lineSegT = 0f;

			float a = Vector3.Dot(lineVec, lineVec);
			float b = Vector3.Dot(lineVec, lineSegVec);
			float e = Vector3.Dot(lineSegVec, lineSegVec);

			float d = a * e - b * b;

			//lines are not parallel
			if (d != 0.0f) {

				Vector3 r = linePoint - lineSegPoint;
				float c = Vector3.Dot(lineVec, r);
				float f = Vector3.Dot(lineSegVec, r);

				float s = (b * f - c * e) / d;
				float t = (a * f - c * b) / d;

				lineSegT = Mathf.Clamp01(t);

				var closestPointLine1 = linePoint + lineVec * s;
				var closestPointLine2 = lineSegPoint + lineSegVec * lineSegT;

				distance = Vector3.Distance(closestPointLine1, closestPointLine2);

				return true;
			}

			else {
				return false;
			}
		}

		void AddNode(int afterIndex, Vector3 worldPosition) {

			var localPosition = this.path.worldToLocal.MultiplyPoint(worldPosition);
			var newNode = new ScriptedPathNode(localPosition, this.path.pathNodes[this.path.pathNodes.Count-1]);
			this.path.pathNodes.Insert(afterIndex + 1, newNode);

			foreach(var modifier in this.path.modifiers) {
				if(modifier.edgeIndex == afterIndex) {
					// Modifier is on the edge that is being split
					if(modifier.edgeT > 0.5f) {
						modifier.edgeIndex++;
						modifier.edgeT = (modifier.edgeT - 0.5f)*2;
					}
					else {
						modifier.edgeT = modifier.edgeT * 2;
					}
				}
				else if(modifier.edgeIndex > afterIndex) {
					// Modifier is on an edge after the one being split
					modifier.edgeIndex++;
				}
			}

			SelectNode(afterIndex + 1);
		}

		void AddModifier<T>(int edgeIndex) where T : ScriptedPathEdgeModifier, new() {
			var modifier = new T();
			modifier.edgeIndex = edgeIndex;

			this.path.modifiers.Add(modifier);

			SelectModifier(this.path.modifiers.Count - 1);
		}


		void RemoveNode(int index) {
			this.scheduledDeleteNode = index;
		}

		void RemoveModifier(int index) {
			this.scheduledDeleteModifier = index;
		}

		private void SelectNode(int index) {
			this.selectedNode = index;
			this.showAddPanel = false;
			DeselectModifier();
		}

		

		bool MatchesSpeed(float value, float target) {
			return Mathf.Abs(target - value) < MATCH_SPEED_DIFFERENCE;
		}
	}
}
