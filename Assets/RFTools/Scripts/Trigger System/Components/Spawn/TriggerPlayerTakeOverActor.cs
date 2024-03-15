using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Spawn/Trigger Player Take Over Actor")]
	[TriggerDoc("When triggered, The player takes over the target actor, silently killing the original actor. The player gets the loadout, health and ammo of the original actor.")]
	public partial class TriggerPlayerTakeOverActor : TriggerReceiver
	{
		[Header("Parameters")]
		public ActorReference actor;

		[Header("Sends")]

		[SignalDoc("Sent when takeover succeeded")]
		public TriggerSend onSuccess;

		[SignalDoc("Sent when takeover failed")]
		public TriggerSend onFail;
	}
}