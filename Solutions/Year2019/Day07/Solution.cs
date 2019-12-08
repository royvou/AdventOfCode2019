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

        //protected override string SolvePartOne()
        //{
        //    var amps = new[] { "A", "B", "C", "D", "E" };

        //    var testInput1 = "3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0".ToIntArray(",");
        //    var phase1 = new[] { 4, 3, 2, 1, 0 };

        //    var testInput2 = "3,23,3,24,1002,24,10,24,1002,23,-1,23, 101,5,23,23,1,24,23,23,4,23,99,0,0".ToIntArray(","); ;
        //    var phase2 = new[] { 0, 1, 2, 3, 4 };

        //    var testInput3 = "3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0".ToIntArray(",");
        //    var phase3 = new int[] { 1, 0, 4, 3, 2 };

        //    var input = Input.ToIntArray(",");
        //    //var input = testInput3;
        //    var phaseSolutions = Enumerable.Range(0, 5).ToArray().GetPermutations().Select(x => x.ToArray()).ToList();//GetPhases(5, amps.Length).ToList();           
        //    //var phaseSolutions = new[] { phase3 };
        //    var results = GetSolutionsOne(input, amps, phaseSolutions).ToList();

        //    var result = results.OrderByDescending(x => x.Perf).First().Perf;
        //    return string.Join("", result);
        //}
        protected override string SolvePartOne()
        {
            var amps = Enumerable.Range(0, 5).Select(x => new IntCodeMachine()).ToList();//new[] { "A", "B", "C", "D", "E" };

            var input = Input.ToIntArray(",");
            var phaseSolutions = Enumerable.Range(0, 5).ToArray().GetPermutations().ToList();//GetPhases(5, amps.Length).ToList();        

            for (var i = 1; i < amps.Count; i++)
            {
                amps[i].Input = amps[i - 1].Output;
            }

            var results = GetSolutionsTwo(input, amps, phaseSolutions).ToList();

            var result = results.OrderByDescending(x => x.Perf).First().Perf;
            return string.Join("", result);
        }

        protected override string SolvePartTwo()
        {
            var amps = Enumerable.Range(0, 5).Select(x => new IntCodeMachine()).ToList();//new[] { "A", "B", "C", "D", "E" };

            //var input = "3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5".ToIntArray(",");
            //var phase1 = new[] { 9, 8, 7, 6, 5 };

            //var input = "3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,- 5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10".ToIntArray(",");
            //var phase1 = new[] { 9, 7, 8, 5, 6 };
//            var phaseSolutions = new[] { phase1 };

            var input = Input.ToIntArray(",");
            var phaseSolutions = Enumerable.Range(5, 5).ToArray().GetPermutations().ToList();//GetPhases(5, amps.Length).ToList();        

            for (var i = 1; i < amps.Count; i++)
            {
                amps[i].Input = amps[i - 1].Output;
            }
            amps[0].Input = amps[amps.Count - 1].Output;

            var results = GetSolutionsTwo(input, amps, phaseSolutions).ToList();

            var result = results.OrderByDescending(x => x.Perf).First().Perf;
            return string.Join("", result);
        }

        private IEnumerable<(int[] Phase, int Perf)> GetSolutionsOne(int[] input, string[] amps, IEnumerable<int[]> phases)
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

        private IEnumerable<(IList<int> Phase, int Perf)> GetSolutionsTwo(int[] input, IList<IntCodeMachine> amps, IEnumerable<IList<int>> phases)
        {
            foreach (var phase in phases)
            {
                for (var i = 0; i < amps.Count; i++)
                {
                    var amp = amps[i];
                    amp.Index = 0;
                    amp.Input.Clear();
                    amp.Output.Clear();
                    amp.Memory = input.CloneAs();
                }

                for (var i = 0; i < amps.Count; i++)
                {
                    var amp = amps[i];
                    amp.Input.Enqueue(phase[i]);
                }
                amps[0].Input.Enqueue(0);


                var keepRunning = true;
                while (keepRunning)
                {
                    keepRunning = false;
                    foreach (var amp in amps)
                    {
                        keepRunning |= amp.Solve();
                    }
                }


                yield return (phase, amps[amps.Count - 1].Output.Peek());
            }
        }

        private bool SolvePart1With(int[] memory, Queue<int> input, Queue<int> output)
        {
            for (var index = 0; index < memory.Length;)
            {
                //Only last 2 digits are OpCode
                var inst = memory[index];
                var op = (inst % 100);

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

        public class IntCodeMachine
        {
            public IntCodeMachine()
            {
                Input = new Queue<int>();
                Output = new Queue<int>();
            }

            public Queue<int> Input { get; set; }
            public Queue<int> Output { get; set; }

            public int[] Memory { get; set; }
            public int Index { get; set; } = 0;
            public bool Solve()
            {
                //for (; ;)
               // {
                    //Only last 2 digits are OpCode
                    var inst = Memory[Index];
                    var op = (inst % 100);

                    Func<int, int> argPos = (int i) =>
                        Memory[Memory[Index + i]];

                    Func<int, int> argDir = (int i) =>
                        Memory[Index + i];

                    Func<int, ParameterMode> getMode = (int i) =>
                       (Memory[Index] / (int)Math.Pow(10, i + 1) % 10) == 0 ? ParameterMode.Position : ParameterMode.Immediate;

                    Func<int, int> pos = (int i) =>
                        getMode(i) == ParameterMode.Immediate
                        ? argDir(i)
                        : argPos(i);

                    switch (op)
                    {
                        case 1:
                            Memory[Memory[Index + 3]] = pos(1) + pos(2);
                            Index += 4;
                            break;
                        case 2:
                            Memory[Memory[Index + 3]] = pos(1) * pos(2);
                            Index += 4;
                            break;

                        case 3:
                            if (Input.Count > 0)
                            {
                                Memory[Memory[Index + 1]] = Input.Dequeue();
                                Index += 2;
                            }
                            return true;

                            break;

                        case 4:
                            var result = pos(1);
                            Output.Enqueue(result);
                            Index += 2;
                            break;

                        case 5:
                            Index = pos(1) != 0 ? pos(2) : Index + 3;
                            break;
                        case 6:
                            Index = pos(1) == 0 ? pos(2) : Index + 3;
                            break;
                        case 7:
                            Memory[Memory[Index + 3]] = pos(1) < pos(2) ? 1 : 0;
                            Index += 4;
                            break;
                        case 8:
                            Memory[Memory[Index + 3]] = pos(1) == pos(2) ? 1 : 0;
                            Index += 4;
                            break;
                        case 99:
                            return false;
                        default:
                            throw new InvalidOperationException();
                    }
                //}
                return true;
            }

        }
    }
}
