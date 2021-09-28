using UnityEngine;

namespace Fossil
{
    [System.Serializable]
    public class AnglePIDController : GenericPIDController<float, float>
    {
        public AnglePIDController() : base(
            (vecA, vecB) => vecA + vecB,
            (vec, scalar) => vec * scalar,
            (vec, magnitude) => Mathf.Clamp(vec, -magnitude, magnitude),
            (vec, min, max) => Mathf.Clamp(vec, min, max))
        {

        }

        new public float CalculateControlValue(float value, float target, float seconds)
        {
            float delta = (target - value + 540) % 360 - 180;
            float control = base.CalculateControlValue(value, value + delta, seconds);
            return control;
        }
    }
}