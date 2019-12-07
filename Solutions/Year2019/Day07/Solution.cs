using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2019
{
    public class Day07 : ASolution
    {
        public Day07() : base(7, 2019, "")
        {
        }

        protected override string SolvePartOne()
        {
            var amps = new[] { "A", "B", "C", "D", "E" };

            var testInput1 = "3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0".ToIntArray(",");
            var result1 = 43210;
            var phase1 = new[] { 4, 3, 2, 1, 0};

            var testInput2 = "3,23,3,24,1002,24,10,24,1002,23,-1,23, 101,5,23,23,1,24,23,23,4,23,99,0,0".ToIntArray(","); ;
            var result2 = 54321;
            var phase2 = new[] { 0, 1, 2, 3, 4 };

            var testInput3 = "3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0".ToIntArray(",");
            var result3 = 65210;
            var phase3 = new int[] { 1, 0, 4, 3, 2 };

            var input = Input.ToIntArray(",");
            //var input = testInput3;
            var phaseSolutions =Enumerable.Range(0, 5).ToArray().GetPermutations().Select(x => x.ToArray()).ToList();//GetPhases(5, amps.Length).ToList();           
            //var phaseSolutions = new[] { phase3 };
            var results = GetSolutions(input, amps, phaseSolutions).ToList();

            var result = results.OrderByDescending(x => x.Perf).First().Perf;
            return string.Join("",result);
        }



        private IEnumerable<(int[] Phase, int Perf)> GetSolutions(int[] input, string[] amps, IEnumerable<int[]> phases)
        {
            foreach (var phase in phases)
            {
                var inp = 0;
                for (var i = 0; i < amps.Length; i++)
                {
                    var ip = new Queue<int>(new[] { phase[i], inp });
                    var op = new Queue<int>();
                    var r = SolvePart1With(input.CloneAs(), ip, op);
                    inp = op.Peek();
                }

                yield return (phase, inp);
            }
        }

        protected override string SolvePartTwo() => null;

        private bool SolvePart1With(int[] memory, Queue<int> input, Queue<int> output)
        {
            //int currentInput = input;

            for (var index = 0; index < memory.Length;)
            {
                //Only last 2 digits are OpCode
                var inst = memory[index];
                var op = (inst % 100);
                var opMod = inst / 100;

                Func<int, int> argPos = (int i) =>
                    memory[memory[index + i]];

                Func<int, int> argDir = (int i) =>
                    memory[index + i];

                Func<int, ParameterMode> getMode = (int i) =>
                   (memory[index] / (int)Math.Pow(10, i + 1) % 10) == 0 ? ParameterMode.Position : ParameterMode.Immediate;

                Func<int, int> pos = (int i) =>
                    getMode(i) == ParameterMode.Immediate
                    ? argDir(i)
                    : argPos(i);

                var instructionsDone = 0;
                switch (op)
                {
                    case 1:
                        memory[memory[index + 3]] = pos(1) + pos(2);
                        instructionsDone = 4;
                        break;
                    case 2:
                        memory[memory[index + 3]] = pos(1) * pos(2);
                        instructionsDone = 4;
                        break;

                    case 3:
                        memory[memory[index + 1]] = input.Dequeue();
                        instructionsDone = 2;
                        break;

                    case 4:
                        var result = pos(1);
                        output.Enqueue(result);                      
                        instructionsDone = 2;
                        break;

                    case 5:
                        index = pos(1) != 0 ? pos(2) : index + 3;
                        instructionsDone = 0;
                        break;
                    case 6:
                        index = pos(1) == 0 ? pos(2) : index + 3;
                        instructionsDone = 0;
                        break;
                    case 7:
                        memory[memory[index + 3]] = pos(1) < pos(2) ? 1 : 0;
                        instructionsDone = 4;

                        break;
                    case 8:
                        memory[memory[index + 3]] = pos(1) == pos(2) ? 1 : 0;
                        instructionsDone = 4;

                        break;
                    case 99:
                        return true;
                        break;
                }

                index += instructionsDone;
            }
            return true;
        }

        public enum ParameterMode
        {
            Position,
            Immediate
        }
    }
}
