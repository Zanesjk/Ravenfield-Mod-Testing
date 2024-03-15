using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Ravenfield.Trigger
{
	[CustomEditor(typeof(TriggerVolume))]
	public class TriggerVolumeEditor : Editor
	{
		TriggerVolume volume;

		TriggerOnActorEnter onEnterComponent;

		void OnEnable() {
			this.volume = (TriggerVolume)this.target;
			this.volume.UpdateTransformData();

			this.onEnterComponent = this.volume.GetComponent<TriggerOnActorEnter>();
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			if(this.onEnterComponent == null) {
				if(GUILayout.Button("Add OnEnter Event Component")) {
					AddEventComponent();
				}
			}
		}

		void AddEventComponent() {
			this.onEnterComponent = Undo.AddComponent<TriggerOnActorEnter>(this.volume.gameObject);
		}

		void OnSceneGUI() {

			var e = Event.current;

			Vector3 projectedPosition;
			bool rayHit = SolveMouseRay(out projectedPosition);

			this.volume.UpdateTransformDataIfNeeded();
			
			//DrawBoundingBoxes();

			float floorLocalY = -this.volume.data.floor;
			float ceilingLocalY = this.volume.data.ceiling;

			int scheduleDeleteVertex = -1;
			int scheduleAddVertex = -1;
			Vector3 addVertexPoint = Vector3.zero;

			var localToWorld = this.volume.LocalToWorldMatrix();

			Undo.RecordObject(this.volume, "Update Trigger Volume");

			for (int i = 0; i < this.volume.data.vertices.Count; i++) {
				var from = this.volume.GetLocalVertexPosition(i);
				var to = this.volume.GetLocalVertexPosition((i + 1) % this.volume.data.vertices.Count);

				Handles.color = Color.red;
				Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
				Handles.matrix = Matrix4x4.identity;

				var fromWorldPos = localToWorld.MultiplyPoint(from);
				var toWorldPos = localToWorld.MultiplyPoint(to);
				var halfwayWorldPos = (fromWorldPos + toWorldPos) / 2;

				var size = HandleUtility.GetHandleSize(fromWorldPos) * 0.2f;

				bool showNodeButtons = e.control && !e.shift;

				if (!showNodeButtons) {
					Vector3 newWorldPos = Handles.FreeMoveHandle(fromWorldPos, Quaternion.identity, size, Vector3.zero, Handles.SphereHandleCap);

					if (newWorldPos != fromWorldPos) {
						if (rayHit) {
							this.volume.SetVertexPosition(i, projectedPosition);
						}
					}
				}
				else {
					Handles.SphereHandleCap(-1, fromWorldPos, Quaternion.identity, size, EventType.Repaint);

					Handles.BeginGUI();

					if(ShowButton(fromWorldPos, "DEL", new Vector2(40f, 30f))) {
						scheduleDeleteVertex = i;
					}

					if (ShowButton(halfwayWorldPos, "+", new Vector2(40f, 30f))) {
						scheduleAddVertex = i+1;
						addVertexPoint = halfwayWorldPos;
					}

					Handles.EndGUI();
				}

				Handles.matrix = localToWorld;
				Handles.DrawLine(from, to);

				var floorFrom = from;
				floorFrom.y = floorLocalY;

				var floorTo = to;
				floorTo.y = floorLocalY;

				var ceilFrom = from;
				ceilFrom.y = ceilingLocalY;

				var ceilTo = to;
				ceilTo.y = ceilingLocalY;

				Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
				Handles.color = new Color(1f, 0f, 0f, 0.7f);
				bool drawCeiling = this.volume.data.ceiling > 0f;
				bool drawFloor = this.volume.data.floor > 0f;
				if (drawCeiling) {
					Handles.DrawLine(ceilFrom, ceilTo);
				}
				if (drawFloor) {
					Handles.DrawLine(floorFrom, floorTo);
				}

				Handles.DrawLine(floorFrom, ceilFrom);
			}
			Handles.matrix = Matrix4x4.identity;

			if(scheduleDeleteVertex >= 0) {
				this.volume.data.vertices.RemoveAt(scheduleDeleteVertex);
			}
			else if(scheduleAddVertex >= 0) {
				this.volume.data.vertices.Insert(scheduleAddVertex, Vector2.zero);
				this.volume.data.SetVertexPosition(scheduleAddVertex, addVertexPoint);
			}
		}

		bool ShowButton(Vector3 worldPoint, string label, Vector2 size) {

			var guiPoint = HandleUtility.WorldToGUIPoint(worldPoint);

			var areaRect = new Rect(guiPoint - size/2, size);
			GUILayout.BeginArea(areaRect);
			var result = GUILayout.Button(label);

			GUILayout.EndArea();

			return result;
		}

		public static void DrawOutline(TriggerVolume volume) {

			volume.UpdateTransformDataIfNeeded();

			Handles.matrix = volume.LocalToWorldMatrix();
			Handles.color = Color.red;
			Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;

			for (int i = 0; i < volume.data.vertices.Count; i++) {
				var from = volume.GetLocalVertexPosition(i);
				var to = volume.GetLocalVertexPosition((i + 1) % volume.data.vertices.Count);

				Handles.DrawLine(from, to);
			}
		}

		private void DrawBoundingBoxes() {
			Handles.color = Color.white;

			Matrix4x4 worldBoundsMatrix = Matrix4x4.TRS(this.volume.data.worldBounds.center, Quaternion.identity, this.volume.data.worldBounds.extents * 2);
			Matrix4x4 localBoundsMatrix = this.volume.data.worldToLocalMatrix.inverse * Matrix4x4.TRS(this.volume.data.localBounds.center, Quaternion.identity, this.volume.data.localBounds.extents * 2);

			Handles.matrix = worldBoundsMatrix;
			Handles.DrawWireCube(Vector3.zero, Vector3.one);

			Handles.matrix = localBoundsMatrix;
			Handles.DrawWireCube(Vector3.zero, Vector3.one);
		}

		bool SolveMouseRay(out Vector3 projectedPosition) {
			UnityEngine.Plane plane = new UnityEngine.Plane(this.volume.transform.up, this.volume.transform.position);
			float t;
			projectedPosition = Vector3.zero;
			
			var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
			bool rayHit = plane.Raycast(ray, out t);

			if (rayHit) {
				projectedPosition = ray.origin + ray.direction * t;
			}

			return rayHit;
		}
	}
}