using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
namespace Fossil
{
    public class PeriodicEvent : MonoBehaviour
    {
        public UnityEvent eventToRun;
        [MinMaxSlider(0.0f, 60.0f)]
        public Vector2 countdown;
        float currentCountdown;
        public bool useUnscaledUpdate;
        public bool resetCountdownOnEnable;
        public bool runEventOnStart = true;

        void ResetCountdown()
        {
            currentCountdown = Random.Range(countdown.x, countdown.y);
        }

        private void Awake()
        {
            ResetCountdown();
        }

        private void Start()
        {
            eventToRun.Invoke();
        }

        private void OnEnable()
        {
            if (resetCountdownOnEnable)
            {
                ResetCountdown();
            }
        }

        private void Update()
        {
            currentCountdown -= useUnscaledUpdate ? Time.unscaledDeltaTime : Time.deltaTime;
            if (currentCountdown <= 0)
            {
                eventToRun.Invoke();
                ResetCountdown();
            }
        }
    }
}