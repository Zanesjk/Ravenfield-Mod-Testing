using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Level/Trigger Slowmotion")]
	[TriggerDoc("On triggered, starts slow motion. Time Scale values should be between 0 and 1.")]
	public partial class TriggerSlowmotion : TriggerReceiver
	{
		public bool reset = false;
		public float changeTime = 1f;
		[ConditionalField("reset", false)] public float timeScale = 0.2f;
	}
}