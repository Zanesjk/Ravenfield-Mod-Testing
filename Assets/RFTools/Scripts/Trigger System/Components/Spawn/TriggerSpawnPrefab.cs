using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Spawn/Trigger Spawn Prefab")]
	[TriggerDoc("When triggered, Instantiates a prefab at the specified SpawnPoint. If PropagateSignalToPrefab is true, propagates the signal to a trigger receiver on the prefab root GameObject.")]
	public partial class TriggerSpawnPrefab : TriggerReceiver
	{
		public GameObject prefab;
		public Transform spawnPoint;
		public bool attachToSpawnPointParentTransform = false;

		public bool propagateSignalToPrefab = false;

		[SignalDoc("Sent when the prefab is spawned")]
		public TriggerSend onSpawnCompleteTrigger;
	}
}