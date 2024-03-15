using System.Collections.Generic;

namespace Ravenfield.Trigger
{
	[System.Serializable]
	public struct SquadGroup
	{
		public enum Group
		{
			All,
			Player,
			TeamBlue,
			TeamRed,
			VehicleOwner,
			DetectionGroup,
		}

		public Group group;
		[ConditionalField("group", Group.VehicleOwner)] public VehicleReference vehicle;
		[ConditionalField("group", Group.DetectionGroup)] public TriggerDetectionGroup detectionGroup;
	}
}