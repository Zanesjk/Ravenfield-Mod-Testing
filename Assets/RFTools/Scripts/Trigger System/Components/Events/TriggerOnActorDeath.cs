using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [AddComponentMenu("Trigger/Events/Trigger On Actor Death")]
    [TriggerDoc("Sends OnDeath when an actor from the specified actor group dies.")]
    public partial class TriggerOnActorDeath : TriggerBaseComponent
    {
        public ActorFilter filter;
        public bool ignoreSilentKills = false;

        [SignalDoc("Sent when an actor dies", actor = "The dying actor", squad = "The squad the actor belonged to")]
        public TriggerSend onDeath;
    }
}