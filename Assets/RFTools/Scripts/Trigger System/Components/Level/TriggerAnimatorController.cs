using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Level/Trigger Animator Controller")]
	[TriggerDoc("When triggered, Sets the trigger TriggerName on the specified Animator controller.")]
	public partial class TriggerAnimatorController : TriggerReceiver
	{
		public Animator animator;
		public string triggerName;
	}
}