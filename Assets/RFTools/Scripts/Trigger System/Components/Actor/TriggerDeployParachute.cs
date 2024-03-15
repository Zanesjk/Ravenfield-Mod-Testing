using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [AddComponentMenu("Trigger/Actor/Trigger Deploy Parachute")]
    [TriggerDoc("When Triggered, Deploys or cuts an actor's parachute. This action ignores the actor canDeployParachute value.")]
    public partial class TriggerDeployParachute : TriggerReceiver
    {
        public enum Type
		{
            DeployParachute,
            CutParachute,
		};

        public Type type;
        public ActorReference actor;
	}
}
