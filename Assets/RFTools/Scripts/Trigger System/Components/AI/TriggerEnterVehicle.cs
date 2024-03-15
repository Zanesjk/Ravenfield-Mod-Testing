using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/AI/Trigger Enter Vehicle")]
	[TriggerDoc("When triggered, makes the specified squad enter the specified vehicle. Only affects player squad if EnterInstantly is true.")]
	public partial class TriggerEnterVehicle : TriggerReceiver
	{
		public SquadReference squad;
		public VehicleReference vehicle;
		public bool enterInstantly = false;
	}
}