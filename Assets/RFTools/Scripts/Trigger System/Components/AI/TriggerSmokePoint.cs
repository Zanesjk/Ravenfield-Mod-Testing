using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/AI/Trigger Smoke Point")]
	[TriggerDoc("When triggered, places an AI Smoke Point at this position, which makes the AI throw a smoke grenade.")]
	public partial class TriggerSmokePoint : TriggerReceiver
    {
		public Team team;
		public float lifetime = 30f;
	}
}