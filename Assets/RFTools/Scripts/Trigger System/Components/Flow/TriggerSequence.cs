using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Flow/Trigger Sequence")]
	[TriggerDoc("When triggered, sends a signal to a single destination one by one, in order.")]
	public partial class TriggerSequence : TriggerReceiver
	{
		[SignalDoc("Sent to ONE destination, in order.")]
		[AutoPopulateChildReceivers] public TriggerSend sequencedSend;
	}
}