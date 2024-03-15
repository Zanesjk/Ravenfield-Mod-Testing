using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Level/Trigger Activation")]
	[TriggerDoc("When triggered, Activates ObjectsToActivate and Deactivates ObjectsToDeactivate. Note that deactivated trigger receivers cannot be triggered, so this component can be used to create conditional logic.")]
	public partial class TriggerActivation : TriggerReceiver
	{
		[Header("Parameters")]
		public GameObject[] objectsToActivate;
		public GameObject[] objectsToDeactivate;
	}
}