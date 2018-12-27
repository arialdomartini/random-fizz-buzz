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

        public static Dictionary<int, int> Classify<T>(
            this Dictionary<int, IEnumerable<T>> collections, IEnumerable<T> reference)
        where T : IEquatable<T>
        {
            return collections.Aggregate(new Dictionary<int, int>(), (acc, pair) =>
            {
                var (key, value) = pair;
                acc.Add(key, value.MatchesUpToWith(reference));
                return acc;
            });
        }

        public static IEnumerable<string> ReferenceFizzBuzz()
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

        public static IEnumerable<string> FizzBuzzWithSeed(int seed)
        {
            var random = new Random(seed);
            int i = 1;
            while (true)
            {
                var x = new[] {"fizz", "buzz", "fizz buzz", i++.ToString()};
                yield return x[random.Next(4)];
            }
        }

        public static IEnumerable<string> FizzBuzzWithSeed(int seed, int permutation)
        {
            var random = new Random(seed);
            int i = 1;
            while (true)
            {
                var x = new[] {"fizz", "buzz", "fizz buzz", i++.ToString()};
                var y = GetPermutations(x).ToList()[permutation].ToList();
                yield return y[random.Next(4)];
            }
        }

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IList<T> list) =>
            GetPermutations(list, list.Count);

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IList<T> list, int length)
        {
            if (length == 1) return list.Select(t => new[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new[] { t2 }));
        }

        private static readonly Random Random = new Random(40043843);

        public static IEnumerable<string> FizzBuzz() =>
            Enumerable.Range(1, 14).Select( i =>
                new[] {"fizz", "buzz", "fizz buzz", i.ToString()}[Random.Next(4)]);
    }
}