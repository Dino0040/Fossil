using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
using UnityEngine;

namespace Fossil
{
    public class GlobalInstantiate : MonoBehaviour
    {
        const string prefabName = "Global";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Instantiate()
        {
            Object globalPrefab = Resources.Load(prefabName);
            if (globalPrefab)
            {
                GameObject globalPrefabInstance = Instantiate(globalPrefab) as GameObject;
                globalPrefabInstance.name = globalPrefab.name;
                DontDestroyOnLoad(globalPrefabInstance);
            }
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/Fossil/" + prefabName, false, 1)]
        private static void CreateGlobalPrefab()
        {
            Object globalPrefab = Resources.Load(prefabName);
            if (globalPrefab)
            {
                Debug.LogError($"A {prefabName} already exists!");
                return;
            }

            const string resourceFolder = "Assets/Resources/";

            if (!Directory.Exists(resourceFolder))
            {
                string folderUUID = AssetDatabase.CreateFolder("Assets", "Resources");
                if (string.IsNullOrEmpty(folderUUID))
                {
                    Debug.LogError($"Failed to create folder {resourceFolder}!");
                    return;
                }
            }

            GameObject empty = new(prefabName);
            PrefabUtility.SaveAsPrefabAssetAndConnect(empty, $"{resourceFolder}{prefabName}.prefab", InteractionMode.UserAction);
            DestroyImmediate(empty);
        }
#endif

    }
}