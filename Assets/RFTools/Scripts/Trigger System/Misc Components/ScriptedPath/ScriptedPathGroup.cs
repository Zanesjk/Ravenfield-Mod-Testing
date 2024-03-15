using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ravenfield.Trigger;

namespace Ravenfield.Trigger
{
	public class ScriptedPathGroup : TriggerBaseComponent, ICompoundTriggerSender
	{
		HashSet<byte> activeSyncs = new HashSet<byte>();
		HashSet<byte> notReadySyncs = new HashSet<byte>();
		bool isPlaying = false;

		ScriptedPathSeeker[] activeSeekers;
		[System.NonSerialized] public List<ScriptedPath> paths;

		public TriggerVolume[] playerSyncVolumes;
		public TriggerSend[] syncSends;

		HashSet<TriggerSyncPathGroup> triggerSyncs = new HashSet<TriggerSyncPathGroup>();

		public void RegisterTriggerSync(TriggerSyncPathGroup trigger) {
			this.triggerSyncs.Add(trigger);
		}

		public void FindPaths() {
			this.paths = new List<ScriptedPath>(GetComponentsInChildren<ScriptedPath>());
		}

		public void Play(ScriptedPathSeeker[] seekers) {
			this.isPlaying = true;
			this.activeSeekers = seekers;
		}

		public void Stop() {
			this.isPlaying = false;
			this.activeSeekers = null;
		}

		public void Update() {
			if (!this.isPlaying) return;

			try {
				this.activeSyncs.Clear();
				this.notReadySyncs.Clear();

				bool anySeekerIsTraversingPath = false;

				for (int i = 0; i < this.activeSeekers.Length; i++) {

					if (!this.activeSeekers[i].isTraversingPath) {
						continue;
					}

					anySeekerIsTraversingPath = true;

					byte syncNumber = this.activeSeekers[i].nextSyncNumber;
					if (syncNumber != ScriptedPathSeeker.SYNC_NUMBER_NONE) {

						if (this.activeSeekers[i].awaitingSync) {
							this.activeSyncs.Add(syncNumber);
						}
						else {
							this.notReadySyncs.Add(syncNumber);
						}
					}
				}

				foreach (var syncNumber in this.activeSyncs) {
					if (!this.notReadySyncs.Contains(syncNumber)) {
						Synchronize(syncNumber);
					}
				}

				if (!anySeekerIsTraversingPath) {
					Stop();
				}
			}
			catch(System.Exception e) {
				Debug.LogException(e);
				Stop();
			}
		}

		public void Synchronize(byte syncNumber) {
			//Debug.Log("Sync: " + syncNumber);
			for (int i = 0; i < this.activeSeekers.Length; i++) {
				this.activeSeekers[i].Synchronize(syncNumber);
			}
		}

		public IEnumerable<TriggerSend> GetCompoundSends() {
			for (int i = 0; i < this.syncSends.Length; i++) {
				yield return this.syncSends[i];
			}
		}
	}
}
