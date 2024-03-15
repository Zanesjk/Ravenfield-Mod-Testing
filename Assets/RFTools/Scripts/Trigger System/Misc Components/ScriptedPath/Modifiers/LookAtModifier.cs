using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ravenfield.Trigger {

	[System.Serializable]
	public class LookAtModifier : ScriptedPathEdgeModifier {

		public enum Target
		{
			Body,
			Head,
		}

		public bool stop = false;
		public Vector3 lookAtPoint;
		public Target target;

#if UNITY_EDITOR
		public override GuiEventResult DrawEditorSceneUI(Vector3 handlePosition, ScriptedPath path) {
			GuiEventResult result = base.DrawEditorSceneUI(handlePosition, path);

			if (result == GuiEventResult.Remove) return result;

			Vector3 worldPosition = path.localToWorld.MultiplyPoint(this.lookAtPoint);

			if (!this.stop) {
				Handles.zTest = UnityEngine.Rendering.CompareFunction.Less;
				Handles.color = this.target == Target.Body ? Color.red : Color.blue;
				Handles.DrawDottedLine(handlePosition, worldPosition, 5f);
				Handles.CubeHandleCap(-1, worldPosition, Quaternion.identity, 0.4f, EventType.Repaint);

				Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
				var newWorldPosition = Handles.PositionHandle(worldPosition, Quaternion.identity);

				if (newWorldPosition != worldPosition) {
					this.lookAtPoint = path.worldToLocal.MultiplyPoint(newWorldPosition);
					result = GuiEventResult.SetDirty;
				}
			}

			return result;
		}

		public override bool DrawEditorGUI() {
			bool dirty = base.DrawEditorGUI();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Type", ScriptedPath.Debug.BODY_GUI_STYLE);
			if (GUILayout.Button(this.target.ToString())) {
				if (this.target == Target.Body) {
					this.target = Target.Head;
				}
				else {
					this.target = Target.Body;
				}
				dirty = true;
			}
			GUILayout.EndHorizontal();

			GUILayout.Space(5f);

			var newCancel = ScriptedPath.Debug.Toggle(this.stop, "Stop LookAt");
			if (newCancel != this.stop) {
				this.stop = newCancel;
				dirty = true;
			}

			return dirty;
		}
#endif

		public override void OnPassed(ScriptedPathSeeker seeker) {
			if(this.stop) {
				seeker.CancelOverrideLookAtPoint(this.target);
			}
			else {
				seeker.OverrideLookAtPoint(seeker.path.localToWorld.MultiplyPoint(this.lookAtPoint), this.target);
			}
		}
	}
}