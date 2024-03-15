using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[TriggerDoc("When triggered, changes the fog density and color over time.")]
	[AddComponentMenu("Trigger/Level/Trigger Update Fog")]
    public partial class TriggerUpdateFog : TriggerReceiver
    {
        public float fogDensity = 0f;
        public float changeTime = 0f;
		public Color fogColor;

		float oldDensity;
		Color oldColor;

		bool isRunning = false;
		TimedAction updateAction = new TimedAction();
	}
}