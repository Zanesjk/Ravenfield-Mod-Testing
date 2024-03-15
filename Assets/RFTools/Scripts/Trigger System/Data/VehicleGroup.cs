using System.Collections.Generic;

namespace Ravenfield.Trigger
{
	[System.Serializable]
	public struct VehicleGroup
	{
		public enum Group
		{
			AnyType,
			Vehicles,
			Turrets,
		}

		public Group group;
	}
}