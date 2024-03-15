using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ravenfield.Trigger;

namespace Ravenfield.Trigger
{
	public class ScriptedPath : TriggerBaseComponent, ICompoundTriggerSender
	{
		public float gotoStartMovementSpeed = AiActorController.NORMAL_WALK_SPEED;

		public bool limitSpeed = true;
		public bool loop = false;
		public bool startAlert = true;
		public bool stayAtEnd = false;

		public List<ScriptedPathNode> pathNodes = new List<ScriptedPathNode>();
		public List<ScriptedPathEdgeModifier> modifiers = new List<ScriptedPathEdgeModifier>();

		[System.NonSerialized] public Matrix4x4 localToWorld = Matrix4x4.identity;
		[System.NonSerialized] public Matrix4x4 worldToLocal = Matrix4x4.identity;

		[System.NonSerialized] public bool isStatic;

		public ScriptedPath() {
			this.pathNodes = new List<ScriptedPathNode>() {
				new ScriptedPathNode(new Vector3(0f, 0f, -5f)),
				new ScriptedPathNode(new Vector3(0f, 0f, 5f)),
			};
		}

		public Vector3 StartPosition() {
			return GetNodeWorldPosition(0);
		}

		private void Awake() {
			UpdateTransformMatrix();

			this.isStatic = GetComponentInParent<Rigidbody>() == null;
		}

		void FixedUpdate() {
			if (!this.isStatic) {
				UpdateTransformMatrix();
			}
		}

		public void SortModifiers() {
			this.modifiers.Sort(ScriptedPathEdgeModifier.Compare);
		}

		public int GetNodeCount() {
			return this.pathNodes.Count;
		}

		public void UpdateTransformMatrix() {
			this.localToWorld = this.transform.localToWorldMatrix;
			this.worldToLocal = this.transform.worldToLocalMatrix;
		}

		public Vector3 GetNodeWorldPosition(int index) {
			return this.localToWorld.MultiplyPoint(this.pathNodes[index].localPosition);
		}

		public void SetNodeWorldPosition(int index, Vector3 worldPosition) {
			var node = this.pathNodes[index];
			node.localPosition = this.worldToLocal.MultiplyPoint(worldPosition);
			this.pathNodes[index] = node;
		}

		public IEnumerable<TriggerSend> GetCompoundSends() {
			for (int i = 0; i < this.pathNodes.Count; i++) {
				yield return this.pathNodes[i].onReached;
			}
		}

		public static class Debug
		{
			static GUIStyle _titleGuiStyle;
			static GUIStyle _bodyGuiStyle;

			public static GUIStyle TITLE_GUI_STYLE {
				get {
					if (_titleGuiStyle == null) {
						_titleGuiStyle = new GUIStyle() {
							fontSize = 14,
							normal = new GUIStyleState() {
								textColor = Color.white,
							},
						};
					}
					return _titleGuiStyle;
				}
			}

			public static GUIStyle BODY_GUI_STYLE {
				get {
					if (_bodyGuiStyle == null) {
						_bodyGuiStyle = new GUIStyle() {
							normal = new GUIStyleState() {
								textColor = Color.white,
							},
						};
					}
					return _bodyGuiStyle;
				}
			}

			public static bool Toggle(bool value, string label) {
				GUILayout.BeginHorizontal();

				GUILayout.Label(label, ScriptedPath.Debug.BODY_GUI_STYLE);
				GUILayout.FlexibleSpace();
				value = GUILayout.Toggle(value, "");

				GUILayout.EndHorizontal();

				return value;
			}
		}
	}
}