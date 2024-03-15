using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Ravenfield.Trigger;
using System;

[CustomEditor(typeof(ScriptedPathGroup))]
public class ScriptedPathGroupEditor : Editor {

	public static GUIStyle syncLabelStyle {
		get {
			if(_syncLabelStyle == null) {
				_syncLabelStyle = new GUIStyle() {
					fontSize = 14,
					normal = {
						textColor = Color.white,
					},
				};
			}

			return _syncLabelStyle;
		}
	}
	static GUIStyle _syncLabelStyle;

	static readonly Color[] pathColors = new Color[] {
		new Color(0f, 0f, 1f),
		new Color(0f, 0.5f, 1f),
		new Color(0f, 1f, 1f),
		new Color(0.5f, 0f, 1f),
		new Color(1f, 0f, 1f),
		new Color(1f, 0f, 0.5f),
		new Color(1f, 0f, 0f),
	};

	ScriptedPathGroup pathGroup;

	List<EditorPathPreview> pathPreviews;

	float prevTime;
	bool isPlayingPreview = false;

	void OnEnable() {
		this.pathGroup = this.target as ScriptedPathGroup;
		UpdateInternalPathsList();

		this.prevTime = Time.realtimeSinceStartup;
	}

	void UpdateInternalPathsList() {
		this.pathGroup.FindPaths();
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		GUILayout.Label(string.Format("Paths: {0}", this.pathGroup.paths.Count));
		if(GUILayout.Button("Create Path")) {
			CreateNewPath();
		}
	}

	void CreateNewPath() {
		UpdateInternalPathsList();
		int pathIndex = this.pathGroup.paths.Count;

		var pathGO = new GameObject($"Path {pathIndex}", typeof(ScriptedPath));
		Undo.RegisterCreatedObjectUndo(pathGO, "Create Path");
		pathGO.transform.SetParent(this.pathGroup.transform);
		pathGO.transform.localPosition = new Vector3(pathIndex, 0f, 0f);
		pathGO.transform.localRotation = Quaternion.identity;
		UpdateInternalPathsList();

		Selection.activeObject = pathGO;
	}

	void PlayPreview(byte syncNumber = byte.MaxValue) {

		this.isPlayingPreview = true;

		int nSeekers = this.pathGroup.paths.Count;
		this.pathPreviews = new List<EditorPathPreview>(nSeekers);
		var seekers = new ScriptedPathSeeker[nSeekers];

		for (int i = 0; i < nSeekers; i++) {
			this.pathPreviews.Add(new EditorPathPreview(this.pathGroup.paths[i]));
			seekers[i] = this.pathPreviews[i].seeker;
		}
		
		if (syncNumber == byte.MaxValue) {
			for (int i = 0; i < this.pathPreviews.Count; i++) {
				this.pathPreviews[i].Play(0);
			}
		}
		else {
			for (int i = 0; i < this.pathPreviews.Count; i++) {
				this.pathPreviews[i].PlayFromSyncNumber(syncNumber);
			}
		}

		this.pathGroup.Play(seekers);
	}

	public static bool GetPlayFromSyncNodeInput(Event e, out byte syncNumber) {
		syncNumber = 0;

		if (e.type == EventType.KeyDown) {
			if (e.keyCode == KeyCode.Alpha0) {
				syncNumber = 0;
				e.Use();
			}
			else if (e.keyCode == KeyCode.Alpha1) {
				syncNumber = 1;
				e.Use();
			}
			else if (e.keyCode == KeyCode.Alpha2) {
				syncNumber = 2;
				e.Use();
			}
			else if (e.keyCode == KeyCode.Alpha3) {
				syncNumber = 3;
				e.Use();
			}
			else if (e.keyCode == KeyCode.Alpha4) {
				syncNumber = 4;
				e.Use();
			}
			else if (e.keyCode == KeyCode.Alpha5) {
				syncNumber = 5;
				e.Use();
			}
			else if (e.keyCode == KeyCode.Alpha6) {
				syncNumber = 6;
				e.Use();
			}
			else if (e.keyCode == KeyCode.Alpha7) {
				syncNumber = 7;
				e.Use();
			}
			else if (e.keyCode == KeyCode.Alpha8) {
				syncNumber = 8;
				e.Use();
			}
			else if (e.keyCode == KeyCode.Alpha9) {
				syncNumber = 9;
				e.Use();
			}
			return e.type == EventType.Used;
		}

		return false;
	}

