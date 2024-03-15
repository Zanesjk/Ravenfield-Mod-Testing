using System.Collections.Generic;

namespace Ravenfield.Trigger
{
	[System.Serializable]
	public struct ActorGroup
	{
		public enum Group
		{
			Everyone,
			EveryoneIncludingDead,
			Player,
			SquadMembers,
			TeamBlue,
			TeamRed,
			DetectionGroup,
		}

		public Group group;
		[ConditionalField("group", Group.SquadMembers)] public SquadReference squad;
		[ConditionalField("group", Group.DetectionGroup)] public TriggerDetectionGroup detectionGroup;
	}
}