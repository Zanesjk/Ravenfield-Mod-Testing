using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Ravenfield.Trigger
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TriggerDocAttribute : PropertyAttribute
	{
		public string message;

		public TriggerDocAttribute(string message) {
			this.message = message;
		}
	}
}