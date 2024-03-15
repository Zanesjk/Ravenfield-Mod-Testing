using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Objective/Trigger Create Objective")]
	[TriggerDoc("When triggered, Creates an objective on the objective screen")]
	public partial class TriggerCreateObjective : TriggerReceiver
    {
		public string label;
		public Target target;

		[System.Serializable]
		public partial struct Target
		{
			public enum Type
			{
				None,
				Transform,
				Actor,
				Vehicle,
			}

			public Type type;

			[ConditionalField("target", Type.Transform)] public Transform targetTransform;
			[ConditionalField("target", Type.Actor)] public ActorReference targetActor;
			[ConditionalField("target", Type.Vehicle)] public VehicleReference targetVehicle;
		}
	}
}