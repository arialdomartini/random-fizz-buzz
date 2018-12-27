using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

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

            var sequences = new Dictionary<int, IEnumerable<int>>
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
        
        [Fact]
        public void should_generate_fizz_buzz_sequence()
        {
            var result = SequencesExtensions.ReferenceFizzBuzz().Take(16);

            result.Should().BeEquivalentTo(new List<string>
            {
                "1", "2", "fizz", "4", "buzz", "fizz", "7", "8", "fizz", "buzz",
                "11", "fizz", "13", "14", "fizz buzz", "16"
            }, option => option.WithStrictOrdering());
        }

        [Fact]
        public void should_generate_arbitrary_long_fizz_buzz_sequences()
        {
            var result = SequencesExtensions.ReferenceFizzBuzz().Take(1000);

            result.Count().Should().Be(1000);
        }

        [Fact]
        public void should_generate_random_fizz_buzz_sequence()
        {
            var result = SequencesExtensions.FizzBuzzWithSeed(1).Take(16).ToList();

            result.Should().BeEquivalentTo(new List<string>
            {
                "fizz", "fizz", "buzz", "4", "fizz buzz", "buzz", "buzz", "8", "fizz", "fizz buzz",
                "fizz", "fizz", "buzz", "14", "fizz buzz", "fizz buzz"
            }, option => option.WithStrictOrdering());
        }

        [Fact(Skip = "This consumes a lot of memory: use should_iteratively_detect_best_seed")]
        public void should_detect_best_seed()
        {
            var step = 1_000_000;
            var k = 60;
            var enumerable = Enumerable.Range(322584 + k * step, step);

            var keyValuePairs =
                enumerable.Select(i =>
                (i, SequencesExtensions.FizzBuzzWithSeed(i)));

            var dictionary =
                keyValuePairs.ToDictionary(a => a.Item1, a => a.Item2);
            var results = dictionary.Classify(SequencesExtensions.ReferenceFizzBuzz());


            var bestResult = results.OrderByDescending(a => a.Value).Take(1).Single();

            bestResult.Value.Should().Be(12);
            bestResult.Key.Should().Be(60502432);
        }

        [Fact]
        public void should_work_up_to_11_values()
        {
            var result = SequencesExtensions.FizzBuzz();

            result.Should().BeEquivalentTo(new List<string>
            {
                "1", "2", "fizz", "4", "buzz", "fizz", "7", "8", "fizz", "buzz", "11"
            }, option => option.WithStrictOrdering());
        }

        [Fact]
        public void should_iteratively_detect_best_seed()
        {
            var record = 0;
            for (var i = 0; i < int.MaxValue; i++)
            {
                var sequence = SequencesExtensions.FizzBuzzWithSeed(i);

                var length = sequence.MatchesUpToWith(SequencesExtensions.ReferenceFizzBuzz());
                if (length > record)
                {
                    record = length;
                    var index = i;
                    var text = $"New record! {index} generated a length of {record}\n";
                    File.AppendAllText("/tmp/fizz-buzz.log", text);
                }
            }
        }

        [Fact]
        public void should_get_permutations()
        {
            var list = new[] {"fizz", "buzz", "fizz buzz", "100"};

            var result = SequencesExtensions.GetPermutations(list);

            result.Count().Should().Be(24);
        }

        [Fact]
        public void should_iteratively_detect_best_seed_considering_permutations()
        {
            var record = 0;
            for (var i = 0; i < int.MaxValue; i++)
            {
                for (var permutation = 0; permutation < 24; permutation++)
                {
                    var sequence = SequencesExtensions.FizzBuzzWithSeed(i, permutation);

                    var length = sequence.MatchesUpToWith(SequencesExtensions.ReferenceFizzBuzz());
                    if (length > record)
                    {
                        record = length;
                        var index = i;
                        Log($"New record! {index} with permutation {permutation} generated a length of {record}\n");
                    }

                }
            }
        }

        private static void Log(string text)
        {
            File.AppendAllText("/tmp/fizz-buzz.log",
                text);
        }
    }
}