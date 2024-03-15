using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapMetaData
{
	public enum MapType
	{
		Unknown,
		Plains,
		Hills,
		Forest,
		Mountain,
		Village,
		City,
		Coastline,
		Base,
	}

	public enum MapTheme
	{
		None,
		Temperate,
		Desert,
		Jungle,
		Swamp,
		Snow,
		Barren,
		Island,
		Space,
	}

	public static MapMetaData Default {
		get {
			return new MapMetaData() {
				hasBuiltInGameMode = false,
				visibleInInstantAction = true,
				suggestedBots = 60,
				loadingScreenFOV = 20f,
			};
		}
	}

	public void WriteToFile(string path) {

	}

	public bool hasBuiltInGameMode;
	public bool usesTriggerSystem;
	public bool visibleInInstantAction;
	public int suggestedBots;
	public string displayName;

	public MapType typePrimary;
	public MapType typeSecondary;
	public MapTheme theme;

	public string loadingScreenBackgroundImage;
	public string loadingScreenModel;
	public string loadingScreenTexture;
	public bool loadingScreenUseArtworkShader;
	public float loadingScreenFOV;
}
