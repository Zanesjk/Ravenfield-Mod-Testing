using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ravenfield.Trigger;

public class AnimationEventReceiver : TriggerBaseComponent, ICompoundTriggerSender
{
	public bool isSkippableCinematic = false;
	public bool freezeGameplay = false;
	public bool usePassepartout = false;
	public float passepartoutAspect = 2.2f;

	public EventGroup awakeEvent;
	public EventGroup skipEvent;

	public bool skipEventEndsCutscene;

	public EventGroup[] events;

	public IEnumerable<TriggerSend> GetCompoundSends() {

		yield return this.awakeEvent.trigger;
		yield return this.skipEvent.trigger;

		for (int i = 0; i < this.events?.Length; i++) {
			yield return this.events[i].trigger;
		}
	}

	[System.Serializable]
	public struct EventGroup
	{
		public string name;

		public GameObject[] activate;
		public GameObject[] deactivate;
		public CameraEvent camera;
		public FadeEvent fade;
		public ScreenShakeEvent shake;
		public AudioEvent audio;
		public TriggerSend trigger;

		[System.Serializable]
		public struct CameraEvent
		{
			public enum Type
			{
				None,
				OverrideCamera,
				CancelOverrideCamera,
				CancelOverrideCameraIfSetTo,
			}

			public Type eventType;
			[ConditionalField("eventType", Type.OverrideCamera, Type.CancelOverrideCameraIfSetTo)] public Camera overrideCamera;
		}

		[System.Serializable]
		public struct FadeEvent
		{
			public enum Type
			{
				None,
				FadeIn,
				FadeOut,
			}

			public Type eventType;
			public EffectUi.FadeType fadeType;
			public float duration;
			public Color color;
		}

		[System.Serializable]
		public struct ScreenShakeEvent
		{
			public enum Type
			{
				None,
				Shake,
			}

			public Type eventType;
			public float shakeAmount;
			public int shakeCount;
		}

		[System.Serializable]
		public struct AudioEvent
		{
			public enum Type {
				None,
				TransitionToSnapshot,
			}

			public Type eventType;
			public AudioManager.AudioSnapshot snapshot;
			public float transitionTime;
		}
	}
}
