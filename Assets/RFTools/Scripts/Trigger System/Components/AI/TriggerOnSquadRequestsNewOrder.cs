using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [AddComponentMenu("Trigger/AI/Trigger On Squad Requests New Order")]
	[TriggerDoc("Sends OnRequest every time a squad requests a new order. Having this component active in your map prevents the AICommander from automatically issuing orders to squads. Only one of these components may be active at one time.")]
    public partial class TriggerOnSquadRequestsNewOrder : TriggerBaseComponent
    {
        public static TriggerOnSquadRequestsNewOrder activeConsumer;

        [SignalDoc("Sent when squad requests a new order.", squad = "The requesting squad")]
        public TriggerSend onRequest;
	}
}