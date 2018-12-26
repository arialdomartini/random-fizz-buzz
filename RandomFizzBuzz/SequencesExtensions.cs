using System.Collections.Generic;
using System.Linq;

namespace RandomFizzBuzz
{
    public static class SequencesExtensions
    {
        public static int MatchesUpToWith(this IEnumerable<int> sequence, IEnumerable<int> reference) =>
            sequence.TakeWhile((item, index) => item == reference.ElementAt(index)).Count();
    }
}