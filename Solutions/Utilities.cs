/**
 * This utility class is largely based on:
 * https://github.com/jeroenheijmans/advent-of-code-2018/blob/master/AdventOfCode2018/Util.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Solutions
{
    public static class Utilities
    {
        public static int[] ToIntArray(this string str, string delimiter = "") => str
                .Split(delimiter)
                .Where(n => int.TryParse(n, out int v))
                .Select(n => Convert.ToInt32(n))
                .ToArray();

        public static int MinOfMany(params int[] items)
        {
            var result = items[0];
            for (int i = 1; i < items.Length; i++)
            {
                result = Math.Min(result, items[i]);
            }
            return result;
        }

        public static int MaxOfMany(params int[] items)
        {
            var result = items[0];
            for (int i = 1; i < items.Length; i++)
            {
                result = Math.Max(result, items[i]);
            }
            return result;
        }

        // https://stackoverflow.com/a/3150821/419956 by @RonWarholic
        public static IEnumerable<T> Flatten<T>(this T[,] map)
        {
            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    yield return map[row, col];
                }
            }
        }

        public static string JoinAsStrings<T>(this IEnumerable<T> items) 
            => string.Join("", items);

        public static string[] SplitByNewline(this string input, bool shouldTrim = false) => input
                .Split(new[] { "\r", "\n", "\r\n" }, StringSplitOptions.None)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => shouldTrim ? s.Trim() : s)
                .ToArray();

        public static int[] SplitByNewlineAsInt(this string input, bool shouldTrim = false) 
            => input.SplitByNewline(shouldTrim).Select(int.Parse).ToArray();

        public static T[] CloneAs<T>(this T[] t)
        {
            T[] result = (T[])Array.CreateInstance(typeof(T), t.Length);
            t.CopyTo(result,0);
            return result;
        }

        public static IEnumerable<T[]> GetPermutations<T>(this T[] items)
        {
            int countOfItem = items.Length;

            if (countOfItem <= 1)
            {
                yield break;
            }

            var indexes = new int[countOfItem];
            for (int i = 0; i < countOfItem; i++)
            {
                indexes[i] = 0;
            }

            yield return items;
         
            for (int i = 1; i < countOfItem;)
            {
                if (indexes[i] < i)
                { 
                    if ((i & 1) == 1) 
                    {
                        Swap(ref items[i], ref items[indexes[i]]);
                    }
                    else
                    {
                        Swap(ref items[i], ref items[0]);
                    }

                    yield return items;

                    indexes[i]++;
                    i = 1;
                }
                else
                {
                    indexes[i++] = 0;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
        //LINQ version
        //public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this ICollection<T> list)
        //    => GetPermutations(list, list.Count);
        //static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        //{
        //    if (length == 1) return list.Select(t => new T[] { t });

        //    return GetPermutations(list, length - 1)
        //        .SelectMany(t => list.Where(e => !t.Contains(e)),
        //            (t1, t2) => t1.Concat(new T[] { t2 }));
        //}
    }
}
