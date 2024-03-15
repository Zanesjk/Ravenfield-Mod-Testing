using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Events/Trigger On Actor Enter Vehicle")]
	[TriggerDoc("Sends a signal every time an actor enters or leaves the target vehicle")]
    public partial class TriggerOnActorEnterVehicle : TriggerBaseComponent
    {
		public VehicleReference vehicle;

		[Header("Sends")]

		[SignalDoc("Sent when an actor entered the vehicle", actor ="The entering actor", squad = "The squad the actor belongs to")]
		public TriggerSend onActorEntered;

		[SignalDoc("Sent when an actor exits the vehicle", actor = "The exiting actor", squad = "The squad the actor belongs to")]
		public TriggerSend onActorExited;

	}
}