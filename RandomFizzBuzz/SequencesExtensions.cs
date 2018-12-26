using System.Collections.Generic;
using System.Linq;

namespace RandomFizzBuzz
{
    public static class SequencesExtensions
    {
        public static int MatchesUpToWith(this IEnumerable<int> sequence, IEnumerable<int> reference) =>
            sequence.TakeWhile((item, index) => item == reference.ElementAt(index)).Count();

        public static Dictionary<int, int> Classify(this Dictionary<int, List<int>> collections, IEnumerable<int> reference)
        {
            return collections.Aggregate(new Dictionary<int, int>(), (acc, pair) =>
            {
                var (key, value) = pair;
                acc.Add(key, value.MatchesUpToWith(reference));
                return acc;
            });
        }
    }
}