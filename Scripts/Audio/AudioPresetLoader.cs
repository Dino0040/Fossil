using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace Fossil
{
    public class AudioPresetLoader : MonoBehaviour
    {
        public AudioPresetProvider audioPreset;
        [System.NonSerialized]
        public AudioPreset currentPreset;
        public bool autoplay = false;
        public bool destroyOnFinish = false;
        public bool randomStartSample = false;
        public PanningSource panningSource = PanningSource.Spatial;

        AudioSource source;
        Camera mainCamera;
        int lastAudioSample = -1;

        bool hasPlayed = false;

        public AudioPresetLoader(AudioPreset audioPreset)
        {
            this.audioPreset = audioPreset;
        }

        public void Awake()
        {
            lastAudioSample = -1;
            source = GetComponent<AudioSource>();
            if (source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
            }
            source.spatialBlend = 0.0f;
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorApplication.update += Update;
            }
#endif
        }

        public AudioSource GetSource()
        {
            return source;
        }

        private void Start()
        {
            if (autoplay)
            {
                Play();
            }
        }

        private void OnDisable()
        {
            if (currentPreset != null)
            {
                if (currentPreset.looping)
                {
                    lastAudioSample = source.timeSamples;
                }
            }
        }

        private void OnEnable()
        {
            if (audioPreset == null)
            {
                return;
            }
            if (currentPreset != null)
            {
                if (currentPreset.looping && lastAudioSample != -1)
                {
                    Play();
                    source.timeSamples = lastAudioSample;
                }
            }
        }

        private void Update()
        {
            if (mainCamera == null)
            {
                if (Application.isPlaying)
                {
                    mainCamera = Camera.main;
                }
            }
            if (source.isPlaying)
            {
                if (panningSource == PanningSource.Screen && Application.isPlaying && currentPreset != null)
                {
                    source.panStereo = Mathf.Clamp((mainCamera.WorldToViewportPoint(transform.position).x * 2.0f - 1.0f) * currentPreset.panningStrength, -1, 1);
                    if (currentPreset.panningStrength > 0.1f)
                    {
                        source.volume = Mathf.Lerp((1.0f - Vector2.Distance(mainCamera.transform.position, transform.position) / 25.0f) * currentPreset.volume, 1, 0);
                    }
                }
            }
            else
            {
                if (hasPlayed && destroyOnFinish)
                {
                    if (Application.isPlaying)
                    {
                        Destroy(gameObject);
                    }
#if UNITY_EDITOR
                    else
                    {
                        EditorApplication.update -= Update;
                        DestroyImmediate(gameObject);
                    }
#endif
                }
            }
        }

        public void Play()
        {
            Play(true);
        }

        public void Play(bool getNewPreset)
        {
            if (getNewPreset)
            {
                currentPreset = audioPreset.GetPreset();
            }
            if (audioPreset == null)
            {
                Debug.LogWarning("Trying to play sound, but audioPreset is null!", this);
            }
            source.loop = currentPreset.looping;
            source.volume = currentPreset.volume;
            if (panningSource == PanningSource.Screen)
            {
                source.spatialBlend = 0;
            }
            if (panningSource == PanningSource.Spatial)
            {
                source.spatialBlend = currentPreset.panningStrength;
            }
            source.outputAudioMixerGroup = currentPreset.mixerGroup;
            source.pitch = currentPreset.pitchMin + Random.value * (currentPreset.pitchMax - currentPreset.pitchMin);
            source.clip = currentPreset.audioClip;
            if (randomStartSample)
            {
                source.timeSamples = (int)(currentPreset.audioClip.samples * Random.value);
            }
            source.Play();
            hasPlayed = true;
        }

        public void FadeOut(float duration, System.Action callback)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(source.DOFade(0.0f, duration)).AppendCallback(() => { source.Stop(); callback(); });
            sequence.SetUpdate(true);
            sequence.Play();
        }

        public void Stop()
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }

        public bool IsPlaying()
        {
            return source.isPlaying;
        }

        public void Destroy()
        {
            if (Application.isPlaying)
            {
                Destroy(gameObject);
            }
#if UNITY_EDITOR
            else
            {
                EditorApplication.update -= Update;
                DestroyImmediate(gameObject);
            }
#endif
        }

        public enum PanningSource
        {
            Screen,
            Spatial
        }
    }
}