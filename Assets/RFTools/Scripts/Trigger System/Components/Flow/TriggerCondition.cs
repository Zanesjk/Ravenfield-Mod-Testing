using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [AddComponentMenu("Trigger/Flow/Trigger Condition")]
    [TriggerDoc("When triggered, sends OnTrue if the condition is true, otherwise sends OnFalse")]
    public partial class TriggerCondition : TriggerReceiver
    {
        public enum Condition
		{
            ActorIsInAnyVehicle = 0,
            ActorIsInVehicle = 1,
            ActorIsInsideVolume = 2,
            ActorIsVehiclePilot = 3,

            SquadMemberCountGreaterOrEqual = 100,
            SquadIsOnTeam = 101,

            TeamHasActorsInsideVolume = 200,
            TeamAllActorsDead = 201,
            TeamActorsAliveGreaterOrEqual = 202,
            TeamActorCountGreaterOrEqual = 203,

            GameObjectIsActive = 300,

            MatchIsOver = 400,
		}

        [Header("Parameters")]
        public Condition condition;

        [ConditionalField("condition", Condition.ActorIsInAnyVehicle, Condition.ActorIsInVehicle, Condition.ActorIsInsideVolume, Condition.ActorIsVehiclePilot)]
        public ActorReference actor;

        [ConditionalField("condition", Condition.SquadMemberCountGreaterOrEqual, Condition.SquadIsOnTeam)]
        public SquadReference squad;

        [ConditionalField("condition", Condition.ActorIsInVehicle)]
        public VehicleReference vehicle;

        [ConditionalField("condition", Condition.GameObjectIsActive)]
        public GameObject targetGameObject;

        [ConditionalField("condition", Condition.TeamHasActorsInsideVolume, Condition.TeamAllActorsDead, Condition.TeamActorsAliveGreaterOrEqual, Condition.TeamActorCountGreaterOrEqual, Condition.SquadIsOnTeam)]
        public Team team;

        [ConditionalField("condition", Condition.ActorIsInsideVolume, Condition.TeamHasActorsInsideVolume)]
        public TriggerVolume volume;

        [ConditionalField("condition", Condition.SquadMemberCountGreaterOrEqual, Condition.TeamActorsAliveGreaterOrEqual, Condition.TeamActorCountGreaterOrEqual)]
        public int count;

        [Header("Sends")]

        [SignalDoc("Sent if condition is true")]
        public TriggerSend onTrue;

        [SignalDoc("Sent if condition is false")]
        public TriggerSend onFalse;
	}
}