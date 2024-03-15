using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Ravenfield.Trigger
{
	[System.Serializable]
	public struct SquadReference
	{
		public static Squad lastSpawnedSquad;

		public enum Type
		{
			Spawner,
			PlayerSquad,
			FindClosest,
			LastSpawned,
			FromSignal,
			PickRandomOnTeam,
		}

		public Type type;
		[ConditionalField("type", Type.Spawner)] public TriggerSpawnSquad spawner;

		[ConditionalField("type", Type.FindClosest)] public Transform closestToPoint;
		[ConditionalField("type", Type.FindClosest, Type.PickRandomOnTeam)] public SquadFilter filter;
		[ConditionalField("type", Type.FindClosest)] public float maxDistance;
		[ConditionalField("type", Type.PickRandomOnTeam)] public Team team;
	}
}
