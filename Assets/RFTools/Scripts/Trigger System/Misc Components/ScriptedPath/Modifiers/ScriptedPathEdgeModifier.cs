using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[System.Serializable]
	public class ScriptedPathEdgeModifier : ScriptableObject
	{
		public enum GuiEventResult
		{
			None,
			SetDirty,
			Remove,
		};

		public int edgeIndex;
		public float edgeT = 0.5f;

		public static int Compare(ScriptedPathEdgeModifier x, ScriptedPathEdgeModifier y) {
			return x.GetSequenceValue().CompareTo(y.GetSequenceValue());
		}

		public float GetSequenceValue() {
			return this.edgeIndex + this.edgeT;
		}

		public virtual GuiEventResult DrawEditorSceneUI(Vector3 handlePosition, ScriptedPath path) {
#if UNITY_EDITOR
			var guiRect = GenerateGUIRect(handlePosition, new Vector2(150f, 100f));

			UnityEditor.Handles.BeginGUI();
			UnityEditor.EditorGUI.DrawRect(guiRect, new Color(0f, 0f, 0f, 0.7f));
			GUILayout.BeginArea(guiRect);

			GUILayout.Label(this.GetType().Name, ScriptedPath.Debug.TITLE_GUI_STYLE);
			GUILayout.Space(5f);

			bool dirty = DrawEditorGUI();

			GUILayout.FlexibleSpace();

			bool remove = GUILayout.Button("Remove");

			GUILayout.EndArea();
			UnityEditor.Handles.EndGUI();

			if(remove) {
				return GuiEventResult.Remove;
			}
			else if(dirty) {
				return GuiEventResult.SetDirty;
			}
			else {
				return GuiEventResult.None;
			}
#else
			return GuiEventResult.None;
#endif
		}

		public virtual bool DrawEditorGUI() {
			return false;
		}

#if UNITY_EDITOR
		public Rect GenerateGUIRect(Vector3 handlePosition, Vector2 size) {
			var guiPoint = UnityEditor.HandleUtility.WorldToGUIPoint(handlePosition);
			var guiRect = new Rect(guiPoint - new Vector2(size.x / 2, size.y + 20f), size);
			return guiRect;
		}
#endif

		public virtual void OnPassed(ScriptedPathSeeker seeker) {
			
		}
	}
}