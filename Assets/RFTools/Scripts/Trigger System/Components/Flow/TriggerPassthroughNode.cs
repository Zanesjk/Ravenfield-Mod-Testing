using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Flow/Trigger Passthrough Node")]
	[TriggerDoc("When triggered, sends OnTriggered. This component makes it easy to trigger the same output triggers from multiple inputs.")]
	public partial class TriggerPassthroughNode : TriggerReceiver
    {
		[Header("Sends")]
		[SignalDoc("Propagated signal")]
		[AutoPopulateChildReceivers] public TriggerSend onTriggered;
    }
}