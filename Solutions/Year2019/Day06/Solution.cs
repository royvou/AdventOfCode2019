using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2019
{
    public class Day06 : ASolution
    {
        public Day06() : base(6, 2019, "")
        {
        }

        protected override string SolvePartOne()
        {
            var map = CreateMap(Input);

            return StartRecursive(map, "COM").ToString();

        }

        private Dictionary<string, IList<string>> CreateMap(string input)
        {
            var map = new Dictionary<string, IList<string>>();

            foreach (var line in input.SplitByNewline())
            {
                var io = line.IndexOf(")");
                var key = line.Substring(0, io);
                var value = line.Substring(io + 1);

                if (!map.ContainsKey(key))
                    map[key] = new List<string>();

                map[key].Add(value);
            }

            return map;
        }

        private int StartRecursive(Dictionary<string, IList<string>> map, string currentItem, int score = 0)
        {
            var returnScore = score;
            if (map.TryGetValue(currentItem, out var item) && item.Count > 0)
            {
                for (int i = 0; i < item.Count; i++)
                    returnScore += StartRecursive(map, item[i], score + 1);
            }

            return returnScore;
        }



        protected override string SolvePartTwo()
        {     
            var map = CreateMapTwo(Input);

            var par1 = GetParentsFor(map, "SAN").ToList();
            var par2 = GetParentsFor(map, "YOU").ToList();

            for(int i =0; i < par1.Count;i++)
                if (par2.Contains(par1[i]))
                {
                    return (i + par2.IndexOf(par1[i])).ToString();
                }

            return null;
        }

        private IEnumerable<string> GetParentsFor(Dictionary<string, IList<string>> map, string v)
        {
            if (map.ContainsKey(v))
            {
                foreach(var item in map[v])
                {
                    yield return item;

                    foreach (var item2 in GetParentsFor(map, item))
                        yield return item2;
                }
            }        
        }

        private Dictionary<string, IList<string>> CreateMapTwo(string input)
        {
            var map = new Dictionary<string, IList<string>>();

            foreach (var line in input.SplitByNewline())
            {
                var io = line.IndexOf(")");
                var key = line.Substring(0, io).Trim();
                var value = line.Substring(io + 1).Trim();
                if (!map.ContainsKey(value))
                    map[value] = new List<string>();

                map[value].Add(key);
            }

            return map;
        }
    }
}
