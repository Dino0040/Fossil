using System.Collections.Generic;
using UnityEngine;
namespace Fossil
{
    public static class GlobalReferenceProvider
    {
        static Dictionary<System.Type, Object> references;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            references = new Dictionary<System.Type, Object>();
        }

        public static void Register(System.Type type, Object newReference)
        {
            if (references.TryGetValue(type, out Object existingReference))
            {
                if (existingReference != null)
                {
                    Debug.LogWarning("Overwriting active global reference of " + type + "!");
                }
            }
            references[type] = newReference;
        }

        public static bool TryGet<T>(out T a) where T : class
        {
            if (references.TryGetValue(typeof(T), out Object reference))
            {
                if (reference != null && reference is T t)
                {
                    a = t;
                    return true;
                }
            }
            a = null;
            return false;
        }

        public static void Get<T>(out T a) where T : class
        {
            if (!TryGet(out a))
            {
                Debug.LogWarning("No global reference found for type " + typeof(T) + "!");
            }
        }

        public static void Fill<T>(ref T a) where T : class
        {
            if (!TryFill(ref a))
            {
                Debug.LogWarning("No global reference found for type " + typeof(T) + "!");
            }
        }

        public static bool TryFill<T>(ref T a) where T : class
        {
            if (a != null)
            {
                return true;
            }
            return TryGet(out a);
        }
    }
}