using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Level/Trigger Change Gravity")]
	[TriggerDoc("When Triggered, Changes the gravity of the level")]
	public partial class TriggerChangeGravity : TriggerReceiver
	{
		public Vector3 newGravity;
	}
}