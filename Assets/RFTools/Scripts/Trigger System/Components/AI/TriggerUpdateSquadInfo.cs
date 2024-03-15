using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ravenfield.Trigger {
	[TriggerDoc("On Trigger, Updates the target squad state.")]
	[AddComponentMenu("Trigger/AI/Trigger Update Squad Info")]
	public partial class TriggerUpdateSquadInfo : TriggerReceiver
	{
		public enum AlertStatus
		{
			Keep,
			MakeAlert,
			MakeNotAlert,
			MakeNotAlertLimitSpeed,
		}

		public enum EngagementRule
		{
			Keep,
			FireAtWill,
			OnlyAlerted,
			HoldFire,
		}

		public SquadReference squad;

		public AlertStatus alertStatus;
		public EngagementRule engagementRule;
	}
}