using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Actor/Trigger Fire Weapon")]
	[TriggerDoc("When Triggered, Fires a single shot on a weapon.")]
    public partial class TriggerFireWeapon : TriggerReceiver
    {
        public WeaponReference weapon;
		public bool ignoreCanFireCheck;

		[SignalDoc("Sent when fire succeeded")] public TriggerSend onFired;
		[SignalDoc("Sent when fire failed")] public TriggerSend onCouldntFire;
	}
}