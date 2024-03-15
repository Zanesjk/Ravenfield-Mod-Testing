using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Level/Trigger Crossfade Audio")]
	[TriggerDoc("When triggered, Crossfades the audio sources, fading out From and fading in To")]
	public partial class TriggerCrossfadeAudio : TriggerReceiver
    {
		public AudioSource from, to;
		public float crossfadeTime = 1f;
		public float toVolume = 1f;
	}
}