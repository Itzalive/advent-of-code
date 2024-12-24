using System.CommandLine;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode2023;

public class Program
{
    private static readonly HttpClientHandler HttpHandler;
    private static readonly HttpClient HttpClient;

    static Program()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile("appsettings.Development.json", true, true)
            .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
        var configuration = builder.Build();
        var cookieContainer = new CookieContainer();
        var baseAddress = new Uri($"https://adventofcode.com");
        cookieContainer.Add(baseAddress,
            new Cookie("session", configuration["aocSessionToken"]));
        HttpHandler = new HttpClientHandler();
        HttpHandler.CookieContainer = cookieContainer;
        HttpClient = new HttpClient(HttpHandler) { BaseAddress = baseAddress };
    }

    public static async Task<int> Main(string[] args)
    {
        var yearOption = new Option<int>(new[] { "--year", "-y" }, () => DateTime.Now.Year, "Year");
        var dayOption = new Option<int>(new[] { "--day", "-d" }, () => DateTime.Now.Day, "Day");
        var partOption = new Option<int?>(new[] { "--part", "-p" }, "Part");
        var rootCommand = new RootCommand("Advent of Code solver")
        {
            yearOption,
            dayOption,
            partOption
        };
        rootCommand.SetHandler(RunAdventOfCode, yearOption, dayOption, partOption);

        var result = await rootCommand.InvokeAsync(args);

        OutputRuntimeInformation();

        return result;
    }

    private static async Task RunAdventOfCode(int year, int day, int? part)
    {
        var type = typeof(IDay);
        var days = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(t => type.IsAssignableFrom(t) && !t.IsAbstract);

        var daySolver = days.Select(d => (IDay)Activator.CreateInstance(d)!)
            .SingleOrDefault(d => d.Year == year && d.Day == day);

        Console.WriteLine($"Advent of Code {year} - Solving day {day}");
        if (daySolver == null)
        {
            Console.WriteLine("Nothing found... why are you running me already?!");
            return;
        }

        part ??= await GetCurrentPart(daySolver);

        await EnsureInputLoadedAsync(daySolver);
        var input = await LoadInputAsync(daySolver);

        var timer = new Stopwatch();
        if (part is 1 or null)
        {
            var canSubmit = false;
            if (!string.IsNullOrWhiteSpace(daySolver.TestInput))
            {
                Console.WriteLine("Part 1 Test");
                timer.Restart();
                var answerTest = await daySolver.Part1(daySolver.TestInput);
                timer.Stop();
                Console.WriteLine($"Solution: {answerTest}");
                if (daySolver.Part1TestSolution != null)
                {
                    if (daySolver.Part1TestSolution == answerTest)
                    {
                        Console.WriteLine($"CORRECT");
                        canSubmit = true;
                    }
                    else
                    {
                        Console.WriteLine($"Try again! Aim for {daySolver.Part1TestSolution}");
                    }
                }

                Console.WriteLine($"Time taken: {timer.Elapsed}");
                Console.WriteLine();
            }

            Console.WriteLine("Part 1");
            timer.Restart();
            var answer = await daySolver.Part1(input);
            timer.Stop();
            Console.WriteLine($"Solution: {answer}");
            Console.WriteLine($"Time taken: {timer.Elapsed}");
            if (canSubmit && part == 1)
            {
                await SubmitAnswerAsync(daySolver, part.Value, answer);
            }
        }

        if (part == null)
        {
            Console.WriteLine();
            Console.WriteLine();
        }

        if (part is 2 or null)
        {
            var canSubmit = false;
            if (!string.IsNullOrWhiteSpace(daySolver.TestInput))
            {
                Console.WriteLine("Part 2 Test");
                timer.Restart();
                var answerTest = await daySolver.Part2(daySolver.TestInput);
                timer.Stop();
                Console.WriteLine($"Solution: {answerTest}");
                if (daySolver.Part2TestSolution != null)
                {
                    if (daySolver.Part2TestSolution == answerTest)
                    {
                        Console.WriteLine($"CORRECT");
                        canSubmit = true;
                    }
                    else
                    {
                        Console.WriteLine($"Try again! Aim for {daySolver.Part2TestSolution}");
                    }
                }

                Console.WriteLine($"Time taken: {timer.Elapsed}");
                Console.WriteLine();
            }

            Console.WriteLine("Part 2");
            timer.Restart();
            var answer = await daySolver.Part2(input);
            timer.Stop();
            Console.WriteLine($"Solution: {answer}");
            Console.WriteLine($"Time taken: {timer.Elapsed}");
            if (canSubmit && part == 2)
            {
                await SubmitAnswerAsync(daySolver, part.Value, answer);
            }
        }
    }

    private static async Task<string> LoadInputAsync(IDay day)
    {
        Console.WriteLine($"Downloading input for {day.Year}-{day.Day:00}");
        var inputPath = $"../../../{day.Year}/{day.Day:00}/input.txt";
        return (await File.ReadAllTextAsync(inputPath)).Replace("\n", Environment.NewLine)
            .Replace("\r\r\n", Environment.NewLine).Trim('\r', '\n', ' ');
    }

    private static async Task EnsureInputLoadedAsync(IDay day)
    {
        var inputPath = $"../../../{day.Year}/{day.Day:00}/input.txt";
        if (!File.Exists(inputPath))
        {
            var response = await HttpClient.GetAsync($"/{day.Year}/day/{day.Day}/input");
            await using var fs = new FileStream(inputPath, FileMode.CreateNew);
            await response.Content.CopyToAsync(fs);
        }
    }

    private static async Task SubmitAnswerAsync(IDay day, int part, string answer)
    {
        var wrongAnswerPath = $"../../../{day.Year}/{day.Day:00}/{part}-bad.txt";
        if (File.Exists(wrongAnswerPath))
        {
            var previousAnswers = (await File.ReadAllTextAsync(wrongAnswerPath)).Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            if (previousAnswers.Contains(answer))
            {
                Console.WriteLine("Already tried this answer and it was not correct!");
                return;
            }
        }

        var formContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("level", part.ToString()), 
            new KeyValuePair<string, string>("answer", answer) 
        });
        var response = await HttpClient.PostAsync($"/{day.Year}/day/{day.Day}/answer", formContent);
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("Error on submission:" + response.StatusCode);
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync();
            var rightAnswer = Regex.Match(content, "That's the right answer!");
            if (rightAnswer.Success)
            {
                Console.WriteLine("Yay! You got a star");
                return;
            }

            var notRightAnswer = Regex.Match(content, "That's not the right answer");
            if (notRightAnswer.Success)
            {
                Console.WriteLine("Not the right answer");
                await File.AppendAllTextAsync(wrongAnswerPath, answer + Environment.NewLine);
                return;
            }

            var waitForSubmission = Regex.Match(content, "You have (([0-9]*)m )?([0-9]*)s left to wait");
            if (waitForSubmission.Success)
            {
                var mins = waitForSubmission.Groups[1].Success ? int.Parse(waitForSubmission.Groups[2].Value) : 0;
                var seconds = int.Parse(waitForSubmission.Groups[3].Value);
                Console.WriteLine($"Waiting to submit for {mins * 60 + seconds}s");
                await Task.Delay(TimeSpan.FromSeconds(mins * 60 + seconds + 1));
                await SubmitAnswerAsync(day, part, answer);
                return;
            }

            // not sure the outcome...
            Console.WriteLine(content);
        }
    }

    private static async Task<int?> GetCurrentPart(IDay day)
    {
        var response = await HttpClient.GetAsync($"/{day.Year}/day/{day.Day}");
        var content = await response.Content.ReadAsStringAsync();
        var hasHiddenLevel = Regex.Match(content, "<input type=\"hidden\" name=\"level\" value=\"(1|2)\"/>");
        if (hasHiddenLevel.Success)
        {
            return int.Parse(hasHiddenLevel.Groups[1].Value);
        }

        return null;
    }

    private static void OutputRuntimeInformation()
    {
        Console.WriteLine();
        Console.WriteLine(".Net: {0}", Environment.Version.ToString());
        var searcher = new ManagementObjectSearcher();
        searcher.Query = new SelectQuery("SELECT * FROM Win32_Processor");
        foreach (var mObject in searcher.Get())
        {
            Console.WriteLine("CPU: {0}", mObject["Name"]);
        }

        searcher.Query = new SelectQuery("Select * From Win32_ComputerSystem");
        foreach (var mObject in searcher.Get())
        {
            var ramBytes = (Convert.ToDouble(mObject["TotalPhysicalMemory"]));
            Console.WriteLine("RAM Size in Bytes: {0}", ramBytes);
            Console.WriteLine("RAM Size in Giga Bytes: {0}", ramBytes / 1073741824);
        }
    }
}