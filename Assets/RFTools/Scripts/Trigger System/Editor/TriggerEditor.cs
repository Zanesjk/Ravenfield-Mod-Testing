using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Ravenfield.Trigger
{
    [CustomEditor(typeof(TriggerBaseComponent), true), CanEditMultipleObjects]
	public class TriggerEditor : Editor
    {
		const float INACTIVE_CONNECTION_ALPHA = 0.3f;

		static bool showNetwork = false;
		static bool showDocDefault = false;

		protected TriggerBaseComponent component;
		TriggerDocAttribute docAttribute;
		TriggerSend.DocInfo[] signalDocs;

		bool showDoc = false;

		List<ConnectionGizmo> highlightConnections;

		private void OnEnable() {
			this.component = this.target as TriggerBaseComponent;

			this.docAttribute = (TriggerDocAttribute) System.Attribute.GetCustomAttribute(this.component.GetType(), typeof(TriggerDocAttribute));
			this.showDoc = showDocDefault;

			FindSignalDocs();

			if (showNetwork) {
				HighlightNetwork();
			}
		}

		void FindSignalDocs() {
			this.signalDocs = this.component.GetSendInfos().ToArray();
		}

		void SelectInputs() {
			var nodes = FindObjectsOfType<TriggerBaseComponent>(true);

			HashSet<GameObject> inputGameObjects = new HashSet<GameObject>();
			foreach (var node in nodes) {
				foreach (var send in node.GetSends()) {
					if (send.destinations != null) {
						foreach (var destination in send.destinations) {
							if (destination != null && destination == this.component) {
								inputGameObjects.Add(node.gameObject);
							}
						}
					}
				}
			}

			if(inputGameObjects.Count > 0) {
				Selection.objects = inputGameObjects.Select(go => (UnityEngine.Object)go).ToArray();
				Debug.Log($"Selected {Selection.objects.Length} input(s)");
			}
			else {
				Debug.Log("No inputs found :(");
			}
		}

		public void HighlightNetwork() {

			showNetwork = true;

			this.highlightConnections = new List<ConnectionGizmo>();
			var nodes = FindObjectsOfType<TriggerBaseComponent>(true);

			foreach(var node in nodes) {
				foreach(var send in node.GetSends()) {
					foreach(var destination in send.destinations) {
						if(destination != null) {

							Color color = Color.white;
							if(node == component) {
								// Outgoing are always drawn
								continue;
							}
							else if(destination == component) {
								// Incoming
								color = Color.blue;
							}

							if(!destination.isActiveAndEnabled || !node.isActiveAndEnabled) {
								color.a = INACTIVE_CONNECTION_ALPHA;
							}

							this.highlightConnections.Add(new ConnectionGizmo(node, destination, color));
						}
					}
				}
			}
			SceneView.RepaintAll();
		}

		public override void OnInspectorGUI() {

			GUILayout.BeginHorizontal();

			if (this.docAttribute != null) {
				if(GUILayout.Button("?")) {
					this.showDoc = !this.showDoc;
					showDocDefault = this.showDoc;
				}

				GUILayout.Space(5);
			}

			if (component is TriggerReceiver) {
				var receiver = (TriggerReceiver)component;
				if (GUILayout.Button("Select Inputs")) {
					SelectInputs();
				}
			}

			GUILayout.FlexibleSpace();

			GUILayout.EndHorizontal();

			if (this.showDoc) {

				var docStyle = new GUIStyle(GUI.skin.GetStyle("label")) {
					wordWrap = true,
					richText = true,
				};

				if (this.docAttribute != null) {
					GUILayout.Label(this.docAttribute.message, docStyle);
					GUILayout.Space(10);
				}

				if(this.signalDocs.Length > 0) {
					EditorGUILayout.LabelField("SENDS");
					GUILayout.Space(5);

					foreach (var s in this.signalDocs) {

						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PrefixLabel(s.name);
						EditorGUILayout.LabelField(s.signalDoc.doc, docStyle);
						EditorGUILayout.EndHorizontal();

						EditorGUI.indentLevel++;
						GUI.color = new Color(0.6f, 0.6f, 0.8f);
						ShowSignalContextDoc("Actor", s.signalDoc.actor, docStyle);
						ShowSignalContextDoc("Squad", s.signalDoc.squad, docStyle);
						ShowSignalContextDoc("Vehicle", s.signalDoc.vehicle, docStyle);
						ShowSignalContextDoc("Weapon", s.signalDoc.weapon, docStyle);
						GUI.color = Color.white;
						EditorGUI.indentLevel--;

						GUILayout.Space(5);
					}

					GUILayout.Space(5);
				}
			}

			base.OnInspectorGUI();

			EditorGUILayout.BeginHorizontal();

			if(GUILayout.Button("Show Outgoing")) {
				showNetwork = false;
				this.highlightConnections = null;
				SceneView.RepaintAll();
			}
			else if (GUILayout.Button("Show Network")) {
				HighlightNetwork();
			}

			EditorGUILayout.EndHorizontal();
		}

		static void ShowSignalContextDoc(string contextType, string doc, GUIStyle style) {
			if (string.IsNullOrEmpty(doc)) return;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(contextType, style);
			EditorGUILayout.LabelField(doc, style);
			EditorGUILayout.EndHorizontal();
		}

		public virtual void OnSceneGUI() {
			if(this.highlightConnections != null) {
				foreach(var gizmo in this.highlightConnections) {
					gizmo.Draw();
				}
			}

			// Draw outgoing connections
			foreach (var send in this.component.GetSends()) {
				if (send.destinations != null) {
					foreach (var destination in send.destinations) {
						if (destination != null) {
							Color c = Color.red;
							if (!this.component.isActiveAndEnabled || !destination.isActiveAndEnabled) {
								c.a = INACTIVE_CONNECTION_ALPHA;
							}

							ConnectionGizmo.Draw(this.component.transform.position, destination.transform.position, c);
						}
					}
				}
			}
		}

		struct ConnectionGizmo
		{
			public Vector3 start, end;
			public Color color;

			public ConnectionGizmo(TriggerBaseComponent from, TriggerBaseComponent to, Color color) {
				this.start = from.transform.position;
				this.end = to.transform.position;
				this.color = color;
			}

			public void Draw() {
				Draw(this.start, this.end, this.color);
			}

			public static void Draw(Vector3 from, Vector3 to, Color color) {
				Vector3 delta = to - from;
				float distance = delta.magnitude;
				Vector3 dir = delta/distance;
				Vector3 right = Vector3.Cross(dir, Vector3.up).normalized;

				float arrowScale = Mathf.Min(10f, distance * 0.05f);
				dir *= arrowScale;
				right *= arrowScale;

				from += dir;
				to -= dir;

				Handles.color = color;
				Handles.DrawPolyLine(from, to, to - dir - right, to, to - dir + right);
			}
		}
	}
}
