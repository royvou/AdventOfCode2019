using AdventOfCode.Solutions;

namespace AdventOfCode
{
    internal class Program
    {
        public static Config Config = Config.Get("config.json");
        private static SolutionCollector Solutions = new SolutionCollector(Config.Year, Config.Days);

        private static void Main(string[] args)
        {
            foreach (ASolution solution in Solutions)
            {
                solution.Solve();
            }
        }
    }
}
