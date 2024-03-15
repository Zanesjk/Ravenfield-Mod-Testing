using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [AddComponentMenu("Trigger/AI/Trigger Exit Vehicle")]
	[TriggerDoc(
		"When Triggered, Makes actors leave a vehicle.\n" +
		"EveryoneLeaveVehicle: Forces all actors in the vehicle to leave and drops any squad claim.\n"
	)]
	public partial class TriggerExitVehicle : TriggerReceiver
    {
		public enum Type
		{
			EveryoneLeaveVehicle,
		}

		public Type type;
		public VehicleReference vehicle;
	}
}