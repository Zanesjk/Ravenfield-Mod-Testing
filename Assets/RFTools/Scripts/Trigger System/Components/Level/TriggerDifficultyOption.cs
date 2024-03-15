using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Level/Trigger Difficulty Option")]
	[TriggerDoc("When triggered, sends a signal based on the selected game difficulty.")]
	public partial class TriggerDifficultyOption : TriggerReceiver
	{
		[SignalDoc("Sent if difficulty is set to easy")]
		public TriggerSend easySend;

		[SignalDoc("Sent if difficulty is set to normal")]
		public TriggerSend normalSend;

		[SignalDoc("Sent if difficulty is set to hard")]
		public TriggerSend hardSend;
	}
}