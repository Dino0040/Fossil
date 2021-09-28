using UnityEngine;

namespace Fossil
{
    [CreateAssetMenu(fileName = "BoolSettingsValue", menuName = "Fossil/Menu/Settings/Bool")]
    public class BoolSettingsValue : SettingsValue<bool>
    {
        [SerializeReference]
        public IBoolValueAccessor valueAccessor;

        public override bool GetValue()
        {
            return valueAccessor.GetValue();
        }

        public override void SetValue(bool value)
        {
            valueAccessor.SetValue(value);
        }
    }
}