using UnityEngine;
namespace Fossil
{
    public abstract class Transition : MonoBehaviour
    {
        public abstract Coroutine StartTransition();

        public abstract Coroutine EndTransition();
    }
}