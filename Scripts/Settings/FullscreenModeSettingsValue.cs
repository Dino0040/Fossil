using System.Collections.Generic;
using UnityEngine;
namespace Fossil
{
    [CreateAssetMenu(fileName = "FullscreenModeSettingsValue", menuName = "Fossil/Menu/Settings/Fullscreen Mode")]
    public class FullscreenModeSettingsValue : ExclusiveSettingsValue
    {
        Dictionary<FullScreenMode, string> modeNames = new Dictionary<FullScreenMode, string>();

        private void OnEnable()
        {
            modeNames = new Dictionary<FullScreenMode, string>();
            modeNames.Add(FullScreenMode.ExclusiveFullScreen, "Exclusive Fullscreen");
            modeNames.Add(FullScreenMode.FullScreenWindow, "Fullscreen Window");
            modeNames.Add(FullScreenMode.MaximizedWindow, "Maximized Window");
            modeNames.Add(FullScreenMode.Windowed, "Windowed");
        }

        public override List<ExclusiveOption> GetPossibleValues()
        {
            List<ExclusiveOption> options = new List<ExclusiveOption>();
            foreach (FullScreenMode mode in System.Enum.GetValues(typeof(FullScreenMode)))
            {
                options.Add(new ExclusiveOption { value = mode, displayName = modeNames[mode] });
            }
            return options;
        }

        public override ExclusiveOption GetValue()
        {
            ExclusiveOption option = new ExclusiveOption
            {
                value = Screen.fullScreenMode,
                displayName = modeNames[Screen.fullScreenMode]
            };
            return option;
        }

        public override void SetValue(ExclusiveOption option)
        {
            Screen.fullScreenMode = (FullScreenMode)option.value;
        }
    }
}