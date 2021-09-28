using System.Collections.Generic;
using UnityEngine;
namespace Fossil
{
    public abstract class MenuItem : ScriptableObject
    {
        public string displayName;
        public List<RuntimePlatform> hideForPlatform;
    }
}