using DG.Tweening;
using UnityEngine;
namespace Fossil
{
    public class FreezeFramer : MonoBehaviour
    {
        IPauseManager pauseManager;
        public float duration = 0.04f;

        public void FreezeFrame()
        {
            if (GlobalReferenceProvider.TryFill(ref pauseManager))
            {
                LowerTimeBoundModifier timeModifier = pauseManager.GetLowerTimeBoundModifier();
                Sequence freezeSequence = DOTween.Sequence();
                freezeSequence.AppendCallback(() => timeModifier.value = 0);
                freezeSequence.AppendInterval(duration);
                freezeSequence.AppendCallback(() => timeModifier.value = 1);
                freezeSequence.AppendCallback(() => timeModifier.Dispose());
                freezeSequence.SetUpdate(true);
            }
        }
    }
}