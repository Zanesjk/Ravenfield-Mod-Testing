using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Flow/Trigger Random")]
	[TriggerDoc("When triggered, sends OnPass based on Chance value, otherwise sends onFail")]
	public partial class TriggerRandom : TriggerReceiver
	{
		[Range(0f, 1f)] public float chance = 0.5f;
		
		[SignalDoc("Sent on success")]
		public TriggerSend onPass;

		[SignalDoc("Sent on fail")]
		public TriggerSend onFail;
	}
}