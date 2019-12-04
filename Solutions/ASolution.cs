using System;
using System.IO;
using System.Net;
using System.Net.Http;

namespace AdventOfCode.Solutions
{
    public abstract class ASolution
    {
        private readonly Lazy<string> _input, _part1, _part2;

        public int Day { get; }
        public int Year { get; }
        public string Title { get; }
        public string Input => string.IsNullOrEmpty(_input.Value) ? null : _input.Value;
        public string Part1 => string.IsNullOrEmpty(_part1.Value) ? "" : _part1.Value;
        public string Part2 => string.IsNullOrEmpty(_part2.Value) ? "" : _part2.Value;

        private protected ASolution(int day, int year, string title)
        {
            Day = day;
            Year = year;
            Title = title;
            _input = new Lazy<string>(() => LoadInput());
            _part1 = new Lazy<string>(() => SolvePartOne());
            _part2 = new Lazy<string>(() => SolvePartTwo());
        }

        public void Solve(int part = 0)
        {
            if (Input == null)
            {
                return;
            }

            var doOutput = false;
            var output = $"--- Day {Day}: {Title} --- \n";

            if (part != 2)
            {
                if (Part1 != "")
                {
                    output += $"Part 1: {Part1}\n";
                    doOutput = true;
                }
                else
                {
                    output += "Part 1: Unsolved\n";
                    if (part == 1)
                    {
                        doOutput = true;
                    }
                }
            }
            if (part != 1)
            {
                if (Part2 != "")
                {
                    output += $"Part 2: {Part2}\n";
                    doOutput = true;
                }
                else
                {
                    output += "Part 2: Unsolved\n";
                    if (part == 2)
                    {
                        doOutput = true;
                    }
                }
            }

            if (doOutput)
            {
                Console.WriteLine(output);
            }
        }

        private static readonly HttpClient httpClient = new HttpClient();

        private string LoadInput()
        {
            var INPUT_FILEPATH = $"./Solutions/Year{Year}/Day{Day.ToString("D2")}/input";
            var INPUT_URL = $"https://adventofcode.com/{Year}/day/{Day}/input";
            var input = "";

            if (File.Exists(INPUT_FILEPATH))
            {
                input = File.ReadAllText(INPUT_FILEPATH);
            }
            else if (DateTime.Now >= new DateTime(Year, 12, Day))
            {
                try
                {
                    using var message = new HttpRequestMessage(HttpMethod.Get, INPUT_URL);
                    message.Headers.Add("Cookie", Program.Config.Cookie);
                    var response = httpClient.SendAsync(message).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        input = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        var fileInfo = new FileInfo(INPUT_FILEPATH);
                        if (!fileInfo.Directory.Exists)
                            fileInfo.Directory.Create();
                        File.WriteAllText(INPUT_FILEPATH, input);
                    }
                    else
                    {
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.BadRequest:
                                Console.WriteLine($"Day {Day}: Error code 400 when attempting to retrieve puzzle input through the web client. Your session cookie is probably not recognized.");
                                break;

                            case HttpStatusCode.NotFound:
                                Console.WriteLine($"Day {Day}: Error code 404 when attempting to retrieve puzzle input through the web client. The puzzle is probably not available yet.");
                                break;

                            default:
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            return input;
        }

        protected abstract string SolvePartOne();

        protected abstract string SolvePartTwo();
    }
}
