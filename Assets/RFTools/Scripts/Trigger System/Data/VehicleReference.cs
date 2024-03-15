using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [System.Serializable]
    public struct VehicleReference
    {
        public enum Type
		{
            SpawnedVehicle,
            FromSignal,
            VehicleInstance,
        }

        public Type type;

        [ConditionalField("type", Type.SpawnedVehicle)] public VehicleSpawner vehicleSpawner;
        [ConditionalField("type", Type.SpawnedVehicle)] public TurretSpawner turretSpawner;
        [ConditionalField("type", Type.VehicleInstance)] public Vehicle vehicleInstance;
    }
}