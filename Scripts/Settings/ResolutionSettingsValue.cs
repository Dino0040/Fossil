using System.Collections.Generic;
using UnityEngine;
namespace Fossil
{
    [CreateAssetMenu(fileName = "ResolutionSettingsValue", menuName = "Fossil/Menu/Settings/Resolution")]
    public class ResolutionSettingsValue : ExclusiveSettingsValue
    {
        public override List<ExclusiveOption> GetPossibleValues()
        {
            List<ExclusiveOption> options = new List<ExclusiveOption>();
            Resolution[] resolutions = Screen.resolutions;
            foreach (var res in resolutions)
            {
                ExclusiveOption option = new ExclusiveOption
                {
                    value = res,
                    displayName = res.width + "x" + res.height + " : " + res.refreshRate
                };
                options.Add(option);
            }
            return options;
        }

        public override ExclusiveOption GetValue()
        {
            Resolution res = Screen.currentResolution;
            ExclusiveOption option = new ExclusiveOption
            {
                value = Screen.currentResolution,
                displayName = res.width + "x" + res.height + " : " + res.refreshRate
            };
            return option;
        }

        public override void SetValue(ExclusiveOption option)
        {
            Resolution resolution = (Resolution)option.value;
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, resolution.refreshRate);
        }
    }
}