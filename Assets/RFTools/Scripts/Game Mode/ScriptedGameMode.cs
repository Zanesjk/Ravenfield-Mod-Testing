using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ravenfield.Trigger;

public class ScriptedGameMode : GameModeBase {

	// If more checkpoints are required, they can be manually added to this list.
	public enum Checkpoint
	{
		Start,
		CP1,
		CP2,
		CP3,
		CP4,
		CP5,
		CP6,
		CP7,
		CP8,
		CP9,
	}

	public enum InstantActionGameConfig
	{
		UserDefined,
		Official,
	}

	public enum InstantActionMutatorConfig
	{
		UserDefined,
		None,
	}

	public enum TeamConfig
	{
		User,
		Blue,
		Red,
	}

	public TestConfiguration testConfiguration;

	public InstantActionGameConfig instantActionGameConfig;
	public InstantActionMutatorConfig instantActionMutatorConfig;
	public TeamConfig playerTeam;

	public static Checkpoint currentCheckpoint = Checkpoint.Start;
	public bool canCaptureFlagsByDefault = true;

	public bool spawnVehicles = true;
	public bool spawnTurrets = true;

	public bool generateAISquadOrders = true;
	public TriggerDebug.DebugLevel triggerDebugLevel = TriggerDebug.DebugLevel.Everything;

	[System.Serializable]
    public struct TestConfiguration {
        public Checkpoint checkpoint;
    }
}
