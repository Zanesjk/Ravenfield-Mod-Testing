using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	public class FireWeaponModifier : ScriptedPathEdgeModifier
	{
		public float holdFireTime = 1f;

#if UNITY_EDITOR
		public override bool DrawEditorGUI() {

			bool dirty = false;

			GUILayout.Label(string.Format("HoldTime: {0:0.0}", this.holdFireTime), ScriptedPath.Debug.BODY_GUI_STYLE);
			var newHoldFireTime = GUILayout.HorizontalSlider(this.holdFireTime, 0f, 5f);

			if (newHoldFireTime != this.holdFireTime) {
				this.holdFireTime = newHoldFireTime;
				dirty = true;
			}

			return dirty;
		}
#endif

		public override void OnPassed(ScriptedPathSeeker seeker) {
			seeker.callbackTarget.OnScriptedPathFireWeapon(this.holdFireTime);
		}
	}
}