using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [AddComponentMenu("Trigger/Spawn/Trigger Spawn Blood Particles")]
    [TriggerDoc("When Triggered, Spawns a number of blood particles. If OverrideActorTeam is assigned, will spawn blood particles of that actor's team.")]
    public partial class TriggerSpawnBloodParticles : TriggerReceiver
    {
        public Team team;
        public ActorReference overrideActorTeam;

        public Transform spawnFrom;
        public Vector3 velocity = new Vector3(0f, 0f, 5f);
        public Vector3 randomVelocity = new Vector3(1f, 1f, 1f);
	}
}