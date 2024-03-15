using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Actor/Trigger Equip Weapon")]
	[TriggerDoc("When triggered, equips the actor with a weapon. If Replace Existing is off, this can fail if the target slot is already occupied by another weapon.")]
	public partial class TriggerEquipWeapon : TriggerReceiver
	{
		public enum SlotTarget
		{
			Primary,
			Secondary,
			Gear1,
			Gear2,
			Gear3,
		}

		public enum WeaponType
		{
			ByName,
			FromWeaponEntry,
		}

		[Header("Parameters")]
		public ActorReference actor;
		public SlotTarget targetSlot;

		public bool replaceExisting = true;
		public bool automaticallySwitch = true;

		public WeaponType weaponType;
		[ConditionalField("weaponType", WeaponType.ByName)] public string weaponName;
		[ConditionalField("weaponType", WeaponType.FromWeaponEntry)] public WeaponManager.WeaponEntry weaponEntry;

		public WeaponState weaponState;

		[Header("Sends")]
		[SignalDoc("Sent when equip succeeded")] public TriggerSend onSuccess;
		[SignalDoc("Sent when equip failed")] public TriggerSend onFail;
	}
}