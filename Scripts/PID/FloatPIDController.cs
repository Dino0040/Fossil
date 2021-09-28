using UnityEngine;
namespace Fossil
{
    [System.Serializable]
    public class FloatPIDController : GenericPIDController<float, float>
    {
        public FloatPIDController() : base(
            (vecA, vecB) => vecA + vecB,
            (vec, scalar) => vec * scalar,
            (vec, magnitude) => Mathf.Clamp(vec, -magnitude, magnitude),
            (vec, min, max) => Mathf.Clamp(vec, min, max))
        {

        }
    }
}