using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ScoreboardUIType { Scoreboard, Objectives, None }

public abstract class GameModeBase : MonoBehaviour {

	public enum SpawnLayout
	{
		Default,
		UserDefined,
		GameModeOverride,
	}

	public enum LoadoutUIType
	{
		Default,
		None,
	}

	public string gameModeName = "NEW GAME MODE";
	public LoadoutUIType loadoutType = LoadoutUIType.Default;
	public ScoreboardUIType scoreboardType = ScoreboardUIType.Scoreboard;

	public bool canConfigureFlags = true;
	public bool canContinuePlayingAfterGameEnd = true;
	public bool autoOpenLoadoutOnStart = true;
}
