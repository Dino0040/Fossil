using System;
namespace Fossil
{
    public enum EndBehaviour
    {
        Stop,
        Restart
    }

    public class Countdown
    {
        float resetValue;
        public float ResetValue => resetValue;
        EndBehaviour endBehaviour;
        public event EventHandler OnReachingEnd;
        int endReachedCount = 0;

        bool stopped = false;
        float buffer;

        bool lockedEnd;
        public bool EndIsLocked => lockedEnd;
        public bool IsRunning => !stopped;
        public bool IsStopped => stopped;
        public float Value => IsRunning ? buffer : 0;

        public Countdown(float initialValue, EndBehaviour endBehaviour)
        {
            resetValue = initialValue;
            buffer = initialValue;
            this.endBehaviour = endBehaviour;
        }

        public void LockEnd()
        {
            lockedEnd = true;
        }

        public void UnlockEnd()
        {
            lockedEnd = false;
        }

        public void DecreaseValue(float delta)
        {
            if (stopped)
            {
                return;
            }

            buffer -= delta;

            if (lockedEnd)
            {
                if (buffer <= 0)
                {
                    buffer = 0;
                }
            }
            else
            {
                while (buffer <= 0)
                {
                    if (OnReachingEnd != null)
                    {
                        OnReachingEnd.Invoke(this, null);
                    }
                    else
                    {
                        endReachedCount++;
                    }

                    buffer += resetValue;

                    if (endBehaviour == EndBehaviour.Stop)
                    {
                        stopped = true;
                    }
                }
            }
        }

        public void Reset()
        {
            stopped = false;
            buffer = resetValue;
        }

        public void ResetToZero()
        {
            stopped = false;
            buffer = 0;
        }

        public void Stop()
        {
            stopped = true;
        }

        public void Resume()
        {
            stopped = false;
        }

        public void SetResetValue(float resetValue)
        {
            this.resetValue = resetValue;
        }

        /// <summary>Decrement restart count; Returns true if count was > 0.</summary>
        public bool UseEndReachedCount()
        {
            if (endReachedCount > 0)
            {
                endReachedCount--;
                return true;
            }
            return false;
        }
    }
}