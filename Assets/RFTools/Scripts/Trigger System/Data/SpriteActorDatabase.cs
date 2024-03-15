using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "SpriteActorDatabase", menuName = "Dialog/Sprite Actor Database")]
public class SpriteActorDatabase : ScriptableObject {

	public enum BlipSound
	{
		Default = 0,
		Bright = 1,
		Radio = 2,
	}

	public Pose[] poses;
	
	[System.Serializable]
	public class Pose
	{

		public string name;
		public string defaultDisplayName = "???";
		public bool firstTalkFrameIsIdle = false;
		public Sprite baseSprite;
		public Sprite[] talkOverlaySprites;

		public BlipSound blipSound;
		public AudioClip overrideBlipSound;

		[System.NonSerialized] int lastTalkFrameIndex = -1;
		[System.NonSerialized] public SpriteActorDatabase sourceDatabase;
		[System.NonSerialized] public int lowercaseNameHash;

		public bool HasTalkFrames() {
			return this.talkOverlaySprites.Length > 0;
		}

		public Sprite GetRandomTalkFrame() {
			if(this.talkOverlaySprites.Length == 0) {
				return null;
			}
			if(this.talkOverlaySprites.Length == 1) {
				return this.talkOverlaySprites[0];
			}
			try {
				int min = this.firstTalkFrameIsIdle ? 1 : 0;

				int frameIndex = Random.Range(min, this.talkOverlaySprites.Length);
				if(frameIndex == this.lastTalkFrameIndex) {
					frameIndex = (frameIndex + 1) % this.talkOverlaySprites.Length;
					
					if(this.firstTalkFrameIsIdle && frameIndex == 0) {
						frameIndex = 1;
					}
				}

				this.lastTalkFrameIndex = frameIndex;

				return talkOverlaySprites[frameIndex];
			}
			catch(System.Exception e) {
				Debug.LogException(e);
				return null;
			}
		}
	}
}
