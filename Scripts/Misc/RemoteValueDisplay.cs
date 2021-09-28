using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
namespace Fossil
{
    public class RemoteValueDisplay : MonoBehaviour
    {
        public MonoBehaviour valueHolder;
        public string valueName;
        public Color highlightColor;
        public float highlightPunchStrength = 0.1f;
        public string floatFormatString;
        public Color zeroColor;
        public bool noAnimation = false;

        Text text;
        System.Reflection.FieldInfo field;
        private void Awake()
        {
            text = GetComponentInChildren<Text>();
        }

        Color initColor;
        private void Start()
        {
            initColor = text.color;
            field = valueHolder.GetType().GetField(valueName);
            oldValue = field.GetValue(valueHolder);
            newValue = field.GetValue(valueHolder);
            UpdateDisplayValue();
        }

        object oldValue;
        object newValue;
        void Update()
        {
            if (valueHolder)
            {
                newValue = field.GetValue(valueHolder);
                if (!newValue.Equals(oldValue))
                {
                    UpdateDisplayValue();
                    oldValue = newValue;
                }
            }
        }

        Sequence newValueAnimation;
        void UpdateDisplayValue()
        {
            if (newValue is float f)
            {
                text.text = f.ToString(floatFormatString);
            }
            else
            {
                text.text = newValue.ToString();
            }
            if (newValueAnimation != null)
            {
                if (!newValueAnimation.IsComplete())
                {
                    newValueAnimation.Complete();
                }
                PlayNewValueAnimation();
            }
            else
            {
                PlayNewValueAnimation();
            }
        }

        void PlayNewValueAnimation()
        {
            if (noAnimation)
            {
                return;
            }

            newValueAnimation = DOTween.Sequence();
            bool isZero = false;

            if (newValue is int)
            {
                if ((int)newValue == 0)
                {
                    newValueAnimation.Append(text.DOColor(zeroColor, 0.1f));
                    isZero = true;
                }
            }

            if (!isZero && (int)newValue >= (int)oldValue)
            {
                newValueAnimation.Append(text.DOColor(highlightColor, 0.1f)).AppendInterval(0.4f).Append(text.DOColor(initColor, 0.6f));
            }
            if (!isZero && (int)newValue < (int)oldValue)
            {
                newValueAnimation.Append(text.DOColor(zeroColor, 0.1f)).AppendInterval(0.4f).Append(text.DOColor(initColor, 0.6f));
            }
            newValueAnimation.Insert(0, text.transform.DOPunchScale(Vector3.one * 0.2f, 0.6f));
            newValueAnimation.SetAutoKill(false);
            newValueAnimation.Play();
        }
    }
}