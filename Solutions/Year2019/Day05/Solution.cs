using System;
using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2019
{
    public class Day05 : ASolution
    {
        public Day05() : base(5, 2019, "")
        {
        }

        protected override string SolvePartOne()
        {
            var inputList = Input.ToIntArray(",");

            return SolvePart1With(inputList ,1).ToString();
        }

        protected override string SolvePartTwo()
        {
            var inputList = Input.ToIntArray(",");

            return SolvePart1With(inputList, 5).ToString();
        }

        private static int SolvePart1With(int[] memory, int input)
        {
            int currentInput = input;        

            for (var index = 0; index < memory.Length;)
            {
                //Only last 2 digits are OpCode
                var inst = memory[index];
                var op = (inst % 100 );
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
                       memory[ memory[index + 3]] = pos(1) + pos(2);
                        instructionsDone = 4;
                        break;
                    case 2:
                       memory[ memory[index + 3]] = pos(1) * pos(2); 
                        instructionsDone = 4;
                        break;

                    case 3:
                      memory[memory[index + 1]] = currentInput;
                        instructionsDone = 2;
                        break;

                    case 4:
                        var result = pos(1);
                        if (result != 0)
                            return result;

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
                        goto result;
                        break;
                }
              
                index += instructionsDone;
            }
result:
            return memory[0];
        }

        public enum ParameterMode
        {
            Position,
            Immediate
        }
    }
}
