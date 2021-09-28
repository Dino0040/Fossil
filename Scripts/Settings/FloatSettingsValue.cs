using UnityEngine;
namespace Fossil
{
    [CreateAssetMenu(fileName = "FloatSettingsValue", menuName = "Fossil/Menu/Settings/Float")]
    public class FloatSettingsValue : SettingsValue<float>
    {
        [SerializeReference]
        public IFloatValueAccessor valueAccessor;

        public float min = 0.0f;
        public float max = 1.0f;
        public float stepsize = 0.1f;

        public override float GetValue()
        {
            return valueAccessor.GetValue();
        }

        public override void SetValue(float value)
        {
            valueAccessor.SetValue(value);
        }
    }
}