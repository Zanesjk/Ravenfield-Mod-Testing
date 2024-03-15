using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ravenfield.Trigger;

namespace Ravenfield.Trigger {
	[AddComponentMenu("Trigger/AI/Trigger Follow Path Group")]
	[TriggerDoc("When triggered, makes the specified squad follow the scripted path group.")]
	public partial class TriggerFollowPathGroup : TriggerReceiver
	{
		public SquadReference squad;
		public ScriptedPathGroup pathGroup;
		public bool teleportToStart = false;
	}
}