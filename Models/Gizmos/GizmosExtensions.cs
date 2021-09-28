using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace Fossil
{
    public static class GizmosExtensions
    {
        static public Mesh Arrow => GetOrCache<Mesh>("Arrow.fbx");

#if UNITY_EDITOR
        static string selfGUID;
        static string SelfGUID => !string.IsNullOrEmpty(selfGUID) ? selfGUID : selfGUID = AssetDatabase.FindAssets("t:Script GizmosExtensions")[0];

        static Dictionary<string, Object> cache;

        static T GetOrCache<T>(string relativePath) where T : Object
        {
            if (cache == null)
            {
                cache = new Dictionary<string, Object>();
            }

            if (!cache.ContainsKey(relativePath))
            {
                string assetPath = Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(SelfGUID)).Replace("\\", "/") + "/" + relativePath;
                cache[relativePath] = AssetDatabase.LoadAssetAtPath(assetPath, typeof(T));
            }
            return (T)cache[relativePath];
        }
#else
    static T GetOrCache<T>(string relativePath) where T : Object
    {
        return null;
    }
#endif

        static public void DrawWireArrow(Vector3 position, Quaternion rotation, float size)
        {
#if UNITY_EDITOR
            Gizmos.DrawWireMesh(Arrow, 0, position, rotation, Vector3.one * size);
#endif
        }

        static public void DrawWireCircle(Vector3 position, Vector3 normal, float radius)
        {
#if UNITY_EDITOR
            Handles.color = Gizmos.color;
            Handles.DrawWireDisc(position, normal, radius);
#endif
        }
    }
}