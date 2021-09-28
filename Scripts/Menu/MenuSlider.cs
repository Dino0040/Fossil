using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
namespace Fossil
{
    public class MenuSlider : MonoBehaviour, IMenuCanvasItem
    {
        public delegate void voidCallback(float f);
        public Slider slider;
        public Text sliderText;
        public Text valueText;
        public float stepsize = -1;

        void Start()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);
            slider.onValueChanged.AddListener(delegate
            { OnValueChanged(); });
        }

        void OnValueChanged()
        {
            ApplyStepsize();
            callback(slider.value);
            if (valueText)
            {
                valueText.text = slider.value.ToString("F2");
            }
        }

        void ApplyStepsize()
        {
            if (stepsize > 0)
            {
                slider.value = Mathf.Round(slider.value / stepsize) * stepsize;
            }
        }

        public void Init(string s, voidCallback c, float initial, float min, float max)
        {
            SetText(s);
            SetCallback(c);
            slider.minValue = min;
            slider.maxValue = max;
            slider.value = initial;
            if (valueText)
            {
                valueText.text = slider.value.ToString("F2");
            }
        }

        public void Init(string s, voidCallback c, float initial, float min, float max, float stepsize)
        {
            Init(s, c, initial, min, max);
            this.stepsize = stepsize;
        }

        public void Close()
        {
            transform.DOScale(0, 0.5f).SetEase(Ease.InBack).SetUpdate(true).OnKill(() => Destroy(gameObject));
            Destroy(gameObject);
        }

        public void SetText(string s)
        {
            sliderText.text = s;
        }

        voidCallback callback;
        void SetCallback(voidCallback callback)
        {
            this.callback = callback;
        }
    }
}