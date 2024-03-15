using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Level/TriggerPlayerOrderPoint")]
	[TriggerDoc("When activated, Creates a Player Order Point at this position. Sends OnIssued when the player issues the order by holding and releasing the squad command key.")]
	public partial class TriggerPlayerOrderPoint : TriggerBaseComponent
    {
		public string label;
		public Texture2D texture;

		[SignalDoc("Sent when order is issued")]
		public TriggerSend onIssued;
	}
}