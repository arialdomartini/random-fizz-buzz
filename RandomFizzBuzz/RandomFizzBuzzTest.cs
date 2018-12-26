using System.Collections.Generic;
using FluentAssertions;
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

    }
}