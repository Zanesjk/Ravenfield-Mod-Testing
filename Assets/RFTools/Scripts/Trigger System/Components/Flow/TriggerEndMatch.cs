using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Flow/Trigger End Match")]
	[TriggerDoc("When Triggered, Plays the Victory/Defeat screen depending on who won the match.")]
	public partial class TriggerEndMatch : TriggerReceiver
	{
		public Team winner;
		
		public enum SequenceType
		{
			PlayVictoryScreen,
			PlayVictoryScreenForceEnd,
			InstantlyExitScene,
			DoNothing,
		}

		public SequenceType sequence;
	}
}