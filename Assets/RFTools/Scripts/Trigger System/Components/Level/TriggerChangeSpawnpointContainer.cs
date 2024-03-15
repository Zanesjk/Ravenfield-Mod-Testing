using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Level/Trigger Change Spawnpoint Container")]
	[TriggerDoc("When Triggered, Changes the capture point's spawnpoint container or contested spawnpoint container.")]
	public partial class TriggerChangeSpawnpointContainer : TriggerReceiver
	{
		public enum Type
		{
			DefaultContainer,
			ContestedContainer,
		}

		public SpawnPoint spawnPoint;
		public Type type;
		public Transform newTarget;
	}
}