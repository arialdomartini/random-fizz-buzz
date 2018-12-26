using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace RandomFizzBuzz
{
    public class RandomFizzBuzzTest
    {
        [Theory]
        [MemberData(nameof(Values))]
        public void should_detect_length_of_matching_sequence(IEnumerable<int> reference, IEnumerable<int> sequence, int matchLength)
        {
            var result = sequence.MatchesUpToWith(reference);

            result.Should().Be(matchLength);
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public static IEnumerable<object[]> Values()
        {
            return new List<object[]>
            {
                new object[]
                {
                    new List<int> {1, 2, 3, 4, 5, 6, 7, 8},
                    new List<int>(),
                    0
                },
                new object[]
                {
                    new List<int> {1, 2, 3, 4, 5, 6, 7, 8},
                    new List<int> {1000, 2, 3, 4, 5, 6, 7, 8},
                    0
                },
                new object[]
                {
                    new List<int> {1, 2, 3, 4, 5, 6, 7, 8},
                    new List<int> {1, 2, 3, 4, 5, 6, 7, 8},
                    8
                },
                new object[]
                {
                    new List<int> {1, 2, 3, 4, 5, 6, 7, 8},
                    new List<int> {1, 2, 3, 4, 100, 200, 300},
                    4
                }
            };
        }

        [Theory]
        [MemberData(nameof(OtherValues))]
        public void should_work_with_infinite_sequences(IEnumerable<int> sequence, int matchLength)
        {
            var reference = AllNumbers();

            var result = sequence.MatchesUpToWith(reference);

            result.Should().Be(matchLength);
        }

        public static IEnumerable<object[]> OtherValues()
        {
            return new List<object[]>
            {
                new object[]
                {
                    new List<int>{ 1, 2, 3},
                    3
                },
                new object[]
                {
                    new List<int>{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10},
                    10
                }
            };
        }

        private IEnumerable<int> AllNumbers()
        {
            var i = 1;
            while (true)
            {
                yield return i++;
            }
        }

        [Fact]
        public void should_identify_best_results()
        {
            var reference = AllNumbers();

            var sequences = new Dictionary<int, List<int>>
            {
                [1] = new List<int> { 1, 2, 3, 4, 5, 6},          // 6
                [2] = new List<int> { 100, 2, 3, 4, 5, 6},        // 0
                [3] = new List<int> { 1, 2, 300, 4, 5, 6},        // 2
                [4] = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8},    // 8
                [5] = new List<int> { 1, 2, 3, 4, 5}              // 5
            };

            var result = sequences.Classify(reference);

            result.Should().BeEquivalentTo(new Dictionary<int, int>
            {
                [1] = 6,
                [2] = 0,
                [3] = 2,
                [4] = 8,
                [5] = 5
            });
        }
    }
}