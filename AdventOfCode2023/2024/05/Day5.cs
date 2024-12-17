namespace AdventOfCode2023._2024._05;

public class Day5 : IDay
{
    public int Year => 2024;

    public int Day => 5;

    public string? Part1TestSolution => "143";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine + Environment.NewLine).ToArray();
        var pageRules = inputs[0].Split(Environment.NewLine).Select(l => l.Split("|").Select(int.Parse).ToArray())
            .ToArray();
        var printers = inputs[1].Split(Environment.NewLine).Select(l => l.Split(",").Select(int.Parse).ToArray())
            .ToArray();

        var answer = 0;
        foreach (var printer in printers)
        {
            var breaksRule = false;
            foreach (var rule in pageRules)
            {
                var firstValueIndex = Array.IndexOf(printer, rule[0]);
                var secondValueIndex = Array.IndexOf(printer, rule[1]);
                if (firstValueIndex >=0 && secondValueIndex >= 0 && firstValueIndex > secondValueIndex)
                {
                    breaksRule = true;
                    break;
                }
            }

            if (breaksRule) continue;

            answer += printer[(printer.Length - 1) / 2];
        }

        return Task.FromResult(answer.ToString());
    }

    public string? Part2TestSolution => "123";


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine + Environment.NewLine).ToArray();
        var pageRules = inputs[0].Split(Environment.NewLine).Select(l => l.Split("|").Select(int.Parse).ToArray())
            .ToArray();
        var printers = inputs[1].Split(Environment.NewLine).Select(l => l.Split(",").Select(int.Parse).ToList())
            .ToArray();

        var answer = 0;
        foreach (var printer in printers)
        {
            var breaksAnyRule = false;
            var breaksRule = false;
            do
            {
                breaksRule = false;
                foreach (var rule in pageRules)
                {
                    var firstValueIndex = printer.IndexOf(rule[0]);
                    var secondValueIndex = printer.IndexOf(rule[1]);
                    if (firstValueIndex >= 0 && secondValueIndex >= 0 && firstValueIndex > secondValueIndex)
                    {
                        breaksRule = true;
                        breaksAnyRule = true;
                        printer.RemoveAt(firstValueIndex);
                        printer.Insert(secondValueIndex, rule[0]);
                    }
                }
            } while (breaksRule);

            if (!breaksAnyRule) continue;

            Console.WriteLine(string.Join(",", printer));
            answer += printer[(printer.Count - 1) / 2];
        }

        return Task.FromResult(answer.ToString());
    }

    public string? TestInput => @"47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47";
}