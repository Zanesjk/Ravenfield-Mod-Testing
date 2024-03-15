using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [System.Serializable]
    public struct OrderDefinition
    {
        public Order.OrderType type;
        public SpawnPoint target;
        public SpawnPoint source;
        public Transform targetPoint;
    }
}