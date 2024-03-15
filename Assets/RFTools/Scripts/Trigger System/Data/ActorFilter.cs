using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [System.Serializable]
    public struct ActorFilter
    {
        public Filter[] filters;

        [System.Serializable]
        public struct Filter
        {
            public enum Type
            {
                IsPlayer,
                Team,
                InsideVolume,
                IsSeated,
                IsSeatedInVehicle,
                IsDriver,
                IsFallenOver,
                IsSquadMember,
            }

            public Type type;
            [ConditionalField("type", Type.Team)] public Team team;
            [ConditionalField("type", Type.InsideVolume)] public TriggerVolume volume;
            [ConditionalField("type", Type.IsSeatedInVehicle)] public VehicleReference vehicle;
            [ConditionalField("type", Type.IsSquadMember)] public SquadReference squad;

            public bool invertFilter;
		}
    }
}