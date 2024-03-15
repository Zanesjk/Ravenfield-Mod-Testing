using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [AddComponentMenu("Trigger/Flow/Trigger On Start")]
	[TriggerDoc("Sends OnTriggered either on Start (Triggeres once, when the object is activated) or OnEnable (Triggered every time the object is activated). When TestModeOnly is set, OnTriggered will only be sent when launching the game via the RF Tools in the Unity Editor.")]
	public partial class TriggerOnStart : TriggerBaseComponent
    {
        public enum Type
		{
            OnStart,
            OnEnable,
		}

		public Type type;

		[SignalDoc("Sent on Start or OnEnable")]
		[AutoPopulateChildReceivers]
		public TriggerSend onTriggered;

		public bool testModeOnly = false;
	}
}