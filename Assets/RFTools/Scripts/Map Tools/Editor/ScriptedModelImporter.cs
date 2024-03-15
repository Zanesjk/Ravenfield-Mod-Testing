using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using System.IO;

public class ScriptedModelImporter : AssetPostprocessor
{
	void OnPreprocessModel() {
		// Force Read/Write enabled to ensure navmesh generator has collider mesh data available.
		var importer = assetImporter as ModelImporter;
		importer.isReadable = true;
	}
}
