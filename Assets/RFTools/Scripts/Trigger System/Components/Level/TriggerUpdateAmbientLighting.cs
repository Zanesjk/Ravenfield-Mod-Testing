using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [AddComponentMenu("Trigger/Level/Trigger Update Ambient Lighting")]
    [TriggerDoc("When triggered, updates the ambient lighting over time. the Scene environment lighting source must be set to gradient for this to take effect (this is found under the lighting tab).")]
    public partial class TriggerUpdateAmbientLighting : TriggerReceiver
    {
        public Color sky, equator, ground;
        public float changeTime;
	}
}