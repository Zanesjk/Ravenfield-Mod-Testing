using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Ravenfield.Data;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Spawn/Trigger Spawn Squad")]
	[TriggerDoc("When triggered, Spawns an AI squad. SpawnSource is used to control which actors can be spawned:\n" +
		"RespawnOrCreateNew: Respawns a dead actor or creates a new actor if none is available.\n" +
		"CreateNew: Always creates a new actor.")]
	public partial class TriggerSpawnSquad : TriggerReceiver
	{
		public enum RespawnType
		{
			Never,
			Automatically,
		}

		public enum CapType
		{
			IgnoreCap,
			AwaitCap,
			DontSpawn,
		}

		public enum SpawnSource
		{
			RespawnOrCreateNew,
			CreateNew,
		}

		[Header("Actors")]
		public Team team;
		public SpawnSource spawnSource = SpawnSource.RespawnOrCreateNew;
		public RespawnType respawnType = RespawnType.Never;
		[ConditionalField("respawnType", RespawnType.Automatically)] public float autoRespawnTime = 10f;

		public int squadMembersToSpawn = 4;
		public ScriptedPathGroup pathGroup;
		public bool spawnAtPathGroupNodes;

		[Header("Enemy Detection")]
		public TriggerDetectionGroup detectionGroup;

		[SignalDoc("Sent when a squad spawned by this component is alerted", squad = "The alerted squad")]
		public TriggerSend onSquadAlerted;

		[Header("Squad AI State")]
		public SquadState squadState;
		public SpawnInfo[] squadMemberInfo = { SpawnInfo.Default };
		public AiInfo[] squadMemberAIInfo = { AiInfo.Default };

		[Header("Cap Actor Count")]
		public CapType aliveActorsOnTeamCap;
		[ConditionalField("aliveActorsOnTeamCap", CapType.IgnoreCap, invert = true)] public int capCount = 30;

		[SignalDoc("Sent when a squad is spawned", squad = "The spawned squad")]
		public TriggerSend onSpawnCompleteTrigger;

		[System.Serializable]
		public struct SquadState
		{
			public static SquadState Default = new SquadState() {
				isAlert = true,
				walkWhileUnalerted = true,
				slowDetection = false,
				leavePathGroupWhenAlerted = false,
				allowLeaveVehicleWhenStuck = true,
				followersInvisibleToUnalertedEnemies = true,
			};

			public bool isAlert;
			public bool walkWhileUnalerted;
			public bool slowDetection;
			public bool leavePathGroupWhenAlerted;
			public bool allowLeaveVehicleWhenStuck;
			public bool followersInvisibleToUnalertedEnemies;
			public Squad.EngagementRule engagementRule;

			public OrderDefinition order;
		}

		[System.Serializable]
		public partial struct SpawnInfo
		{
			public static SpawnInfo Default {
				get {
					return new SpawnInfo() {
						loadout = LoadoutInfo.Default,
						hp = 100f,
						dropsAmmoWhenKicked = true,
						canDeployParachute = true,
					};
				}
			}

			public string overrideName;
			public Transform overrideSpawnPoint;
			public LoadoutInfo loadout;

			public ActorSkinData overrideSkin;

			public EquippedSlot equipped;
			public Health health;
			public float hp;

			public bool attackersIgnoreEngagementRule;
			public bool dropsAmmoWhenKicked;
			public bool canDeployParachute;

			public enum Health
			{
				Normal,
				HeroArmor,
				Invulnerable,
			}

			public enum EquippedSlot
			{
				Primary,
				Secondary,
				Gear1,
				Gear2,
				Gear3,
				Nothing,
			}

			[System.Serializable]
			public partial struct LoadoutInfo
			{
				public static LoadoutInfo Default {
					get {
						return new LoadoutInfo() {
							useAutoPick = true,
						};
					}
				}

				public bool useAutoPick;
				public AiActorController.LoadoutPickStrategy autoPickStrategy;
				public string primary, secondary, gear1, gear2, gear3;
				public WeaponStates weaponStates;

				[System.Serializable]
				public struct WeaponStates
				{
					public WeaponState primary, secondary, gear1, gear2, gear3;
				}
			}
		}

		[System.Serializable]
		public struct AiInfo
		{
			public static AiInfo Default {
				get {
					return new AiInfo() {
						skill = AiActorController.SkillLevel.Normal,
						modifier = AiActorController.Modifier.Default,
					};
				}
			}

			public AiActorController.SkillLevel skill;
			public AiActorController.Modifier modifier;
		}
	}

}
