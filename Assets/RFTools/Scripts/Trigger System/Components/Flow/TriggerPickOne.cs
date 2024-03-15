using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Flow/Trigger Pick One")]
	[TriggerDoc("When triggered, sends a signal to a single, random destination")]
	public partial class TriggerPickOne : TriggerReceiver
    {
		[SignalDoc("Sent when triggered, but only to ONE random destination.")]
		[AutoPopulateChildReceivers]
		public TriggerSend sends;
    }
}