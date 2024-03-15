using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [System.Serializable]
    public struct ActorReference
    {
        public enum Type
		{
            Player,
            SquadMember,
            VehiclePassenger,
            FromSignal,
            None,
		}

        public Type type;

        [ConditionalField("type", Type.SquadMember)] public SquadReference squad;
        [ConditionalField("type", Type.SquadMember)] public int squadmemberIndex;

        [ConditionalField("type", Type.VehiclePassenger)] public VehicleReference vehicle;
        [ConditionalField("type", Type.VehiclePassenger)] public int seatIndex;
    }
}