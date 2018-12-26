using System;
using System.Collections.Generic;
using System.Linq;

namespace RandomFizzBuzz
{
    public static class SequencesExtensions
    {
        public static int MatchesUpToWith<T>(this IEnumerable<T> sequence, IEnumerable<T> reference) 
            where T : IEquatable<T> =>
            sequence.TakeWhile((item, index) => item.Equals(reference.ElementAt(index))).Count();

        public static Dictionary<int, int> Classify(this Dictionary<int, List<int>> collections, IEnumerable<int> reference)
        {
            return collections.Aggregate(new Dictionary<int, int>(), (acc, pair) =>
            {
                var (key, value) = pair;
                acc.Add(key, value.MatchesUpToWith(reference));
                return acc;
            });
        }

        public static IEnumerable<string> FizzBuzz()
        {
            var i = 0;
            while (true)
            {
                i++;
                if (i % 15 == 0)
                    yield return "fizz buzz";
                else if (i % 3 == 0)
                    yield return "fizz";
                else if (i % 5 == 0)
                    yield return "buzz";
                else yield return i.ToString();
            }
        }
    }
}