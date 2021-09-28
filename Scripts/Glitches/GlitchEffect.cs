using UnityEngine;
namespace Fossil
{
    public class GlitchEffect : MonoBehaviour
    {

        public Material material;

        public Texture glitchImage;
        public Texture glitchImage2;
        public float effectStrength;

        float glitchStrength;
        Vector4 offsetVector;
        double nextGlitchTime = 0;

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            material.SetTexture("_glitchImage", glitchImage);
            material.SetTexture("_glitchImage2", glitchImage2);
            material.SetFloat("_glitchStrength", glitchStrength);
            material.SetVector("_OffsetScale", offsetVector);

            Graphics.Blit(source, destination, material);

            if (Time.time > nextGlitchTime)
            {
                float randValue = (Random.value - 0.5f) * 2.0f * effectStrength;
                nextGlitchTime = Time.time + (1 - Mathf.Abs(randValue)) * 1;
                glitchStrength = randValue * 10;
                offsetVector.x = Random.value;
                offsetVector.y = Random.value;
                offsetVector.z = Random.value;
                offsetVector.w = Random.value;
            }
        }
    }
}