using UnityEngine;
using System.Collections;

public struct TimedAction
{

	float lifetime, end;

	bool unscaledTime;

	// Lie once so that stuff that uses ratio receives a 1.0f before done is called.
	bool lied;

	public TimedAction(float lifetime, bool unscaledTime = false) {
		this.lifetime = lifetime;
		this.end = 0f;
		this.lied = true;
		this.unscaledTime = unscaledTime;
	}

	public void Start() {
		this.end = GetTime() + this.lifetime;
		this.lied = false;
	}

	public void StartLifetime(float lifetime) {
		this.lifetime = lifetime;
		Start();
	}

	public void Stop() {
		this.end = 0f;
		this.lied = true;
	}

	public float Remaining() {
		return this.end - GetTime();
	}

	public float Elapsed() {
		return this.lifetime - Remaining();
	}

	public float Ratio() {
		return Mathf.Clamp01(1f - (this.end - GetTime()) / this.lifetime);
	}

	public bool TrueDone() {
		return this.end <= GetTime();
	}

	float GetTime() {
#if UNITY_EDITOR
		if (Application.isPlaying) {
			return this.unscaledTime ? Time.unscaledTime : Time.time;
		}
		else {
			// Use system time as that allows time measurements when running anything in-editor.
			return this.unscaledTime ? Time.realtimeSinceStartup : Time.time;
		}
#else
		return this.unscaledTime ? Time.unscaledTime : Time.time;
#endif
	}

	public bool Done() {
		if (TrueDone()) {
			if (!this.lied) {
				this.lied = true;
				return false;
			}
			return true;
		}
		return false;
	}
}
