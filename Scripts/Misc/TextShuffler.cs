using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Fossil
{
    public class TextShuffler : MonoBehaviour
    {
        [ResizableTextArea]
        public string separator;
        [ResizableTextArea]
        public List<string> textFragments;
        public UnityEvent<string> setTextEvent;


        void Start()
        {
            List<int> fragmentsIndexLeft = new List<int>();
            for (int i = 0; i < textFragments.Count; i++)
            {
                fragmentsIndexLeft.Add(i);
            }
            string s = "";
            while (fragmentsIndexLeft.Count > 0)
            {
                int elementIndex = Random.Range(0, fragmentsIndexLeft.Count);
                int fragmentIndex = fragmentsIndexLeft[elementIndex];
                s += textFragments[fragmentIndex];
                fragmentsIndexLeft.Remove(fragmentIndex);
                if (fragmentsIndexLeft.Count > 0)
                {
                    s += "\n" + separator;
                }
            }
            setTextEvent.Invoke(s);
        }

        void Update()
        {

        }
    }
}