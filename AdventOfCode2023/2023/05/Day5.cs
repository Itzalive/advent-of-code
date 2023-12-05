namespace AdventOfCode2023._2023._05;

internal class Day5 : IDay
{
    public int Year => 2023;

    public int Day => 5;

    public Task<string> Part1(string input)
    {
        var blocks = input.Split(Environment.NewLine + Environment.NewLine);

        var seeds = blocks[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse)
            .ToList();

        var maps = blocks[1..].Select(b => new Map(b)).ToList();

        var currentType = "seed";
        var currentNums = seeds;

        while (currentType != "location")
        {
            var map = maps.Single(m => m.From == currentType);
            var newNums = new List<long>();
            foreach (var num in currentNums)
            {
                var added = false;
                foreach (var range in map.Mapping)
                {
                    if (range.SourceStart <= num && num < range.SourceStart + range.Range)
                    {
                        newNums.Add(range.DestinationStart + num - range.SourceStart);
                        added = true;
                        break;
                    }
                }

                if (!added)
                {
                    newNums.Add(num);
                }
            }

            currentNums = newNums;
            currentType = map.To;
        }

        return Task.FromResult(currentNums.Min().ToString());
    }

    public Task<string> Part2(string input)
    {
        var blocks = input.Split(Environment.NewLine + Environment.NewLine);

        var seedRanges = blocks[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse)
            .ToList();

        var maps = blocks[1..].Select(b => new Map(b)).ToList();

        var minimum = long.MaxValue;
        var minimumCheck = new object();
        for (var i = 0; i < seedRanges.Count; i += 2)
        {
            Parallel.For(seedRanges[i], seedRanges[i] + seedRanges[i + 1], (num) =>
            {
                foreach (var map in maps)
                {
                    foreach (var range in map.Mapping.Where(range => range.SourceStart <= num && num <= range.SourceEnd))
                    {
                        num = range.DestinationShift + num;
                        break;
                    }
                }

                if (num >= minimum) return;
                lock (minimumCheck)
                {
                    if (num < minimum)
                    {
                        minimum = num;
                    }
                }
            });
        }

        return Task.FromResult(minimum.ToString());
    }

    private class Map
    {
        public string From { get; set; }

        public string To { get; set; }

        public List<RangeMap> Mapping { get; set; } = new List<RangeMap>();

        public Map(string s)
        {
            var lines = s.Split(Environment.NewLine);
            var mapping = lines[0].Split(' ')[0].Split('-');
            From = mapping[0];
            To = mapping[2];
            foreach (var line in lines[1..].Select(l =>
                         l.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()))
            {
                Mapping.Add(new RangeMap
                {
                    DestinationStart = line[0], SourceStart = line[1], Range = line[2],
                    SourceEnd = line[1] + line[2] - 1, DestinationShift = line[0] - line[1]
                });
            }
        }
    }

    private class RangeMap
    {
        public long SourceStart { get; set; }

        public long SourceEnd { get; set; }

        public long DestinationStart { get; set; }
        public long Range { get; set; }
        public long DestinationShift { get; set; }
    }

    public string TestInput => @"seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4";
}