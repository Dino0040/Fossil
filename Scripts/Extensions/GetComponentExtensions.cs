using UnityEngine;
namespace Fossil
{
    public static class GetComponentExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Type Safety", "UNT0014:Invalid type for call to GetComponent", Justification = "T might be interface")]

        static public bool TryGetComponentInParent<T>(this Component self, out T component) where T : class
        {
            component = self.GetComponentInParent<T>();
            return component != null;
        }

        static public bool TryGetComponentInParent<T>(this GameObject self, out T component) where T : class
        {
            component = self.GetComponentInParent<T>();
            return component != null;
        }

        static public bool TryGetComponentInChildren<T>(this Component self, out T component) where T : class
        {
            component = self.GetComponentInChildren<T>();
            return component != null;
        }

        static public bool TryGetComponentInChildren<T>(this GameObject self, out T component) where T : class
        {
            component = self.GetComponentInChildren<T>();
            return component != null;
        }
    }
}