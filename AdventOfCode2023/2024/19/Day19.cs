using System.Linq;

namespace AdventOfCode2023._2024._19;

public class Day19 : IDay
{
    public int Year => 2024;

    public int Day => 19;

    public string? Part1TestSolution => "6";

    public async Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine + Environment.NewLine).ToArray();
        var towels = inputs[0].Split(", ").OrderByDescending(s => s.Length).ToArray();
        var patterns = inputs[1].Split(Environment.NewLine);
        var manyTowels = new HashSet<string>();
        var threeTowels = new HashSet<string>();
        foreach (var t1 in towels)
        {
            if (patterns.Any(p => p.Contains(t1)))
            {
                manyTowels.Add(t1);
                foreach (var t2 in towels)
                {
                    var twoWay = t1 + t2;
                    if (patterns.Any(p => p.Contains(twoWay)) && manyTowels.Add(twoWay))
                    {
                        foreach (var t3 in towels)
                        {
                            var threeWay = twoWay + t3;
                            if (patterns.Any(p => p.Contains(threeWay)) && manyTowels.Add(threeWay))
                            {
                                threeTowels.Add(threeWay);
                                //foreach (var t4 in towels)
                                //{
                                //    var fourWay = threeWay + t4;
                                //    if (patterns.Any(p => p.Contains(fourWay)) && manyTowels.Add(fourWay))
                                //    {
                                //        //foreach (var t5 in towels)
                                //        //{
                                //        //    var fiveWay = fourWay + t5;
                                //        //    if (patterns.Any(p => p.Contains(fiveWay)) && manyTowels.Add(fiveWay))
                                //        //    {
                                //        //        foreach (var t6 in towels)
                                //        //        {
                                //        //            var sixWay = fiveWay + t6;
                                //        //            if (patterns.Any(p => p.Contains(sixWay)) && manyTowels.Add(sixWay))
                                //        //            {
                                //        //                foreach (var t7 in towels)
                                //        //                {
                                //        //                    var sevenWay = sixWay + t7;
                                //        //                    if (patterns.Any(p => p.Contains(sevenWay)) && manyTowels.Add(sevenWay))
                                //        //                    {//manyTowels.Add(sevenWay);
                                //        //                    }
                                //        //                }
                                //        //            }
                                //        //        }
                                //        //    }
                                //        //}
                                //    }
                                //}
                            }
                        }
                    }
                }
            }
            Console.WriteLine(manyTowels.Count);
        }


        var score = 0;
        Console.WriteLine(score);
        var scoreLock = new object();
        var oneAndTwoTowels = manyTowels.Except(threeTowels).ToArray();
        await Parallel.ForEachAsync(patterns, (pattern, _) =>
        {
            var matchingThreeTowels = threeTowels.Where(pattern.Contains).OrderByDescending(s => s.Length).ToArray();
            var maxThreeTowelLength = matchingThreeTowels.Any() ? matchingThreeTowels.Select(s => s.Length).Max() : 9999999;
            if (MatchPattern(pattern.AsSpan(),
                    oneAndTwoTowels.Where(pattern.Contains).OrderByDescending(t => t.Length).ToArray(), matchingThreeTowels, maxThreeTowelLength))
            {
                lock (scoreLock)
                {
                    score++;
                }
            }
            Console.WriteLine(score);
            return ValueTask.CompletedTask;
        });

        return await Task.FromResult(score.ToString());
    }

    private bool MatchPattern(ReadOnlySpan<char> s, string[] allTowels, string[] threeTowels, int maxTowel)
    {
        if (s.Length == 0) return true;

//        var remainingString = s.ToString();
  //      var remainingTowels = towels.Where(remainingString.Contains).ToArray();
        var towelsToSearch = s.Length < maxTowel ? allTowels : threeTowels;
        foreach (var towel in towelsToSearch)
        {
            if (s.StartsWith(towel) && MatchPattern(s[towel.Length..], allTowels, threeTowels, maxTowel))
                return true;
        }
        return false;
    }

    public string? Part2TestSolution => "16";


    public async Task<string> Part2(string input)
    {

        var inputs = input.Split(Environment.NewLine + Environment.NewLine).ToArray();
        var towels = inputs[0].Split(", ").OrderBy(s => s.Length).ToArray();
        var patterns = inputs[1].Split(Environment.NewLine);
        var manyTowels = new Dictionary<string, (int Combos, bool IsThreeTowels)>();
        foreach (var t1 in towels)
        {
            if (patterns.Any(p => p.Contains(t1)))
            {
                if (!manyTowels.TryAdd(t1, (1, false)))
                    manyTowels[t1] = (manyTowels[t1].Combos + 1, manyTowels[t1].IsThreeTowels);
            }

            Console.WriteLine(manyTowels.Count);
        }

        foreach (var t1 in towels)
        {
            
        }


        var score = 0;
        Console.WriteLine(score);
        var scoreLock = new object();
        var oneAndTwoTowels = manyTowels.Where(t => !t.Value.IsThreeTowels).ToArray();
        var threeTowels = manyTowels.Where(t => t.Value.IsThreeTowels).ToArray();
        await Parallel.ForEachAsync(patterns, (pattern, _) =>
        {
            //var patternCombos = 1;
            //var matchingThreeTowels = threeTowels.Where(p => pattern.Contains(p.Key))
            //    .OrderByDescending(s => s.Key.Length).ToArray();
            //var maxThreeTowelLength = matchingThreeTowels.Any()
            //    ? matchingThreeTowels.Select(s => s.Key.Length).Max()
            //    : 9999999;
            //if (MatchPattern2(pattern.AsSpan(),
            //        oneAndTwoTowels.Where(p => pattern.Contains(p.Key)).OrderByDescending(t => t.Key.Length)
            //            .ToArray(), matchingThreeTowels, maxThreeTowelLength))
            //{
            //    lock (scoreLock)
            //    {
            //        score++;
            //    }
            //}
            //Console.WriteLine(score);
            return ValueTask.CompletedTask;
        });

        return await Task.FromResult(score.ToString());
    }



    private bool MatchPattern2(ReadOnlySpan<char> s, KeyValuePair<string, (int Combos, bool IsThreeTowels)>[] allTowels, KeyValuePair<string, (int Combos, bool IsThreeTowels)>[] threeTowels, int maxTowel, out int combos)
    {
        if (s.Length == 0)
        {
            combos = 1;
            return true;
        }

        //        var remainingString = s.ToString();
        //      var remainingTowels = towels.Where(remainingString.Contains).ToArray();
        var towelsToSearch = s.Length < maxTowel ? allTowels : threeTowels;
        foreach (var towel in towelsToSearch)
        {
            if (s.StartsWith(towel.Key) && MatchPattern2(s[towel.Key.Length..], allTowels, threeTowels, maxTowel,
                    out var innerCombos))
            {
                combos = innerCombos * towel.Value.Combos;
                return true;
            }
        }

        combos = 0;
        return false;
    }


    public string? TestInput => @"r, wr, b, g, bwu, rb, gb, br

brwrr
bggr
gbbr
rrbgbr
ubwu
bwurrg
brgr
bbrgwb";
}