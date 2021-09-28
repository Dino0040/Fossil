using UnityEngine;
namespace Fossil
{
    [System.Serializable]
    public class Vector2PIDController : GenericPIDController<Vector2, float>
    {
        public Vector2PIDController() : base(
            (vecA, vecB) => vecA + vecB,
            (vec, scalar) => vec * scalar,
            (vec, magnitude) => Vector2.ClampMagnitude(vec, magnitude),
            null)
        {

        }
    }
}