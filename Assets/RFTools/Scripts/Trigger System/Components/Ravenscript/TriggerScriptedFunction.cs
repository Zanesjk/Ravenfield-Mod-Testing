using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lua;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Ravenscript/Trigger Scripted Function")]
	[TriggerDoc("When Triggered, Calls a function on the specified scripted behaviour.")]
    public partial class TriggerScriptedFunction : TriggerReceiver
    {
        public ScriptedBehaviour scriptedBehaviour;
        public string functionName;
	}
}