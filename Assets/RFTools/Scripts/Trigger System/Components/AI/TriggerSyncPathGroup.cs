using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/AI/Trigger Sync Path Group")]
	[TriggerDoc("When Triggered, synchronizes a path group.")]
    public partial class TriggerSyncPathGroup : TriggerReceiver
    {
        public ScriptedPathGroup pathGroup;
        public byte sync;
	}
}