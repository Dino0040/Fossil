using UnityEngine;
using UnityEngine.Audio;
namespace Fossil
{
    [System.Serializable]
    public class MixerValueAccessor : IFloatValueAccessor
    {
        public AudioMixer mixer;
        public string parameterName;

        public float GetValue()
        {
            mixer.GetFloat(parameterName, out float f);
            return Mathf.InverseLerp(-80, 0, f);
        }

        public void SetValue(float value)
        {
            mixer.SetFloat(parameterName, Mathf.Lerp(-80, 0, value));
        }
    }
}