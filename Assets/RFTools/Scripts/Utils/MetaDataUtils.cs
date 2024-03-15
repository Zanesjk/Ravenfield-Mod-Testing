using System.IO;
using System.Text;
using UnityEngine;

public static class MetaDataUtils
{
	const string META_DATA_EXTENSION = ".json";

	public static string FormatAssetBundleName(string sourceObjectName) {
		return sourceObjectName.ToLowerInvariant();
	}

    public static string GetMetaDataPath(string contentFilePath) {
		return $"{contentFilePath}{META_DATA_EXTENSION}";
	}

	public static bool MetaDataExistsFor(string contentFilePath) {
		return File.Exists(GetMetaDataPath(contentFilePath));
	}

	public static void WriteMetaData<T>(string contentFilePath, T value) {
		var metaDataPath = GetMetaDataPath(contentFilePath);
		var json = JsonUtility.ToJson(value, true);
		File.WriteAllText(metaDataPath, json, Encoding.UTF8);

#if UNITY_EDITOR
		UnityEditor.AssetDatabase.Refresh();
#endif
	}

	public static bool ReadMetaData<T>(string contentFilePath, out T data, T defaultValue) {
		data = defaultValue;
		var metaDataPath = GetMetaDataPath(contentFilePath);

		if(!File.Exists(metaDataPath)) {
			return false;
		}

		try {
			var json = File.ReadAllText(metaDataPath, Encoding.UTF8);

			// Box data type as object to ensure it's a reference type, as FromJsonOverwrite result will not actually be written into value types.
			var mo = (System.Object)data;
			JsonUtility.FromJsonOverwrite(json, mo);
			data = (T)mo;

			return true;
		}
		catch(System.Exception e) {
			Debug.LogException(e);
			return false;
		}
	}

	public static FileInfo CopyFileAndMetaData(string contentFilePath, string destinationFilePath) {
		File.Copy(contentFilePath, destinationFilePath, true);
		CopyMetaData(contentFilePath, destinationFilePath);

		return new FileInfo(destinationFilePath);
	}

	public static void CopyMetaData(string contentFilePath, string destinationFilePath) {
		if (MetaDataExistsFor(contentFilePath)) {
			File.Copy(GetMetaDataPath(contentFilePath), GetMetaDataPath(destinationFilePath), true);
		}
	}

	public static void DeleteFileAndMetaData(string contentFilePath) {
		File.Delete(contentFilePath);
		if (MetaDataExistsFor(contentFilePath)) {
			File.Delete(GetMetaDataPath(contentFilePath));
		}
	}
}
