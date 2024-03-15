using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[TriggerDoc("When Triggered, all members in FromSquad joins ToSquad.")]
	[AddComponentMenu("Trigger/AI/Trigger Join Squad")]
    public partial class TriggerJoinSquad : TriggerReceiver
    {
		public SquadReference fromSquad;
		public SquadReference toSquad;

		[SignalDoc("Sent when squads are joined", squad = "The resulting squad")]
		public TriggerSend onSquadsJoined;
    }
}