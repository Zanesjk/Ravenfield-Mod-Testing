using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ravenfield.Trigger
{
	[AddComponentMenu("Trigger/Flow/Trigger For Each")]
	[TriggerDoc("When triggered, sends a signal for each matching subject. The sent signal context contains the matched subject.")]
	public partial class TriggerForEach : TriggerReceiver
	{
		public enum SubjectType
		{
			Actors,
			Squads,
			Vehicles,
		}

		public enum SortMode
		{
			Default,
			ByDistanceClosestFirst,
		}

		[Header("Query")]
		public SubjectType subjectType;

		[ConditionalField("subjectType", SubjectType.Actors)] public ActorGroup actorGroup;
		[ConditionalField("subjectType", SubjectType.Actors)] public ActorFilter actorFilter;

		[ConditionalField("subjectType", SubjectType.Squads)] public SquadGroup squadGroup;
		[ConditionalField("subjectType", SubjectType.Squads)] public SquadFilter squadFilter;

		[ConditionalField("subjectType", SubjectType.Vehicles)] public VehicleGroup vehicleGroup;
		[ConditionalField("subjectType", SubjectType.Vehicles)] public VehicleFilter vehicleFilter;

		[Header("Sorting")]
		public SortMode sorting = SortMode.Default;
		[ConditionalField("sorting", SortMode.ByDistanceClosestFirst)] public Transform closestToPoint;
		public bool reverseSort = false;

		[Header("Sends")]
		[SignalDoc("Sent once per matching actor/squad/vehicle", actor = "The matched actor", squad = "The matched squad", vehicle = "The matched vehicle")]
		public TriggerSend send;
	}
}