using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Fossil
{
    [CreateAssetMenu(fileName = "AudioPresetGroup", menuName = "Fossil/Audio/Preset Group")]
    public class AudioPresetGroup : AudioPresetProvider
    {
        public enum Behaviour
        {
            order,
            randomOrder,
            fullRandom,
        }

        public Behaviour behaviour;
        public bool useSingleAudioPresetAsTemplate;

        [ReorderableList, HideIf("useSingleAudioPresetAsTemplate")]
        public List<AudioPresetProvider> presets = new List<AudioPresetProvider>();

        [ShowIf("useSingleAudioPresetAsTemplate")]
        public AudioPreset template;
        [ReorderableList, ShowIf("useSingleAudioPresetAsTemplate")]
        public List<AudioClip> clips = new List<AudioClip>();

        AudioPreset templateInstance;

        List<int> playlist;

        public void OnValidate()
        {
            playlist = new List<int>();
            switch (behaviour)
            {
                case Behaviour.order:
                    for (int i = 0; i < (useSingleAudioPresetAsTemplate ? clips.Count : presets.Count); i++)
                    {
                        playlist.Add(i);
                    }
                    break;
                case Behaviour.randomOrder:
                    for (int i = 0; i < (useSingleAudioPresetAsTemplate ? clips.Count : presets.Count); i++)
                    {
                        playlist.Insert(Random.Range(0, playlist.Count), i);
                    }
                    break;
            }
            if(templateInstance != null)
            {
                if(Application.isPlaying)
                {
                    Destroy(templateInstance);
                }
                else
                {
                    DestroyImmediate(templateInstance);
                }
            }
            if (useSingleAudioPresetAsTemplate && template)
            {
                templateInstance = Instantiate(template.GetPreset());
            }
        }

        int playlistPosition = -1;

        public override AudioPreset GetPreset()
        {
            if (useSingleAudioPresetAsTemplate && !templateInstance)
            {
                OnValidate();
            }
            if (useSingleAudioPresetAsTemplate)
            {
                if (behaviour == Behaviour.fullRandom)
                {
                    templateInstance.audioClip = clips[Random.Range(0, clips.Count)];
                    return templateInstance;
                }
                else
                {
                    playlistPosition = (playlistPosition + 1) % playlist.Count;
                    templateInstance.audioClip = clips[playlist[playlistPosition]];
                    return templateInstance;
                }
            }
            else
            {
                if (behaviour == Behaviour.fullRandom)
                {
                    return presets[Random.Range(0, presets.Count)].GetPreset();
                }
                else
                {
                    playlistPosition = (playlistPosition + 1) % playlist.Count;
                    return presets[playlist[playlistPosition]].GetPreset();
                }
            }
        }
    }
}
