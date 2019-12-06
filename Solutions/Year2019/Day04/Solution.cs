using System;
using System.Buffers;
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
            =>  HasIncreasingNumbers(currentIndex)
                && HasSameNumbers(currentIndex);
              

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
            => HasIncreasingNumbers(currentIndex)
            && HasSameNumbers(currentIndex)
            && HasAtleastGroupOfExactlyTwo(currentIndex);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool HasAtleastGroupOfExactlyTwoLINQ(int currentIndex) 
            => currentIndex.ToString().GroupBy(x => x).Any(x => x.Count() == 2);
        private bool HasAtleastGroupOfExactlyTwoLinqNative(int currentIndex)
        {
            var result = false;
            var array = new int[10];
            //var array = ArrayPool<int>.Shared.Rent(10); //Array 0-9 for each number count

            int currentIndexLocal = currentIndex;
            while (currentIndexLocal > 0)
            {
                var lastNumber = currentIndexLocal % 10;
                array[lastNumber]++;
                currentIndexLocal /= 10;
            }


            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 2)
                {
                    result = true;
                    break;
                }
            }

            //ArrayPool<int>.Shared.Return(array, true);

            return result;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool HasAtleastGroupOfExactlyTwo(int currentIndex)
        {
            int itemsInGroup = 1;
            int currentIndexLocal = currentIndex;
            int lastNumber = -1;
            while (currentIndexLocal > 0)
            {
                var currentNumber = currentIndexLocal % 10;


                if (lastNumber == currentNumber)
                {
                    itemsInGroup += 1;
                }
                else if (lastNumber != currentNumber
                    && itemsInGroup == 2)
                {
                    return true;
                }
                else
                {
                    itemsInGroup = 1;
                }

                lastNumber = currentNumber;
                currentIndexLocal /= 10;
            }
            return itemsInGroup == 2;
        }
    }
}
