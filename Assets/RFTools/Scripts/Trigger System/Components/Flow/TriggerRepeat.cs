using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Flow/Trigger Repeat")]
	[TriggerDoc("When triggered, Repeatedly sends a signal. Cancel the repeated sequence by deactivating this component.")]
	public partial class TriggerRepeat : TriggerReceiver
	{
		public bool repeatForever = false;
		[ConditionalField("repeatForever", false)] public int repeatCount = 10;
		public float repeatDelay = 0f;

		[Header("Sends")]
		[SignalDoc("Sent repeatedly")]
		public TriggerSend send;

		[SignalDoc("Sent once after sequence is completed")]
		public TriggerSend finallySend;
	}
}