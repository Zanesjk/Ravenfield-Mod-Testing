using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	public class LeanModifier : ScriptedPathEdgeModifier
	{
		public float lean;

#if UNITY_EDITOR
		public override bool DrawEditorGUI() {

			bool dirty = false;

			string label = "";
			if(lean < -0.1f) {
				label = "(Left)";
			}
			else if(lean > 0.1f) {
				label = "(Right)";
			}

			GUILayout.Label(string.Format("Lean: {0:0.0} {1}", this.lean, label), ScriptedPath.Debug.BODY_GUI_STYLE);
			var newLean = GUILayout.HorizontalSlider(this.lean, -1f, 1f);

			if(newLean != this.lean) {
				this.lean = newLean;
				dirty = true;
			}

			return dirty;
		}
#endif

		public override void OnPassed(ScriptedPathSeeker seeker) {
			seeker.modifierData.lean = this.lean;
		}
	}
}