using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ravenfield.Trigger;
using System.Linq;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Actor/Trigger Kill Actor")]
	[TriggerDoc("When triggered, Kill the specified actor. Use SilentKill to instantly kill the actor, without creating a ragdoll.")]
	public partial class TriggerKillActor : TriggerReceiver
	{
		public ActorReference actor;
		public bool silentKill;
	}
}