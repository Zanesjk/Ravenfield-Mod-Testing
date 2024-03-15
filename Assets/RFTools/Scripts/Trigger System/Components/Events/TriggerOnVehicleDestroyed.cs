using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [AddComponentMenu("Trigger/Events/Trigger On Vehicle Destroyed")]
    [TriggerDoc("Sends signals when a vehicle is disabled/destroyed.")]
    public partial class TriggerOnVehicleDestroyed : TriggerBaseComponent
    {
        public VehicleFilter filter;

        [Header("Sends")]
        [SignalDoc("Sent when a vehicle is disabled (starts burning)", vehicle = "The vehicle", actor = "The actor credited for disabling the vehicle")]
        public TriggerSend onDisabled;

        [SignalDoc("Sent when a vehicle is destroyed", vehicle = "The vehicle", actor = "The actor credited for destroying the vehicle")]
        public TriggerSend onDestroyed;
    }
}