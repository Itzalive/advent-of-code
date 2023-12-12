namespace AdventOfCode2023._2023._12;

public class Day12 : IDay
{
    public int Year => 2023;
    public int Day => 12;

    public string? Part1TestSolution => "21";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.Split(' ').ToArray()).ToArray();
        var score = inputs.Sum(line => CalcNumOptions(line[0],
            line[1].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()));
        return Task.FromResult(score.ToString());
    }

    private long CalcNumOptions(string s, int[] groupings)
    {
        var combinationsToSolve = new List<Combination>();
        var chars = s.ToCharArray();
        combinationsToSolve.Add(new Combination { Count = 1, Groupings = Array.Empty<int>(), CurrentRun = 0 });
        for (var i = 0; i < chars.Length; i++)
        {
            var newCombinations = new List<Combination>();
            if (chars[i] == '?' || chars[i] == '#')
            {
                foreach (var combination in combinationsToSolve)
                {
                    if (combination.Groupings.Length < groupings.Length &&
                        combination.CurrentRun + 1 <= groupings[combination.Groupings.Length])
                    {
                        newCombinations.Add(combination with { CurrentRun = combination.CurrentRun + 1 });
                    }
                }
            }

            if (chars[i] == '?' || chars[i] == '.')
            {
                foreach (var combination in combinationsToSolve)
                {
                    if (combination.CurrentRun > 0)
                    {
                        var newOffList = combination.Groupings.Concat(new[] { combination.CurrentRun }).ToArray();
                        if (newOffList.Length <= groupings.Length && newOffList[^1] == groupings[newOffList.Length - 1])
                        {
                            newCombinations.Add(new Combination
                                { Count = combination.Count, Groupings = newOffList, CurrentRun = 0 });
                        }
                    }
                    else
                    {
                        newCombinations.Add(combination);
                    }
                }
            }

            combinationsToSolve =
                newCombinations.GroupBy(c => c, c => c, new CombinationComparer()).Select(g =>
                    g.Count() == 1 ? g.Single() : g.Key with { Count = g.Sum(c => c.Count) }).ToList();
        }

        long combinations = 0;
        foreach (var combination in combinationsToSolve)
        {
            var combGroupings = combination.Groupings;
            if (combination.CurrentRun > 0)
            {
                combGroupings = combination.Groupings.Concat(new int[] { combination.CurrentRun }).ToArray();
            }

            if (combGroupings.Length == groupings.Length && combGroupings[^1] == groupings[combGroupings.Length - 1])
            {
                combinations += combination.Count;
            }
        }

        return combinations;
    }

    public record Combination
    {
        public long Count { get; set; }

        public int[] Groupings { get; set; }

        public int CurrentRun { get; set; }
    }

    public class CombinationComparer : IEqualityComparer<Combination>
    {
        public bool Equals(Combination x, Combination y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x.CurrentRun != y.CurrentRun) return false;
            if (x.Groupings.Length != y.Groupings.Length) return false;
            return !x.Groupings.Where((t, i) => t != y.Groupings[i]).Any();
        }

        public int GetHashCode(Combination obj)
        {
            return HashCode.Combine(obj.Groupings.Length, obj.Groupings.Aggregate(1, HashCode.Combine), obj.CurrentRun);
        }
    }

    public string? Part2TestSolution => "525152";


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.Split(' ').ToArray()).ToArray();
        long score = 0;
        object scoreLock = new object();
        foreach (var line in inputs)
        {
            var unfoldedInput = line[0] + "?" + line[0] + "?" + line[0] + "?" + line[0] + "?" + line[0];
            var groupings = line[1] + "," + line[1] + "," + line[1] + "," + line[1] + "," + line[1];
            var result = CalcNumOptions(unfoldedInput,
                groupings.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray());
            lock (scoreLock)
            {
                score += result;
            }
        }

        return Task.FromResult(score.ToString());
    }

    public string? TestInput => @"???.### 1,1,3
.??..??...?##. 1,1,3
?#?#?#?#?#?#?#? 1,3,1,6
????.#...#... 4,1,1
????.######..#####. 1,6,5
?###???????? 3,2,1";
}