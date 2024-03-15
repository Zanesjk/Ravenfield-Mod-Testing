using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Level/Trigger Play Dialog")]
	[TriggerDoc("When triggered, Plays a dialog using the ingame dialog system. Sends On Dialog End when dialog completes or when canceled by being deactivated or overriden by another dialog.")]
	public partial class TriggerDialog : TriggerReceiver, ICompoundTriggerSender
    {
		public static TriggerDialog currentPlayingDialog;

		public List<DialogEntry> lines = new List<DialogEntry>() { DialogEntry.DEFAULT };

		[SignalDoc("Sent when dialog completes or is canceled")]
		[InspectorName("On Dialog End")] public TriggerSend onDialogComplete;

		public bool stopActiveDialogOnComponentDisabled = true;

		public IEnumerable<TriggerSend> GetCompoundSends() {
			for (int i = 0; i < this.lines.Count; i++) {
				yield return this.lines[i].onSayLine;
			}
		}

		[System.Serializable]
		public struct DialogEntry
		{
			public enum BlipSoundPlayback
			{
				UseActorBlip,
				OverrideBlip,
				OverrideBlipCustomClip,
				OneShotCustomClip,
			}

			public static readonly DialogEntry DEFAULT = new DialogEntry() {
				actorPose = "p grunt",
				duration = 5f,
			};

			public string actorPose;
			public string overrideName;
			[Multiline] public string text;
			public float portraitGrain;

			public TriggerSend onSayLine;

			public BlipSoundPlayback blipSoundPlayback;
			[ConditionalField("blipSoundPlayback", BlipSoundPlayback.OverrideBlip)] public SpriteActorDatabase.BlipSound overrideBlipSound;
			[ConditionalField("blipSoundPlayback", BlipSoundPlayback.OneShotCustomClip, BlipSoundPlayback.OverrideBlipCustomClip)] public AudioClip overrideBlipCustomClip;

			public float duration;
		}
	}
}