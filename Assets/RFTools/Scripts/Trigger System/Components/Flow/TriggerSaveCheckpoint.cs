using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Flow/Trigger Save Checkpoint")]
	[TriggerDoc("When triggered, Saves the current level checkpoint")]
	public partial class TriggerSaveCheckpoint : TriggerReceiver
	{
		public ScriptedGameMode.Checkpoint checkpoint;
		public bool onlyIncrease = true;
	}
}