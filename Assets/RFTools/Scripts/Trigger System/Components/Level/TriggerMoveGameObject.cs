using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Level/Trigger Move GameObject")]
	[TriggerDoc("When triggered, Moves the ObjectToMove to the target point or object.")]
	public partial class TriggerMoveGameObject : TriggerReceiver
    {
		public enum TargetType
		{
			Transform,
			Actor,
			Vehicle,
			Imposter,
		}

		[Header("Parameters")]
		public Transform objectToMove;

		[Header("Move To")]
		public TargetType target;

		[ConditionalField("target", TargetType.Transform)] public Transform destination;
		[ConditionalField("target", TargetType.Actor)] public ActorReference actor;
		[ConditionalField("target", TargetType.Vehicle)] public VehicleReference vehicle;
		[ConditionalField("target", TargetType.Imposter)] public TriggerSpawnImposterActors imposterSpawner;
		[ConditionalField("target", TargetType.Imposter)] public int imposterIndex;

		public bool makeNewParent = false;

		[Header("Sends")]
		public TriggerSend onSuccess;
		public TriggerSend onFail;
	}
}