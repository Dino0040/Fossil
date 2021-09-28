using System.Collections.Generic;
using UnityEngine;
namespace Fossil
{
    public class SoundManager : MonoBehaviour, ISoundManager
    {
        public GameObject singlePlaySoundPrefab;

        public Dictionary<AudioPresetProvider, float> lastPlayedTime;
        public float minSoundTimeDistance = 0.1f;

        private void Awake()
        {
            lastPlayedTime = new Dictionary<AudioPresetProvider, float>();
            GlobalReferenceProvider.Register(typeof(ISoundManager), this);
            GlobalReferenceProvider.Register(typeof(SoundManager), this);
        }

        public AudioPresetLoader PlaySound(AudioPresetProvider audioPreset)
        {
            if (lastPlayedTime.TryGetValue(audioPreset, out float time))
            {
                if (Time.unscaledTime - time < minSoundTimeDistance)
                    return null;
            }
            lastPlayedTime[audioPreset] = Time.unscaledTime;
            AudioPresetLoader audioPresetLoader = Instantiate(singlePlaySoundPrefab).GetComponent<AudioPresetLoader>();
            audioPresetLoader.audioPreset = audioPreset;
            return audioPresetLoader;
        }

        public AudioPresetLoader PlaySoundAtPosition(AudioPresetProvider audioPreset, Vector3 position)
        {
            if (lastPlayedTime.TryGetValue(audioPreset, out float time))
            {
                if (Time.unscaledTime - time < minSoundTimeDistance)
                    return null;
            }
            AudioPresetLoader audioPresetLoader = PlaySound(audioPreset);
            audioPresetLoader.transform.position = position;
            return audioPresetLoader;
        }

        public AudioPresetLoader PlaySoundAttachedTo(AudioPresetProvider audioPreset, Transform emittingTransform, Vector3 relativeOffset)
        {
            if (lastPlayedTime.TryGetValue(audioPreset, out float time))
            {
                if (Time.unscaledTime - time < minSoundTimeDistance)
                    return null;
            }
            AudioPresetLoader audioPresetLoader = PlaySound(audioPreset);
            CopyTransformProperties copyTransformProperties = audioPresetLoader.gameObject.AddComponent<CopyTransformProperties>();
            copyTransformProperties.target = emittingTransform;
            copyTransformProperties.copyPosition = true;
            copyTransformProperties.positionOffset = relativeOffset;
            copyTransformProperties.CopyPosition();
            return audioPresetLoader;
        }

        public AudioPresetLoader PlaySoundAttachedTo(AudioPresetProvider audioPreset, Transform emittingTransform)
        {
            return PlaySoundAttachedTo(audioPreset, emittingTransform, Vector3.zero);
        }
    }
}