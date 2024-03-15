using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/AI/Trigger Squad Recalculate Paths")]
	[TriggerDoc("When Triggered, Recalculates the path to the current order destination for the squad. This is typically done after modifying the navmesh walkability with the DynamicBlockWalkableBox component.")]
	public partial class TriggerSquadRecalculatePaths : TriggerReceiver
	{
		public SquadReference squad;
	}
}