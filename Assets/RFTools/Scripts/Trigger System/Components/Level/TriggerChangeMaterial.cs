using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[TriggerDoc("When triggered, changes a material. Set target type to skybox to change the current skybox.")]
	[AddComponentMenu("Trigger/Level/Trigger Change Material")]
	public partial class TriggerChangeMaterial : TriggerReceiver
	{
		public enum TargetType
		{
			Renderer,
			Skybox,
		}

		public TargetType targetType;

		[ConditionalField("targetType", TargetType.Renderer)] public MaterialTarget target;
		public Material material;
	}
}