using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Fossil
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasFadeTransition : Transition
    {
        CanvasGroup canvasGroup;
        public float fadeDuration;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public override Coroutine EndTransition()
        {
            return StartCoroutine(EndTransitionRoutine());
        }

        public override Coroutine StartTransition()
        {
            return StartCoroutine(StartTransitionRoutine());
        }

        IEnumerator StartTransitionRoutine()
        {
            Time.timeScale = 0;

            Sequence sequence = DOTween.Sequence().SetUpdate(true);
            sequence.Append(canvasGroup.DOFade(1.0f, fadeDuration));

            yield return sequence.WaitForCompletion();

        }

        IEnumerator EndTransitionRoutine()
        {
            Sequence sequence = DOTween.Sequence().SetUpdate(true);
            sequence.Append(canvasGroup.DOFade(0.0f, fadeDuration));
            sequence.AppendCallback(() => Time.timeScale = 1.0f);

            yield return sequence.WaitForCompletion();
        }
    }
}