using System.CommandLine;
using System.Diagnostics;
using System.Net;

namespace AdventOfCode2022;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var yearOption = new Option<int>(new[] {"--year", "-y"}, () => DateTime.Now.Year, "Year");
        var dayOption = new Option<int>(new[] {"--day", "-d"}, () => DateTime.Now.Day, "Day");
        var partOption = new Option<int?>(new[] {"--part", "-p"}, "Part");
        var rootCommand = new RootCommand("Advent of Code solver")
        {
            yearOption,
            dayOption,
            partOption
        };
        rootCommand.SetHandler(RunAdventOfCode, yearOption, dayOption, partOption);

        return await rootCommand.InvokeAsync(args);
    }

    private static async Task RunAdventOfCode(int year, int day, int? part)
    {
        var type = typeof(IDay);
        var days = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(t => type.IsAssignableFrom(t) && !t.IsAbstract);

        var daySolver = days.Select(d => (IDay) Activator.CreateInstance(d)!)
            .SingleOrDefault(d => d.Year == year && d.Day == day);

        Console.WriteLine($"Advent of Code {year} - Solving day {day}");
        if (daySolver == null)
        {
            Console.WriteLine("Nothing found... why are you running me already?!");
            return;
        }

        await EnsureInputLoadedAsync(daySolver);
        var input = await LoadInputAsync(daySolver);

        var timer = new Stopwatch();
        if (part is 1 or null)
        {
            if (!string.IsNullOrWhiteSpace(daySolver.TestInput))
            {
                Console.WriteLine("Part 1 Test");
                timer.Restart();
                var answerTest = daySolver.Part1(daySolver.TestInput);
                timer.Stop();
                Console.WriteLine($"Solution: {answerTest}");
                Console.WriteLine($"Time taken: {timer.Elapsed}");
                Console.WriteLine();
            }


            Console.WriteLine("Part 1");
            timer.Restart();
            var answer = daySolver.Part1(input);
            timer.Stop();
            Console.WriteLine($"Solution: {answer}");
            Console.WriteLine($"Time taken: {timer.Elapsed}");
        }

        if (part == null)
        {
            Console.WriteLine();
            Console.WriteLine();
        }

        if (part is 2 or null)
        {
            if (!string.IsNullOrWhiteSpace(daySolver.TestInput))
            {
                Console.WriteLine("Part 2 Test");
                timer.Restart();
                var answerTest = daySolver.Part2(daySolver.TestInput);
                timer.Stop();
                Console.WriteLine($"Solution: {answerTest}");
                Console.WriteLine($"Time taken: {timer.Elapsed}");
                Console.WriteLine();
            }

            Console.WriteLine("Part 2");
            timer.Restart();
            var answer = daySolver.Part2(input);
            timer.Stop();
            Console.WriteLine($"Solution: {answer}");
            Console.WriteLine($"Time taken: {timer.Elapsed}");
        }
    }

    private static async Task<string> LoadInputAsync(IDay day)
    {
        var inputPath = $"../../../{day.Year}/{day.Day:00}/input.txt";
        return (await File.ReadAllTextAsync(inputPath)).Replace("\n", Environment.NewLine)
            .Replace("\r\r\n", Environment.NewLine).Trim('\r', '\n', ' ');
    }

    private static async Task EnsureInputLoadedAsync(IDay day)
    {
        var inputPath = $"../../../{day.Year}/{day.Day:00}/input.txt";
        if (!File.Exists(inputPath))
        {
            var cookieContainer = new CookieContainer();
            var baseAddress = new Uri($"https://adventofcode.com");
            cookieContainer.Add(baseAddress,
                new Cookie("session",
                    ""));
            using var handler = new HttpClientHandler {CookieContainer = cookieContainer};
            using var client = new HttpClient(handler) {BaseAddress = baseAddress};
            var response = await client.GetAsync($"/{day.Year}/day/{day.Day}/input");
            await using var fs = new FileStream(inputPath, FileMode.CreateNew);
            await response.Content.CopyToAsync(fs);
        }
    }
}