	void OnSceneGUI() {

		Event e = Event.current;

		foreach (var path in this.pathGroup.paths) {
			path.UpdateTransformMatrix();
		}

		byte syncNumber;
		if (e.type == EventType.KeyDown) {
			if (e.keyCode == KeyCode.Space) {
				PlayPreview();
				e.Use();
			}
			else if (GetPlayFromSyncNodeInput(e, out syncNumber)) {
				PlayPreview(syncNumber);
			}
			else if(e.keyCode == KeyCode.Tab) {
				Selection.activeGameObject = this.pathGroup.paths[0].gameObject;
			}
		}

		for (int i = 0; i < this.pathGroup.paths.Count; i++) {
			DrawPath(this.pathGroup.paths[i], pathColors[i%pathColors.Length]);
		}

		if (e.type == EventType.Repaint) {
			float newTime = Time.realtimeSinceStartup;
			float dt = newTime - this.prevTime;
			this.prevTime = newTime;

			UpdatePreviews(dt);
		}
	}

	public override bool RequiresConstantRepaint() {
		return this.isPlayingPreview;
	}

	private void UpdatePreviews(float dt) {
		// Catch any exception as values can be set to null on in-editor script reloads.
		try {
			if (this.isPlayingPreview) {

				this.pathPreviews.Sort(BackToFrontPreviewSort);

				this.isPlayingPreview = false;
				foreach (var preview in this.pathPreviews) {
					if (preview.IsPlaying()) {
						this.isPlayingPreview = true;
						preview.Update(dt);
					}
				}
			}

			this.pathGroup.Update();
		}
		catch (System.Exception e) {
			Debug.LogException(e);
			this.isPlayingPreview = false;
		}
	}

	public static int BackToFrontPreviewSort(EditorPathPreview a, EditorPathPreview b) {
		return b.cameraDistance.CompareTo(a.cameraDistance);
	}

	public static void DrawPath(ScriptedPath path, Color c) {
		int nNodes = path.GetNodeCount();

		path.UpdateTransformMatrix();

		Handles.color = c;

		for (int i = 0; i < nNodes; i++) {
			DrawNode(path, i, path.loop || i < nNodes - 1);
		}

		for (int i = 0; i < path.modifiers.Count; i++) {
			DrawModifier(path, i);
		}
	}

	public static bool IsInFrontOfCamera(Vector3 pos) {
		var viewportPoint = SceneView.lastActiveSceneView.camera.WorldToViewportPoint(pos);

		return viewportPoint.x > 0f && viewportPoint.x < 1f && viewportPoint.y > 0f && viewportPoint.y < 1f && viewportPoint.z > 1f;
	}

	static void DrawNode(ScriptedPath path, int index, bool drawEdge) {
		var nodePos = path.GetNodeWorldPosition(index);
		var node = path.pathNodes[index];

		Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;

		bool isInFrontOfCamera = IsInFrontOfCamera(nodePos);

		bool isClickable = false;

		if(isClickable) {
			if (isInFrontOfCamera && Handles.Button(nodePos, Quaternion.identity, 0.3f, 0.3f, Handles.CubeHandleCap)) {

				Selection.activeObject = path.gameObject;
			}
		}
		else {
			Handles.CubeHandleCap(-1, nodePos, Quaternion.identity, 0.3f, EventType.Repaint);
		}
		
		

		if (drawEdge) {
			Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
			int nextIndex = (index + 1) % path.GetNodeCount();
			var nextPos = path.GetNodeWorldPosition(nextIndex);
			Handles.DrawLine(nodePos, nextPos);
		}

		if(isInFrontOfCamera && node.synchronize) {
			DrawSyncLabel(nodePos, node.syncNumber);
		}
	}

	static void DrawModifier(ScriptedPath path, int index) {
		var modifier = path.modifiers[index];

		int nextEdge = (modifier.edgeIndex + 1) % path.GetNodeCount();

		var prevNode = path.GetNodeWorldPosition(modifier.edgeIndex);
		var nextNode = path.GetNodeWorldPosition(nextEdge);
		var delta = nextNode - prevNode;

		Vector3 handlePosition = prevNode + delta * modifier.edgeT;

		Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;

		if(IsInFrontOfCamera(handlePosition) && Handles.Button(handlePosition, Quaternion.identity, 0.2f, 0.2f, Handles.SphereHandleCap)) {
			Selection.activeObject = path.gameObject;
		}
	}

	public static void DrawSyncLabel(Vector3 nodePos, byte syncNumber) {

		var guiPos = HandleUtility.WorldToGUIPoint(nodePos);
		Rect rect = new Rect(guiPos, new Vector2(80f, 18f));

		Handles.BeginGUI();

		EditorGUI.DrawRect(rect, new Color(0f, 0f, 0f, 0.7f));
		GUILayout.BeginArea(rect);

		GUILayout.Label(string.Format("SYNC {0}", syncNumber), syncLabelStyle);

		GUILayout.EndArea();
		Handles.EndGUI();
	}
}
