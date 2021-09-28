using UnityEngine;
namespace Fossil
{
    public class CopyTransformProperties : MonoBehaviour
    {
        public bool copyPosition = false;
        public Vector3 positionOffset;
        public bool copyRotation = false;
        public Quaternion rotationOffset;

        public Transform target;

        void LateUpdate()
        {
            CopyPosition();
        }

        public void CopyPosition()
        {
            if (target)
            {
                if (copyPosition)
                {
                    transform.position = target.position + positionOffset;
                }
                if (copyRotation)
                {
                    transform.rotation = target.rotation * rotationOffset;
                }
            }
        }
    }
}