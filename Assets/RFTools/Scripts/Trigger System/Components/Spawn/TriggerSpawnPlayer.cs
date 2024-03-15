using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Spawn/Trigger Spawn Player")]
	[TriggerDoc("When triggered, Spawns the player and their squad.")]
	public partial class TriggerSpawnPlayer : TriggerSpawnSquad
	{
		[Header("Player State")]
		public SpawnInfo playerInfo = SpawnInfo.Default;

		[Header("Player Squad Respawning")]
		[ConditionalField("respawnType", TriggerSpawnSquad.RespawnType.Automatically)] public bool silentKillSquadMembersOnRespawn;
	}
}
