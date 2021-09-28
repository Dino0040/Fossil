using System.Collections.Generic;
using UnityEngine;
namespace Fossil
{
    public class WeightedRandomSelector<T>
    {
        readonly Dictionary<T, float> valuesAndWeights = new Dictionary<T, float>();
        double weightSum = 0;

        public void Add(T value, float weight)
        {
            valuesAndWeights.Add(value, weight);
            weightSum += weight;
        }

        public void Set(Dictionary<T, float> valuesAndWeights)
        {
            this.valuesAndWeights.Clear();
            foreach (KeyValuePair<T, float> keyValuePair in valuesAndWeights)
            {
                this.valuesAndWeights.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public bool Remove(T value)
        {
            if (valuesAndWeights.TryGetValue(value, out float weight))
            {
                weightSum -= weight;
            }
            return valuesAndWeights.Remove(value);
        }

        public void Clear()
        {
            valuesAndWeights.Clear();
            weightSum = 0;
        }

        public T Select()
        {
            if (valuesAndWeights.Count == 0)
            {
                return default;
            }
            double indicator = Random.value * weightSum;
            T lastElement = default;
            foreach (KeyValuePair<T, float> keyValuePair in valuesAndWeights)
            {
                lastElement = keyValuePair.Key;

                indicator -= keyValuePair.Value;
                if (indicator < 0)
                {
                    return lastElement;
                }
            }
            return lastElement;
        }
    }
}