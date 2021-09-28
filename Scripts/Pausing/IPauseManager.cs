namespace Fossil
{
    public interface IPauseManager
    {
        PauseState PauseState { get; }
        void Pause();
        void Unpause();
        LowerTimeBoundModifier GetLowerTimeBoundModifier();
    }
}