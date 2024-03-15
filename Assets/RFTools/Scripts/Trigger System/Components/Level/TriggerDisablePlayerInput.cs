using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Level/Trigger Enable\\Disable Player Input")]
	[TriggerDoc("When triggered, Disables or Enables the player input")]
	public partial class TriggerDisablePlayerInput : TriggerReceiver
	{
		public enum Action
		{
			Disable,
			Enable
		};

		public enum Type
		{
			Input,
			Movement,
		}

		[Header("Parameters")]
		public Action action;
		public Type type;
	}
}