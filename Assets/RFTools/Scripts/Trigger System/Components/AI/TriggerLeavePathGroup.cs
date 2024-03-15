using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[TriggerDoc("On Triggered, Makes the target squad leave their scripted path group.")]
	[AddComponentMenu("Trigger/AI/Trigger Leave Path Group")]
	public partial class TriggerLeavePathGroup : TriggerReceiver
	{
		public SquadReference squad;
	}
}