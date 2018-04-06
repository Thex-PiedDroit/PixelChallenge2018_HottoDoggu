
#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class DataInvalidator : MonoBehaviour
{
	[MenuItem("Debug/ReCreate Data and Prefabs")]
	static private void ReCreateDataAndPrefabs()
	{
		string[] pAllDataObjectsGUIDs = AssetDatabase.FindAssets("t:ScriptableObject t:Prefab", new string[] { "Assets/ScriptableObjects", "Assets/Prefabs" });
		for (int i = 0; i < pAllDataObjectsGUIDs.Length; ++i)
		{
			string sPath = AssetDatabase.GUIDToAssetPath(pAllDataObjectsGUIDs[i]);
			AssetDatabase.CopyAsset(sPath, sPath);
		}
	}

	[MenuItem("Debug/Reserialize all files")]
	static private void ReserializeAllFiles()
	{
		string[] pAllDataObjectsGUIDs = AssetDatabase.FindAssets("", new string[] { "Assets" });
		string[] pAllNavMeshObjectsGUIDs = AssetDatabase.FindAssets("", new string[] { "Assets/Scenes" });

		List<string> pDataObjectsPaths = new List<string>(pAllDataObjectsGUIDs.Length);
		for (int i = 0; i < pAllDataObjectsGUIDs.Length; ++i)
		{
			string sPath = AssetDatabase.GUIDToAssetPath(pAllDataObjectsGUIDs[i]);
			if (!sPath.StartsWith("Assets/Scenes"))
				pDataObjectsPaths.Add(sPath);

			if (i < pAllNavMeshObjectsGUIDs.Length)
			{
				string sSceneObjectPath = AssetDatabase.GUIDToAssetPath(pAllNavMeshObjectsGUIDs[i]);
				if (sSceneObjectPath.EndsWith("NavMesh.asset"))
					pDataObjectsPaths.Add(sSceneObjectPath);
			}
		}

		AssetDatabase.ForceReserializeAssets(pDataObjectsPaths, ForceReserializeAssetsOptions.ReserializeAssets);
	}
}

#endif
