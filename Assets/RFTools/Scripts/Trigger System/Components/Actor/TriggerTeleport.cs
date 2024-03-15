using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Actor/Trigger Teleport")]
	[TriggerDoc("When Triggered, Teleports the target actor to a destination.")]
	public partial class TriggerTeleport : TriggerReceiver
	{
		public enum Type
		{
			Actor,
		}

		public Type targetType;
		[ConditionalField("targetType", Type.Actor)] public ActorReference actor;
		public Transform destination;
	}
}