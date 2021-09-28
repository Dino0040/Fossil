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
            return f;
        }

        public void SetValue(float value)
        {
            mixer.SetFloat(parameterName, value);
        }
    }
}