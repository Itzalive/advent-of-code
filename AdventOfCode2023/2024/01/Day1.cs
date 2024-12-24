namespace AdventOfCode2023._2024._01;

internal class Day2 : IDay
{
    public int Year => 2024;

    public int Day => 1;

    public string? Part1TestSolution => "11";

    public Task<string> Part1(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var lists = lines.Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray();
        var leftList = lists.Select((l, i) => (i, int.Parse(l[0]))).OrderBy(l => l.Item2).ToArray();
        var rightList = lists.Select((l, i) => (i, int.Parse(l[1]))).OrderBy(l => l.Item2).ToArray();
        var diff = 0;
        var leftListLookup = leftList.GroupBy(l => l.Item2, l => l.i).ToDictionary(l => l.Key, l=> l.ToArray());
        var rightListLookup = rightList.GroupBy(l => l.Item2, l => l.i).ToDictionary(l => l.Key, l => l.ToArray());
        for (var i = 0; i < leftList.Length; i++)
        {
            var leftListOptions = leftListLookup[leftList[i].Item2];
            var rightListOptions = rightListLookup[rightList[i].Item2];
            var minDiff = leftListOptions.SelectMany(l => rightListOptions.Select(r => Math.Abs(r - l))).Min();
            Console.WriteLine($"L:{leftList[i].Item2} R:{rightList[i].Item2} D:{minDiff}");
            diff += Math.Abs(leftList[i].Item2 - rightList[i].Item2);
        }
        return Task.FromResult(diff.ToString());
    }

    public string? Part2TestSolution => "31";

    public Task<string> Part2(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var lists = lines.Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray();
        var leftList = lists.Select(l => int.Parse(l[0])).ToArray();
        var rightList = lists.Select(l => int.Parse(l[1])).GroupBy(r => r).ToLookup(r => r.Key);
        var diff = 0;
        foreach (var left in leftList)
        {
            Console.WriteLine($"L:{left} R:{rightList[left].FirstOrDefault()?.Count()}");
            diff += left * (rightList[left].FirstOrDefault()?.Count() ?? 0);
        }
        return Task.FromResult(diff.ToString());
    }

    public string? TestInput => "3   4\r\n4   3\r\n2   5\r\n1   3\r\n3   9\r\n3   3";


}