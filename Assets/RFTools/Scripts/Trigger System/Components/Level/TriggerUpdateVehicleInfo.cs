using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [AddComponentMenu("Trigger/Level/Trigger Update Vehicle Info")]
    [TriggerDoc("When Triggered, Updates the vehicle state.")]
    public partial class TriggerUpdateVehicleInfo : TriggerReceiver
    {
        public VehicleReference vehicle;

		public enum HealthUpdate
		{
            Keep,
            Normal,
            Invulnerable,
		};

        public enum HealthChange
        {
            Keep,
            Repair,
            Damage,
            Burn,
            Destroy,
        }

        public enum LockUpdate
		{
            Keep,
            Locked,
            Unlocked,
		}

        public enum CanBeTakenOverByPlayerSquad
        {
            Keep,
            Allow,
            Disallow,
		}

        public enum PlayerSeatChange
		{
            Keep,
            Allow,
            DisallowSwapWithOtherActor,
            Disallow,
		}

        [InspectorName("Health Type")] public HealthUpdate health;
        public HealthChange healthChange;

        [ConditionalField("health", HealthChange.Repair, HealthChange.Damage)]
        public float amount;

        [ConditionalField("health", HealthChange.Damage, HealthChange.Burn, HealthChange.Destroy)]
        public ActorReference damageCredit;

        public LockUpdate locked;
        public CanBeTakenOverByPlayerSquad canBeTakenOverByPlayerSquad;

        public PlayerSeatChange playerSeatChange;
    }
}