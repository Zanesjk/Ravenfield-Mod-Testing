using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ravenfield.Trigger;

namespace Ravenfield.Trigger
{
	[System.Serializable]
	public struct ScriptedPathNode
	{
		public enum Stance
		{
			Standing,
			Crouched,
			Prone,
		}

		public const float DEFAULT_SPEED = 3.2f;

		public Vector3 localPosition;
		public float speed;
		public bool slowDownForNextTurn;
		public Stance stance;
		public bool synchronize;
		public byte syncNumber;
		public float waitTime;

		public TriggerSend onReached;

		public ScriptedPathNode(Vector3 position) {
			this.localPosition = position;
			this.speed = DEFAULT_SPEED;
			this.slowDownForNextTurn = false;
			this.stance = Stance.Standing;

			this.synchronize = false;
			this.syncNumber = 0;
			this.waitTime = 0f;

			this.onReached = default;
		}

		public ScriptedPathNode(Vector3 position, ScriptedPathNode basedOn) {
			this.localPosition = position;
			this.speed = basedOn.speed;
			this.slowDownForNextTurn = basedOn.slowDownForNextTurn;
			this.stance = basedOn.stance;

			this.synchronize = false;
			this.syncNumber = 0;
			this.waitTime = 0f;

			this.onReached = default;
		}
	}
}