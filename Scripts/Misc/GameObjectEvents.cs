using UnityEngine;
using UnityEngine.Events;
namespace Fossil
{
    public class GameObjectEvents : MonoBehaviour
    {
        public UnityEvent startEvent = new UnityEvent();
        public UnityEvent OnEnableEvent = new UnityEvent();
        public UnityEvent onBecameInvisibleEvent = new UnityEvent();
        public UnityEvent onBecameVisibleEvent = new UnityEvent();
        public UnityEvent onDisableEvent = new UnityEvent();
        public UnityEvent onDestroyEvent = new UnityEvent();

        private void Start()
        {
            startEvent.Invoke();
        }

        private void OnEnable()
        {
            OnEnableEvent.Invoke();
        }

        private void OnBecameInvisible()
        {
            onBecameInvisibleEvent.Invoke();
        }

        private void OnBecameVisible()
        {
            onBecameVisibleEvent.Invoke();
        }

        private void OnDisable()
        {
            onDisableEvent.Invoke();
        }

        private void OnDestroy()
        {
            onDestroyEvent.Invoke();
        }
    }
}