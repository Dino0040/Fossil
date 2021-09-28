using System.Collections.Generic;
using UnityEngine;
namespace Fossil
{
    [RequireComponent(typeof(AudioPresetLoader))]
    public class MusicManager : MonoBehaviour
    {
        [System.Serializable]
        public class MusicTrack
        {
            public string name;
            public AudioPresetProvider musicPreset;
        }

        public List<MusicTrack> musicTracks;
        MusicTrack currentPlayingTrack;
        public AudioPresetLoader loader;
        bool fadeComplete = true;

        private void Awake()
        {
            GlobalReferenceProvider.Register(typeof(MusicManager), this);
        }

        public void FadeThenPlay(string name)
        {
            MusicTrack track = GetMusicTrackByName(name);
            if (track != null)
            {
                if (currentPlayingTrack != track)
                {
                    if (fadeComplete == true)
                    {
                        Play(track);
                        fadeComplete = false;
                    }
                    else
                    {
                        loader.FadeOut(1.0f, () => { Play(track); fadeComplete = true; });
                    }
                }
            }
        }

        void Play(MusicTrack track)
        {
            if (currentPlayingTrack != track)
            {
                currentPlayingTrack = track;
                loader.audioPreset = track.musicPreset;
                loader.Play();
                fadeComplete = false;
            }
        }

        public void Play(string name)
        {
            MusicTrack track = GetMusicTrackByName(name);
            Play(track);
        }

        public void FadeOut()
        {
            fadeComplete = false;
            loader.FadeOut(1.0f, () => { currentPlayingTrack = null; fadeComplete = true; });
        }

        MusicTrack GetMusicTrackByName(string name)
        {
            foreach (MusicTrack track in musicTracks)
            {
                if (track.name == name)
                {
                    return track;
                }
            }
            return null;
        }
    }
}