using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

public class MetaDataWindow : EditorWindow
{
	[MenuItem("Ravenfield Tools/Map/Metadata Editor")]
	public static void PublishToWorkshop() {
		MetaDataWindow window = EditorWindow.GetWindow<MetaDataWindow>();
		window.titleContent.text = "Scene Metadata";
		window.Show();
	}

	private void OnEnable() {
		LoadMetadata();
		EditorSceneManager.activeSceneChangedInEditMode += OnActiveSceneChange;
	}

	private void OnDisable() {
		EditorSceneManager.activeSceneChangedInEditMode -= OnActiveSceneChange;
	}

	void OnActiveSceneChange(Scene a, Scene b) {
		LoadMetadata();
	}

	bool hasLoadedMetaData;
	MapMetaData metadata;
	bool showMapInfo = true;
	bool showLoadingScreenInfo = true;

	string[] mapTypeOptions;

	void LoadOrCreateMetadata() {
		var currentScene = EditorSceneManager.GetSceneAt(0);

		if (!MetaDataUtils.MetaDataExistsFor(currentScene.path)) {
			MetaDataUtils.WriteMetaData(currentScene.path, MapMetaData.Default);
		}

		LoadMetadata();
	}

	void LoadMetadata() {
		var currentScene = EditorSceneManager.GetSceneAt(0);
		this.hasLoadedMetaData = MetaDataUtils.ReadMetaData(currentScene.path, out this.metadata, MapMetaData.Default);
	}

	void SaveMetadata() {
		var currentScene = EditorSceneManager.GetSceneAt(0);
		MetaDataUtils.WriteMetaData(currentScene.path, this.metadata);
		Debug.Log($"Saved {currentScene.name} metadata!");

		MetaDataUtils.CopyMetaData(currentScene.path, MapExport.GetExportFilePath(currentScene));
	}

	void OnGUI() {
		if(GUILayout.Button("Create/Load Metadata")) {
			LoadOrCreateMetadata();
			GUI.FocusControl(null);
			Repaint();
		}

		if (!this.hasLoadedMetaData) return;

		EditorGUILayout.LabelField("Metadata:");

		this.metadata.displayName = EditorGUILayout.TextField("Display Name", this.metadata.displayName);
		this.metadata.suggestedBots = EditorGUILayout.IntField("Suggested Bots", this.metadata.suggestedBots);
		this.metadata.visibleInInstantAction = EditorGUILayout.Toggle("Visible in Instant Action", this.metadata.visibleInInstantAction);



		showMapInfo = EditorGUILayout.BeginFoldoutHeaderGroup(showMapInfo, "Map Tags");

		if(showMapInfo) {
			EditorGUI.indentLevel++;

			this.metadata.typePrimary = (MapMetaData.MapType)EditorGUILayout.EnumPopup("Primary type", this.metadata.typePrimary);
			this.metadata.typeSecondary = (MapMetaData.MapType)EditorGUILayout.EnumPopup("Secondary type", this.metadata.typeSecondary);
			this.metadata.theme = (MapMetaData.MapTheme)EditorGUILayout.EnumPopup("Theme", this.metadata.theme);

			EditorGUI.indentLevel--;
		}

		EditorGUILayout.EndFoldoutHeaderGroup();

		showLoadingScreenInfo = EditorGUILayout.BeginFoldoutHeaderGroup(showLoadingScreenInfo, "Loading Screen");

		if (showLoadingScreenInfo) {
			EditorGUI.indentLevel++;
			this.metadata.loadingScreenBackgroundImage = EditorGUILayout.TextField("Background (png/jpg)", this.metadata.loadingScreenBackgroundImage);
			this.metadata.loadingScreenModel = EditorGUILayout.TextField("Decoration Model (obj)", this.metadata.loadingScreenModel);
			this.metadata.loadingScreenTexture = EditorGUILayout.TextField("Decor. Texture (png/jpg)", this.metadata.loadingScreenTexture);
			this.metadata.loadingScreenFOV = EditorGUILayout.FloatField("Field Of View", this.metadata.loadingScreenFOV);
			this.metadata.loadingScreenUseArtworkShader = EditorGUILayout.Toggle("Use Artwork Shader", this.metadata.loadingScreenUseArtworkShader);
			EditorGUI.indentLevel--;
		}

		EditorGUILayout.EndFoldoutHeaderGroup();

		if (GUILayout.Button("Save Changes")) {
			SaveMetadata();
		}
	}
}
