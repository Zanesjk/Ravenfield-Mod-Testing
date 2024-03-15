using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [AddComponentMenu("Trigger/AI/Trigger Land Helicopter")]
    [TriggerDoc("When Triggered, The squad will land their helicopter at the specified position. Create a new trigger with type set to TakeOff to trigger a take off.")]
    public partial class TriggerLandHelicopter : TriggerReceiver
    {
        public enum Type
		{
            Land,
            TakeOff,
            CancelLanding,
		}

        public SquadReference squad;

        public Type type = Type.Land;
        [ConditionalField("type", Type.Land)] public Transform landPosition;

        [SignalDoc("Sent when the helicopter has completed its landing", squad = "The squad piloting the vehicle", vehicle = "The helicopter")]
        [ConditionalField("type", Type.Land)] public TriggerSend onLanded;
	}
}