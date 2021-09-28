namespace Fossil
{
    public abstract class SettingsValue<T> : MenuItem
    {
        public abstract T GetValue();

        public abstract void SetValue(T value);
    }
}