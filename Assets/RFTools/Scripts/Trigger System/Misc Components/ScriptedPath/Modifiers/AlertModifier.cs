using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	public class AlertModifier : ScriptedPathEdgeModifier
	{
		public bool isAlert;

#if UNITY_EDITOR
		public override bool DrawEditorGUI() {
			var newAlert = ScriptedPath.Debug.Toggle(this.isAlert, "Is Alert");
			if(this.isAlert != newAlert) {
				this.isAlert = newAlert;
				return true;
			}
			return false;
		}
#endif

		public override void OnPassed(ScriptedPathSeeker seeker) {
			seeker.modifierData.isAlert = this.isAlert;
		}
	}
}