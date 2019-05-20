using System;
using System.Collections.Generic;

namespace Sesim.Helpers.Gameplay
{
    /// <summary>
    /// A weighted random picker. Not designed for multiple use.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WeightedRandomPicker<T>
    {
        public WeightedRandomPicker()
        {
            candidates = new List<T>();
            weights = new List<float>();
            totalWeight = 0;
        }

        public WeightedRandomPicker(IList<T> candidates, IList<float> weights)
        {
            this.candidates = candidates;
            this.weights = weights;
            totalWeight = 0;
            foreach (var w in weights) totalWeight += w;
        }

        IList<T> candidates;
        IList<float> weights;
        double totalWeight;

        public void AssignCandidate(T candidate, float weight)
        {
            candidates.Add(candidate);
            weights.Add(weight);
            totalWeight += weight;
        }

        public T Pick()
        {
            if (candidates.Count != weights.Count)
                throw new MissingMemberException($"Candidate count {candidates.Count} is not equal to weight count {weights.Count}. Abort.");
            var random = new Random();
            var picked = random.NextDouble() * totalWeight;
            int pickedIndex = -1;
            double partial = 0;
            for (int i = 0; i < weights.Count; i++)
            {
                var w = weights[i];
                partial += w;
                if (partial > picked)
                {
                    pickedIndex = i;
                    break;
                }
            }
            return candidates[pickedIndex];
        }
    }
}
