using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Level/Trigger Audio Snapshot")]
	[TriggerDoc("When Triggered, Transitions to another audio snapshot.")]
    public partial class TriggerAudioSnapshot : TriggerReceiver
    {
		public enum Type
		{
			TransitionToSnapshot,
			ResetPlayerSnapshot,
		}

		public Type type;

		[ConditionalField("type", Type.TransitionToSnapshot)] public AudioManager.AudioSnapshot snapshot;
		public float transitionTime = 1f;
	}
}