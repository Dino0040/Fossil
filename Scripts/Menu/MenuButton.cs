using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Fossil
{
    public class MenuButton : MonoBehaviour, IMenuCanvasItem, ISelectHandler, IDeselectHandler
    {
        public Text buttonText;
        public AudioPreset selectSound;
        public AudioPreset clickSound;
        public GameObject singlePlaySoundPrefab;
        public GameObject toggleGameobject;
        public float jumpScale = 1.05f;

        public delegate void voidCallback();

        void Start()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);
        }

        public void OnPointerEnter()
        {
            AudioPresetLoader presetLoader = Instantiate(singlePlaySoundPrefab).GetComponent<AudioPresetLoader>();
            presetLoader.audioPreset = selectSound;
            transform.DOScale(jumpScale, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);
        }

        public void OnPointerExit()
        {
            transform.DOScale(1, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);
        }

        public void Init(string s, voidCallback c)
        {
            SetText(s);
            SetCallback(c);
        }

        public void Init(string s, voidCallback c, bool enabled)
        {
            SetText(s);
            SetCallback(c);
            SetToggleStatus(enabled);
        }

        public void Close()
        {
            transform.DOScale(0, 0.5f).SetEase(Ease.InBack).SetUpdate(true).OnKill(() => Destroy(gameObject));
            Destroy(gameObject);
        }

        public void SetText(string s)
        {
            buttonText.text = s;
        }

        public void OnPress()
        {
            AudioPresetLoader presetLoader = Instantiate(singlePlaySoundPrefab).GetComponent<AudioPresetLoader>();
            presetLoader.audioPreset = clickSound;
            callback();
        }

        voidCallback callback;
        void SetCallback(voidCallback callback)
        {
            this.callback = callback;
        }

        public void SetToggleStatus(bool enabled)
        {
            toggleGameobject.SetActive(enabled);
        }

        public void OnSelect(BaseEventData eventData)
        {
            OnPointerEnter();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            OnPointerExit();
        }
    }
}