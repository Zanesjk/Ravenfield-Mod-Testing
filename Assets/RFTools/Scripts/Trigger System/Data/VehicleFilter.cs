using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [System.Serializable]
    public struct VehicleFilter
    {
        public Filter[] filters;

        [System.Serializable]
        public struct Filter
        {
            public enum Type
            {
                PlayerIsInside,
                OwnedByTeam,
                HasPlayerDriver,
                HasDriver,
                HasAnyoneSeated,
                AllSeatsTaken,
            }

            public Type type;
            [ConditionalField("type", Type.OwnedByTeam)] public Team team;

            public bool invertFilter;
		}
    }
}