using System.Collections;
using UnityEngine;
namespace Fossil
{
    public class NullTransition : Transition
    {
        public override Coroutine EndTransition()
        {
            return StartCoroutine(NullRoutine());
        }

        public override Coroutine StartTransition()
        {
            return StartCoroutine(NullRoutine());
        }

        IEnumerator NullRoutine()
        {
            yield return null;
        }
    }
}