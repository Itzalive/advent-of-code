namespace AdventOfCode2023._2024._11;

public class Day11 : IDay
{
    public int Year => 2024;

    public int Day => 11;

    public string? Part1TestSolution => "55312";

    public Task<string> Part1(string input)
    {
        var stones = input.Split(" ").Select(long.Parse).GroupBy(s => s)
            .Select(stone => (Stone: stone.Key, Count: (long)stone.Count())).ToArray();

        for (var i = 0; i < 25; i++)
        {
            var newStones = new List<(long Stone, long Count)>();
            foreach (var stone in stones)
            {
                if (stone.Stone == 0)
                {
                    newStones.Add((1, stone.Count));
                }
                else if (stone.Stone.ToString().Length % 2 == 0)
                {
                    var newLength = stone.Stone.ToString().Length / 2;
                    var leftValue = (long)Math.Truncate(stone.Stone / Math.Pow(10, newLength));
                    newStones.Add((leftValue, stone.Count));
                    newStones.Add((stone.Stone - leftValue * (long)Math.Pow(10, newLength), stone.Count));
                }
                else
                {
                    newStones.Add((stone.Stone * 2024, stone.Count));
                }
            }

            stones = newStones.GroupBy(s => s.Stone)
                .Select(stone => (Stone: stone.Key, Count: stone.Sum(s => s.Count))).ToArray();
        }

        return Task.FromResult(stones.Sum(c => c.Count).ToString());
    }

    public string? Part2TestSolution => null;


    public Task<string> Part2(string input)
    {
        var stones = input.Split(" ").Select(long.Parse).GroupBy(s => s)
            .Select(stone => (Stone: stone.Key, Count: (long)stone.Count())).ToArray();

        for (var i = 0; i < 75; i++)
        {
            var newStones = new List<(long Stone, long Count)>();
            foreach (var stone in stones)
            {
                if (stone.Stone == 0)
                {
                    newStones.Add((1, stone.Count));
                }
                else if (stone.Stone.ToString().Length % 2 == 0)
                {
                    var newLength = stone.Stone.ToString().Length / 2;
                    var leftValue = (long)Math.Truncate(stone.Stone / Math.Pow(10, newLength));
                    newStones.Add((leftValue, stone.Count));
                    newStones.Add((stone.Stone - leftValue * (long)Math.Pow(10, newLength), stone.Count));
                }
                else
                {
                    newStones.Add((stone.Stone * 2024, stone.Count));
                }
            }

            stones = newStones.GroupBy(s => s.Stone)
                .Select(stone => (Stone: stone.Key, Count: stone.Sum(s => s.Count))).ToArray();
        }

        return Task.FromResult(stones.Sum(c => c.Count).ToString());
    }

    public string? TestInput => "125 17";
}