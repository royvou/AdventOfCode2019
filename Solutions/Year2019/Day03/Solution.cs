using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Solutions.Year2019
{
    public class Day03 : ASolution
    {
        public Day03() : base(3, 2019, "")
        {
        }

        protected override string SolvePartOne()
        {            
            var points = Input.SplitByNewline();

            var line1 = CalculatePoints(points[0]);
            var line2 = CalculatePoints(points[1]);
            return line1.Keys
                .Where( x=> x.X != 0 && x.Y != 0)
                .Intersect(line2.Keys)
                .Select(cor => (Manhattan(cor.X, cor.Y), cor))                
                .OrderBy(cor => cor.Item1)
                .Select(x => x.Item1)
                .FirstOrDefault()
                .ToString();
        }


        protected override string SolvePartTwo()
        {
            var points = Input.SplitByNewline();

            var line1 = CalculatePoints(points[0]);
            var line2 = CalculatePoints(points[1]);
            return line1.Keys
                .Where(x => x.X != 0 && x.Y != 0)
                .Intersect(line2.Keys)
                .Select(x => (line1[x] + line2[x], x))
                .OrderBy(x => x.Item1)
                .Select(x => x.Item1)
                .FirstOrDefault()
                .ToString();

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int Manhattan(int x, int y) 
            => Math.Abs(x) + Math.Abs(y);

        private IDictionary<(int X, int  Y), int> CalculatePoints(string input)
        {

            var parsedInput =  input.Split(",");
            var result = new Dictionary<(int X, int Y), int>();

            int x = 0, y = 0, steps = 0;

            result.Add((x, y),0);

            for (int i = 0; i < parsedInput.Length; i++)
            {
                var currentStep = parsedInput[i];
                var stepType = currentStep[0];
                var stepAmount = int.Parse(currentStep.Substring(1));

                for (int j = 0; j < stepAmount; j++)
                {
                    var pos = stepType switch
                    {
                        'R' => (x++, y),
                        'U' => (x, y++),
                        'L' => (x--, y),
                        'D' => (x, y--),
                        _ => throw new InvalidOperationException()
                    };

                    result.TryAdd(pos, steps++);
                }
            }
            return result;
        }
    }
}
