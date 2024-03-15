using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Spawn/Trigger Spawn Vehicle")]
	[TriggerDoc("When triggered, Spawns a vehicle on the specified VehicleSpawner")]
	public partial class TriggerSpawnVehicle : TriggerReceiver
	{
		[Header("Parameters")]
		public Team team;
		public VehicleSpawner vehicleSpawner;
		[ConditionalField("vehicleSpawner", null)] public TurretSpawner turretSpawner;
		public VehicleInfo vehicleInfo = VehicleInfo.Default;

		[Header("Sends")]

		[SignalDoc("Sent when a vehicle is spawned", vehicle = "The spawned vehicle")]
		public TriggerSend onSpawnCompleteTrigger;

		[System.Serializable]
		public partial struct VehicleInfo
		{
			public enum PlayerSeatChange
			{
				Allowed,
				DisallowSwapWithOtherActor,
				Disallow,
			}

			static public VehicleInfo Default {
				get {
					return new VehicleInfo() {
						isInvulnerable = false,
						isLocked = false,
						canBeTakenOverByPlayerSquad = true,
					};
				}
			}

			public bool isInvulnerable;
			public bool isLocked;
			public bool canBeTakenOverByPlayerSquad;
			public PlayerSeatChange playerSeatChange;
		}
	}
}