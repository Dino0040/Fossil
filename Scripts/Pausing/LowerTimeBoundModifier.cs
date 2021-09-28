using System;
using System.Collections.Generic;

public class LowerTimeBoundModifier : IDisposable
{
    readonly HashSet<LowerTimeBoundModifier> lowerTimeBoundModifiers;

    public float value = float.MaxValue;

    public LowerTimeBoundModifier(HashSet<LowerTimeBoundModifier> pauseBlockers)
    {
        lowerTimeBoundModifiers = pauseBlockers;
        pauseBlockers.Add(this);
    }

    public void Dispose()
    {
        lowerTimeBoundModifiers.Remove(this);
    }
}