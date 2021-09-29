using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
namespace Fossil
{
    public class PauseManager : MonoBehaviour, IPauseManager
    {
        public GameObject pauseOverlay;

        PauseState pauseState = PauseState.running;

        public AudioMixer audioMixer;
        public string muteParameterName;
        public float muteFadeDuration = 0.4f;
        float previousParameterValue;

        HashSet<LowerTimeBoundModifier> lowerTimeBoundModifiers = new HashSet<LowerTimeBoundModifier>();
        LowerTimeBoundModifier lowerTimeBoundModifier;

        public PauseState PauseState { get => pauseState; }

        private void Awake()
        {
            GlobalReferenceProvider.Register(typeof(PauseManager), this);
            GlobalReferenceProvider.Register(typeof(IPauseManager), this);
            lowerTimeBoundModifier = GetLowerTimeBoundModifier();
            lowerTimeBoundModifier.value = 1;
        }

        [ContextMenu("Pause")]
        public void Pause()
        {
            if (pauseState != PauseState.running)
            {
                return;
            }
            if (pauseOverlay)
            {
                pauseOverlay.SetActive(true);
            }
            lowerTimeBoundModifier.value = 0;

            if (audioMixer)
            {
                pauseState = PauseState.pausing;
                audioMixer.GetFloat(muteParameterName, out float val);
                previousParameterValue = val;

                Sequence s = DOTween.Sequence();
                s.Append(audioMixer.DOSetFloat(muteParameterName, -70.0f, muteFadeDuration));
                s.AppendCallback(() => pauseState = PauseState.paused);
                s.SetUpdate(true);
            }
            else
            {
                pauseState = PauseState.paused;
            }
        }

        [ContextMenu("Unpause")]
        public void Unpause()
        {
            if (pauseState != PauseState.paused)
            {
                return;
            }
            if (pauseOverlay)
            {
                pauseOverlay.SetActive(false);
            }
            lowerTimeBoundModifier.value = 1;

            if (audioMixer)
            {
                pauseState = PauseState.resuming;

                Sequence s = DOTween.Sequence();
                s.Append(audioMixer.DOSetFloat(muteParameterName, previousParameterValue, muteFadeDuration));
                s.AppendCallback(() => pauseState = PauseState.running);
                s.SetUpdate(true);
            }
            else
            {
                pauseState = PauseState.running;
            }
        }

        private void Update()
        {
            float minimum = float.MaxValue;
            foreach(LowerTimeBoundModifier modifier in lowerTimeBoundModifiers)
            {
                minimum = Mathf.Min(minimum, modifier.value);
            }
            Time.timeScale = minimum;
        }

        public LowerTimeBoundModifier GetLowerTimeBoundModifier()
        {
            return new LowerTimeBoundModifier(lowerTimeBoundModifiers);
        }
    }
}