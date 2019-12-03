using System;
using System.Linq;

namespace AdventOfCode.Solutions.Year2019
{
    public class Day01 : ASolution
	{
		public Day01() : base(1, 2019, "")
		{
		}

		protected override string SolvePartOne()
			=> Input.SplitByNewlineAsInt().Select(CalculatePartOne).Sum().ToString();

		private int CalculatePartOne(int arg1)
			=> (arg1 / 3) - 2;

		protected override string SolvePartTwo()
			=> Input.SplitByNewlineAsInt().Select(CalculatePartTwo).Sum().ToString();

		private int CalculatePartTwo(int arg1)
		{
			var part1 = Math.Max(CalculatePartOne(arg1), 0);
			if (part1 > 0)
			{
				part1 += CalculatePartTwo(part1);
			}

			return part1;
		}
	}
}
