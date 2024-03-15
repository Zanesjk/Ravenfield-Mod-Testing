using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace Ravenfield.Trigger
{
    public abstract class TriggerReceiver : TriggerBaseComponent
    {
		public enum RepeatType
		{
			Once,
			Repeat,
		}

		public RepeatType repeatType = RepeatType.Repeat;
	}
}