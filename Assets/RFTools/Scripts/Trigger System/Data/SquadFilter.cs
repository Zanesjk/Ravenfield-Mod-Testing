using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [System.Serializable]
    public struct SquadFilter
    {
        public Filter[] filters;

        [System.Serializable]
        public struct Filter
        {
			public enum Type
			{
				IsPlayerSquad,
				MemberCountGreaterOrEqual,
				IsAlert,
				HasVehicle,
				IsOnTeam,
			}

			public Type type;

			[ConditionalField("type", Type.MemberCountGreaterOrEqual)] public int memberCount;
			[ConditionalField("type", Type.IsOnTeam)] public Team team;
			[InspectorName("Invert Condition")] public bool invertFilter;
		}
    }
}
