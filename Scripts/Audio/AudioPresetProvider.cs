using UnityEngine;

namespace Fossil
{
    public abstract class AudioPresetProvider : ScriptableObject
    {
        public abstract AudioPreset GetPreset();
    }
}