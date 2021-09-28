using UnityEngine;
namespace Fossil
{
    public interface ISoundManager
    {
        AudioPresetLoader PlaySound(AudioPresetProvider audioPreset);
        AudioPresetLoader PlaySoundAtPosition(AudioPresetProvider audioPreset, Vector3 position);
        AudioPresetLoader PlaySoundAttachedTo(AudioPresetProvider audioPreset, Transform emittingTransform);
        AudioPresetLoader PlaySoundAttachedTo(AudioPresetProvider audioPreset, Transform emittingTransform, Vector3 relativeOffset);
    }
}