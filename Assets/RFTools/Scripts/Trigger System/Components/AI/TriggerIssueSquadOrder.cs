using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/AI/Trigger Issue Squad Order")]
	[TriggerDoc("When triggered, issues a order to the specified squad.")]
	public partial class TriggerIssueSquadOrder : TriggerReceiver
	{
		public enum SquadTarget
		{
			SingleSquad,
			AllSquadsOnTeam,
			AllSquadsOnTeamInsideVolume,
		}

		public enum OrderType
		{
			CreateNew,
			GenerateAICommanderOrder,
		}

		[Header("Parameters")]
		public SquadTarget squadTarget;

		[ConditionalField("squadTarget", SquadTarget.SingleSquad)] public SquadReference squad;
		[ConditionalField("squadTarget", SquadTarget.AllSquadsOnTeam, SquadTarget.AllSquadsOnTeamInsideVolume)] public Team team;
		[ConditionalField("squadTarget", SquadTarget.AllSquadsOnTeamInsideVolume)] public TriggerVolume volume;

		public OrderType orderType = OrderType.CreateNew;

		[ConditionalField("orderType", OrderType.CreateNew)] public OrderDefinition order;
	}
}