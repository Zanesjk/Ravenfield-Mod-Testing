using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ravenfield.Data;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Actor/Trigger Spawn Imposter Actors")]
	[TriggerDoc("When Triggered, Spawns imposter actors that will follow the PathGroup. Up to maxNPaths imposters will be spawned. Imposters will play a death animation when reaching their path end unless path stayAtEnd is enabled. You can remove all spawned imposters by deactivating this component.")]
    public partial class TriggerSpawnImposterActors : TriggerReceiver
    {
        public ScriptedPathGroup pathGroup;

		public ActorImposterInfo[] imposterInfo = new ActorImposterInfo[1];

        public int maxNPaths = 100;

		[System.Serializable]
		public partial struct ActorImposterInfo
		{
			public Team team;
			public bool raycastToGround;

			public ActorSkinData overrideSkin;

			public TriggerEquipWeapon.WeaponType weaponType;
			[ConditionalField("weaponType", TriggerEquipWeapon.WeaponType.ByName)] public string weaponName;
			[ConditionalField("weaponType", TriggerEquipWeapon.WeaponType.FromWeaponEntry)] public WeaponManager.WeaponEntry weaponEntry;
		}
	}
}