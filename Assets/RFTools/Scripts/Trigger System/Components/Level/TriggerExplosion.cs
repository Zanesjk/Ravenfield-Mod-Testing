using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [AddComponentMenu("Trigger/Level/Trigger Explosion")]
    [TriggerDoc("When Triggered, Creates and explosion credited to the CreditedActor. Sends OnHitAnything if anything was hit (IE would produce a hitmarker if player is the credited actor), otherwise sends OnHitNothing.")]
    public partial class TriggerExplosion : TriggerReceiver
    {
        public Transform center;
        public ExplodingProjectile.ExplosionConfiguration explosion;
        public Vehicle.ArmorRating damageRating;
        public GameObject objectToActivate;

        public ActorReference creditedActor;
        public bool reduceFriendlyFireDamage = false;

        [Header("Sends")]

        [SignalDoc("Sent if explosion hit anything")]
        public TriggerSend onHitAnything;

        [SignalDoc("Sent if explosion didn't hit anything")]
        public TriggerSend onHitNothing;
	}
}