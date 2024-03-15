using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Actor/Trigger Ragdoll")]
	[TriggerDoc("When triggered, makes a target actor ragdoll and applies a force.")]
	public partial class TriggerRagdoll : TriggerReceiver
    {
        public Vector3 localForce = Vector3.up;
		public ActorReference actor;
	}
}