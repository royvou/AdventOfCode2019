namespace AdventOfCode.Solutions.Year2019
{
    internal class Day02 : ASolution
    {
        public Day02() : base(2, 2019, "")
        {
        }

        protected override string SolvePartOne()
        {
            var inputList = Input.ToIntArray(",");

            return SolvePart1With(inputList, 12, 2).ToString();
        }

        private static int SolvePart1With(int[] inputList, int noun, int verb)
        {
            inputList[1] = noun;
            inputList[2] = verb;

            for (var index = 0; index < inputList.Length - 4; index += 4)
            {
                var op = inputList[index];
                if (op == 99)
                {
                    break;
                }

                var in1 = inputList[inputList[index + 1]];
                var in2 = inputList[inputList[index + 2]];
                var @out = inputList[index + 3];

                inputList[@out] = op switch
                {
                    1 => in1 + in2,
                    2 => in1 * in2,
                    _ => 0
                };
            }
            return inputList[0];
        }

        protected override string SolvePartTwo()
        {
            var inputList = Input.ToIntArray(",");

            var inputListRun = new int[inputList.Length];

            var noun = 0;
            var verb = 0;
            var answer = 19690720;

            for (noun = 0; noun <= 99; noun++)
            {
                for (verb = 0; verb <= 99; verb++)
                {
                    //inputList.CopyTo(inputListRun, 0);
                    //var inputListRun = inputList.ToArray();
                    inputList.CopyTo(inputListRun, 0);

                    var result = SolvePart1With(inputListRun, noun, verb);
                    if (result == answer)
                    {
                        return $"{noun:00}{verb:00}";
                    }
                }
            }
            return null;
        }
    }
}
