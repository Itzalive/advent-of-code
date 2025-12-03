namespace AdventOfCode2023._2025._03;

internal class Day3 : IDay
{
    public int Year => 2025;

    public int Day => 3;

    public string? Part1TestSolution => "357";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
        var score = 0;
        var numBatteries = 2;
        foreach (var bank in inputs)
        {
            var joltage = 0;
            var lastBattery = -1;
            for (var i = 0; i < numBatteries; i++)
            {
                joltage *= 10;
                var battery = bank[(lastBattery + 1) .. (bank.Length - numBatteries + i)]
                    .Select((battery, index) => (Battery: battery, Index: index))
                    .GroupBy(x => x.Battery).MaxBy(x => x.Key).MinBy(x => x.Index);
                joltage += battery.Battery;
                lastBattery = battery.Index;
            }

            score += joltage;
        }

        return Task.FromResult(score.ToString());
    }

    public string? Part2TestSolution => "3121910778619";

    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
        var score = 0L;
        var numBatteries = 12;
        foreach (var bank in inputs)
        {
            var joltage = 0L;
            var lastBattery = -1;
            for (var i = 0; i < numBatteries; i++)
            {
                joltage *= 10;
                var battery = bank[(lastBattery + 1)..(bank.Length - numBatteries + i + 1)]
                    .Select((battery, index) => (Battery: battery, Index: index))
                    .GroupBy(x => x.Battery).MaxBy(x => x.Key).MinBy(x => x.Index);
                joltage += battery.Battery;
                lastBattery += battery.Index + 1;
            }

            score += joltage;
        }

        return Task.FromResult(score.ToString());
    }

    public string? TestInput => "987654321111111\r\n811111111111119\r\n234234234234278\r\n818181911112111";


}