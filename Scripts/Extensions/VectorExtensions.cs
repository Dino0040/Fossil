using UnityEngine;
namespace Fossil
{
    public static class VectorExtensions
    {
        public static float LengthAlongDirection(this Vector2 vec, Vector2 direction)
        {
            return Vector2.Dot(vec, direction) / direction.magnitude;
        }
    }
}