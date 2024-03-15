using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/AI/Trigger Look At")]
	[TriggerDoc("When triggered, makes the AI look at a transform.")]
	public partial class TriggerLookAt : TriggerReceiver
    {
		public enum Type
		{
			LookAt,
			CancelLook,
		}

		public Type type;
		[ConditionalField("type", Type.LookAt)] public Transform lookTarget;
		public ActorReference actor;
	}
}