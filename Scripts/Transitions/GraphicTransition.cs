using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace Fossil
{
    public class GraphicTransition : Transition
    {
        public float duration = 1.0f;
        public bool flipXOnReturn = false;
        public bool flipYOnReturn = false;

        public AudioPresetLoader startSound;
        public AudioPresetLoader endSound;
        Material transitionMaterial;
        RectTransform rectTransform;
        int thresholdPropertyID;
        int flipXPropertyID;
        int flipYPropertyID;

        private void Awake()
        {
            transitionMaterial = GetComponent<Image>().materialForRendering;
            thresholdPropertyID = Shader.PropertyToID("_ClipThreshold");
            flipXPropertyID = Shader.PropertyToID("_FlipX");
            flipYPropertyID = Shader.PropertyToID("_FlipY");
            transitionMaterial.SetFloat(thresholdPropertyID, 1);
            Flip(true);
        }

        public override Coroutine EndTransition()
        {
            return StartCoroutine(StartTransitionRoutine());
        }

        public override Coroutine StartTransition()
        {
            return StartCoroutine(EndTransitionRoutine());
        }

        IEnumerator StartTransitionRoutine()
        {
            Flip(true);
            startSound.Play();
            yield return GetThresholdTween(1.0f).WaitForCompletion();
        }

        IEnumerator EndTransitionRoutine()
        {
            Flip(false);
            endSound.Play();
            yield return GetThresholdTween(0.0f).WaitForCompletion();
        }

        Tween GetThresholdTween(float endValue)
        {
            return DOTween.To(() => transitionMaterial.GetFloat(thresholdPropertyID),
                x => transitionMaterial.SetFloat(thresholdPropertyID, x), endValue, duration).SetEase(Ease.Linear);
        }

        void Flip(bool start)
        {
            if (!start)
            {
                transitionMaterial.SetFloat(flipXPropertyID, 0);
                transitionMaterial.SetFloat(flipYPropertyID, 0);
            }
            else
            {
                if (flipXOnReturn)
                {
                    transitionMaterial.SetFloat(flipXPropertyID, 1);
                }
                if (flipYOnReturn)
                {
                    transitionMaterial.SetFloat(flipYPropertyID, 1);
                }
            }
        }
    }
}