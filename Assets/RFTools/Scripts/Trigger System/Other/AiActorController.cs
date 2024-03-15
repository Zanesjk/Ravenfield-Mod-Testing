using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Ravenfield.Trigger;

public class AiActorController {

	public const float NORMAL_WALK_SPEED = 3.2f;
	public const float LOOK_TOWARDS_TARGET_DAMP = 0.005f;

	public enum SkillLevel
	{
		Beginner = 0,
		Normal = 1,
		Veteran = 2,
		Elite = 3,
	}

	[System.Serializable]
	public struct Modifier {
		public float meleeChargeRange;
		public bool canSprint;
		public bool ignoreFovCheck;
		public bool dieOnMovementFail;
		public bool alwaysChargeTarget;
        public bool showKillMessage;
		public bool canJoinPlayerSquad;
		public float maxDetectionDistance;
		public float vehicleTopSpeedMultiplier;

		public static Modifier Default {
			get {
				return new Modifier() {
					meleeChargeRange = 30f,
					canSprint = true,
					ignoreFovCheck = false,
					dieOnMovementFail = false,
					alwaysChargeTarget = false,
					showKillMessage = true,
					canJoinPlayerSquad = true,
					maxDetectionDistance = 1000f,
					vehicleTopSpeedMultiplier = 1f,
				};
			}
		}
	}

	[System.Serializable]
    public class LoadoutPickStrategy {

        public WeaponManager.WeaponEntry.LoadoutType type;
        public WeaponManager.WeaponEntry.Distance distance;

        public LoadoutPickStrategy() {
            this.type = WeaponManager.WeaponEntry.LoadoutType.Normal;
            this.distance = WeaponManager.WeaponEntry.Distance.Any;
        }

        public LoadoutPickStrategy(WeaponManager.WeaponEntry.LoadoutType type, WeaponManager.WeaponEntry.Distance distance) {
            this.type = type;
            this.distance = distance;
        }
    }
}
