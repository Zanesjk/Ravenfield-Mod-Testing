using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [System.Serializable]
    public struct WeaponReference
    {
        public enum Type
        {
            ActorActiveWeapon,
            ActorCarriedWeapon,
            VehicleSeatActiveWeapon,
            VehicleSeatCarriedWeapon,
            WeaponInstance,
            FromSignal,
        }

        public enum SubWeaponType
		{
            ActiveSubWeapon,
            AlwaysParentWeapon,
            AlwaysSubWeaponIndex,
		}

        public Type type;

        [Header("Subweapon")]
        public SubWeaponType subWeaponType;
        [ConditionalField("subWeaponType", SubWeaponType.AlwaysSubWeaponIndex)] public int subWeaponIndex;

        [Header("Parameters")]
        [ConditionalField("type", Type.ActorActiveWeapon, Type.ActorCarriedWeapon)] public ActorReference actor;
        [ConditionalField("type", Type.ActorCarriedWeapon)] public TriggerEquipWeapon.SlotTarget carrySlot;
        [ConditionalField("type", Type.VehicleSeatActiveWeapon, Type.VehicleSeatCarriedWeapon)] public VehicleReference vehicle;
        [ConditionalField("type", Type.VehicleSeatActiveWeapon, Type.VehicleSeatCarriedWeapon)] public int vehicleSeatIndex;
        [ConditionalField("type", Type.VehicleSeatCarriedWeapon)] public int seatWeaponIndex;
        [ConditionalField("type", Type.WeaponInstance)] public Weapon weapon;
    }
}
