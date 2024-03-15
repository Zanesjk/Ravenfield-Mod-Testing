using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lua;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Ravenscript/Trigger Scripted Signal Sender")]
	[TriggerDoc("Access this component via Ravenscript to send trigger system signals.")]
	public partial class TriggerScriptedSignal : TriggerBaseComponent, ICompoundTriggerSender
	{
		public Signal[] signals;

		[System.Serializable]
		public struct Signal
		{
			public string name;
			public TriggerSend send;
		}

		[Lua.Ignore]
		public IEnumerable<TriggerSend> GetCompoundSends() {
			foreach (var signal in this.signals) {
				yield return signal.send;
			}
		}
	}
}