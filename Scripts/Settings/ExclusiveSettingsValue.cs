using System.Collections.Generic;
namespace Fossil
{
    public abstract class ExclusiveSettingsValue : SettingsValue<ExclusiveOption>
    {
        public abstract List<ExclusiveOption> GetPossibleValues();
    }
}