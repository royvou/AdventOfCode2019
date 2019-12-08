using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2019
{
    public class Day08 : ASolution
    {
        public Day08() : base(8, 2019, "")
        {
        }

        protected override string SolvePartOne()
        {
            var boards = GetBoard(Input.TrimEnd(), 25, 6).ToList();
            return boards
                .Select((x, y) => (Count: GetCountOf(x, 0), Board: x, BoardIndex: y))
                .OrderBy(x => x.Count)
                .Select(x => GetCountOf(x.Board, 1) * GetCountOf(x.Board, 2))
                .First()
                .ToString();
        }

        private IEnumerable<IList<IList<int>>> GetBoard(string input, int chunk1, int chunk2)
            => input.Select(x => (int)char.GetNumericValue(x)).Chunk(chunk1).Chunk(chunk2);

        private static int GetCountOf(IEnumerable<IEnumerable<int>> input, int number) => input.SelectMany(z => z).Count(z => z == number);

        protected override string SolvePartTwo()
        {
            const int BOARDWIDTH = 25;
            const int BOARDHEIGHT = 6;
            //var img = new int[BOARDWIDTH, BOARDHEIGHT];
            var img = new int[BOARDHEIGHT][];

            var boards = GetBoard(Input.TrimEnd(), 25, 6).ToList();

            for (var y = 0; y < BOARDHEIGHT; y++)
            {
                img[y] = new int[BOARDWIDTH];
                for (var x = 0; x < BOARDWIDTH; x++)
                {
                    //img[i, j] = GetPixelColorAt(boards, i, j);
                    img[y][x] = GetPixelColorAt(boards, x, y);
                }
            }

            var sb = new StringBuilder();

            sb.AppendLine();
            for (var j = 0; j < BOARDHEIGHT; j++)
            {
                for (var i = 0; i < BOARDWIDTH; i++)
                {
                    sb.Append(img[j][i] == 1 ? "#" : " ");
                }
                sb.AppendLine();

            }

            return sb.ToString();
        }

        private int GetPixelColorAt(List<IList<IList<int>>> boards, int x, int y)
        {
            foreach (var board in boards)
            {
                switch (board[y][x])
                {
                    case 0:
                        return 0;
                    case 1:
                        return 1;
                    default:
                        continue;
                }
            }
            return -1;
        }
    }
}
