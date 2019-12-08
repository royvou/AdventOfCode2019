using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2019
{
    public class Day08 : ASolution
    {
        public Day08() : base(8, 2019, "")
        {
        }

        protected override string SolvePartOne()
        {                        
            var board = GetBoard(Input.TrimEnd(), 25, 6).ToList();
            return board
                .Select((x, y) => (Count: GetCountOf(x, 0), Board: x, BoardIndex: y))
                .OrderBy(x => x.Count)
                .Select(x => GetCountOf(x.Board, 1) * GetCountOf(x.Board, 2))
                .First()
                .ToString() ;           
        }

        private IEnumerable<IEnumerable<IEnumerable<int>>> GetBoard(string input, int chunk1, int chunk2) 
            => input.Select(x => (int)char.GetNumericValue(x)).Chunk(chunk1).Chunk(chunk2);

        private static int GetCountOf(IEnumerable<IEnumerable<int>> input, int number) => input.SelectMany(z => z).Count(z => z == number);

        protected override string SolvePartTwo() => null;
    }
}
