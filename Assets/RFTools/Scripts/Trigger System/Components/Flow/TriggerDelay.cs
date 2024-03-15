using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Flow/Trigger Delay")]
	[TriggerDoc("When Triggered, Sends OnDelayDone after the specified delay time.")]
	public partial class TriggerDelay : TriggerReceiver
	{
		public enum SignalType
		{
			MultipleSignals,
			OneAtATime,
		}

		public SignalType signalType;
		public float delayTime = 1f;
		public float randomAdd = 0f;

		[SignalDoc("Sent after delay has passed", overrideName = "On Delay Done")]
		[InspectorName("On Delay Done")] public TriggerSend onDelayDoneTrigger;
	}
}