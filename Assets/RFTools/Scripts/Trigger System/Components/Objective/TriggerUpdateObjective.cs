using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Objective/Trigger Update Objective")]
	[TriggerDoc("When triggered, Updates the state of an objective")]
	public partial class TriggerUpdateObjective : TriggerReceiver
    {
		public enum UpdateType
		{
			UpdateInfo,
			Complete,
			Fail,
			SetIncomplete,
			Remove,
		}

		public TriggerCreateObjective objective;
		public UpdateType update;

		[ConditionalField("update", UpdateType.UpdateInfo)]
		public string newText;

		[ConditionalField("update", UpdateType.UpdateInfo)]
		public TriggerCreateObjective.Target newTarget;
	}
}