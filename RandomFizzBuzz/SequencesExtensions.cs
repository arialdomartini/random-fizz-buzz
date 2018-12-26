﻿using System;
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

        private static readonly Random Random = new Random(322584);
        
        public static IEnumerable<string> MyFizzBuzz() =>
            Enumerable.Range(1, 10).Select( i => 
                new[] {"fizz", "buzz", "fizz buzz", i.ToString()}[Random.Next(4)]);
    }
}