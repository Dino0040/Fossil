namespace Fossil
{
    public interface IFloatValueAccessor : IValueAccessor<float> { }
    public interface IBoolValueAccessor : IValueAccessor<bool> { }
    public interface IIntValueAccessor : IValueAccessor<int> { }

    public interface IValueAccessor<T>
    {
        T GetValue();

        void SetValue(T value);
    }
}