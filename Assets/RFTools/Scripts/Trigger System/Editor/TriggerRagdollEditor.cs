using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Ravenfield.Trigger
{
	[CustomEditor(typeof(TriggerRagdoll))]
	public class TriggerRagdollEditor : TriggerEditor
	{

		const float FORCE_SCALE_WORLD_UNITS = 0.02f;

		public override void OnSceneGUI() {
			base.OnSceneGUI();

			var trigger = (TriggerRagdoll)this.component;

			if(trigger.localForce == Vector3.zero) {
				return;
			}

			var triggerRotation = trigger.transform.rotation;
			Vector3 worldForce = triggerRotation * trigger.localForce;
			var position = trigger.transform.position;
			var handlePosition = position + worldForce * FORCE_SCALE_WORLD_UNITS;

			Handles.color = Color.red;
			Handles.ConeHandleCap(0, handlePosition, Quaternion.LookRotation(worldForce), 0.4f, EventType.Repaint);
			Handles.DrawDottedLine(position, handlePosition, 5f);
			Handles.color = Color.white;
		}
	}
}