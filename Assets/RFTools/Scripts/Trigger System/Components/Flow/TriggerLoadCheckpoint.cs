using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Flow/Trigger Load Checkpoint")]
	[TriggerDoc("Use this component to trigger other components when the level starts.")]
	public partial class TriggerLoadCheckpoint : TriggerBaseComponent
	{
		public ScriptedGameMode.Checkpoint checkpoint;
		[SignalDoc("Sent when the level starts with the specified checkpoint value", overrideName = "On Checkpoint Loaded")]
		[InspectorName("On Checkpoint Loaded"), AutoPopulateChildReceivers] public TriggerSend onCheckpointLoadedTrigger;
	}
}