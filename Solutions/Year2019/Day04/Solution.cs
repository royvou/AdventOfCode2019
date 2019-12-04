using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Solutions.Year2019
{
    public class Day04 : ASolution
    {
        public Day04() : base(4, 2019, "")
        {
        }

        protected override string SolvePartOne()
        {
            var inputs = Input.Split('-');

            var startIndex = int.Parse(inputs[0]);
            var endIndex = int.Parse(inputs[1]);

            var result = CalculateNumbers(startIndex, endIndex);

            return result.Count().ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerable<int> CalculateNumbers(int startIndex, int endIndex)
        {
            var currentIndex = startIndex;
            while (currentIndex < endIndex)
            {
                if (IsValidNumber(currentIndex))
                    yield return currentIndex;
                currentIndex++;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsValidNumber(int currentIndex)
            => HasSameNumbers(currentIndex) &&
               HasIncreasingNumbers(currentIndex);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool HasIncreasingNumbers(in int currentIndex)
        {
            int currentIndexLocal = currentIndex;
            int lastNumber = 9;
            while (currentIndexLocal > 0)
            {
                var currentNumber = currentIndexLocal % 10;

                if (lastNumber < currentNumber)
                    return false;
                lastNumber = currentNumber;

                currentIndexLocal /= 10;
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool HasSameNumbers(in int currentIndex)
        {
            int currentIndexLocal = currentIndex;
            int lastNumber = -1;
            while (currentIndexLocal > 0)
            {
                var currentNumber = currentIndexLocal % 10;

                if (lastNumber == currentNumber)
                    return true;
                lastNumber = currentNumber;
                currentIndexLocal /= 10;
            }
            return false;
        }

        protected override string SolvePartTwo()
        {
            var inputs = Input.Split('-');

            var startIndex = int.Parse(inputs[0]);
            var endIndex = int.Parse(inputs[1]);

            var result = CalculateNumbers2(startIndex, endIndex);

            return result.Count().ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerable<int> CalculateNumbers2(int startIndex, int endIndex)
        {
            var currentIndex = startIndex;
            while (currentIndex < endIndex)
            {
                if (IsValidNumber2(currentIndex))
                    yield return currentIndex;
                currentIndex++;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsValidNumber2(in int currentIndex)
            => HasSameNumbers(currentIndex)
            && HasIncreasingNumbers(currentIndex)
            && HasAtleastGroupOfExactlyTwo(currentIndex);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool HasAtleastGroupOfExactlyTwo(int currentIndex) 
            => currentIndex.ToString().GroupBy(x => x).Any(x => x.Count() == 2);
    }
}
