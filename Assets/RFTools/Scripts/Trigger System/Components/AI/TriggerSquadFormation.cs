using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/AI/Trigger Squad Formation")]
	[TriggerDoc("When Triggered, Changes the formation of the squad")]
	public partial class TriggerSquadFormation : TriggerReceiver
	{
		public Squad.FormationType formation;
		public float formationWidth = 2f;
		public float formationDepth = 2f;

		[ConditionalField("formation", Squad.FormationType.Custom)] public Vector2[] customFormation;

		public SquadReference squad;
	}
}
