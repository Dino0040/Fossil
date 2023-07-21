using NaughtyAttributes;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace Fossil
{
    [CreateAssetMenu(fileName = "AudioPreset", menuName = "Fossil/Audio/Preset")]
    public class AudioPreset : AudioPresetProvider
    {
        public AudioClip audioClip;
        public AudioMixerGroup mixerGroup;
        public bool looping;
        [Range(0, 1)]
        public float volume = 0.8f;
        [Range(0, 1)]
        public float panningStrength = 1.0f;
        [Range(-3.0f, 3.0f)]
        public float pitchMin = 1.0f;
        [Range(-3.0f, 3.0f)]
        public float pitchMax = 1.0f;

        List<AudioPresetLoader> previews = new List<AudioPresetLoader>();

        public override AudioPreset GetPreset()
        {
            return this;
        }

#if UNITY_EDITOR

        [Button]
        void Preview()
        {
            GameObject g = EditorUtility.CreateGameObjectWithHideFlags("Sound Preview",
                HideFlags.HideAndDontSave,
                new System.Type[] { typeof(AudioSource), typeof(AudioPresetLoader) });

            if(Camera.main){
                g.transform.position = Camera.main.transform.position;
            }

            AudioPresetLoader audioPresetLoader = g.GetComponent<AudioPresetLoader>();
            previews.Add(audioPresetLoader);
            audioPresetLoader.audioPreset = this;
            audioPresetLoader.destroyOnFinish = true;
            audioPresetLoader.Awake();
            audioPresetLoader.Play();
        }

        [Button("Stop All Previews")]
        void StopAllPreviews()
        {
            foreach (AudioPresetLoader a in previews)
            {
                if (a != null)
                {
                    a.Destroy();
                }
            }
            previews.Clear();
        }
#endif
    }
}