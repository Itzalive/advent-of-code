using System.Text.RegularExpressions;

namespace AdventOfCode2022._2022._13;

public class Day13 : IDay
{
    public int Year => 2022;
    public int Day => 13;

    public string Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine + Environment.NewLine).ToArray();

        var pairs = inputs.Select(x => x.Split(Environment.NewLine)).ToArray();

        var score = 0;
        for (var i = 0; i < pairs.Length; i++)
        {
            var pair = pairs[i];
            var comparison = CompareInputs(pair[0], pair[1]);
            //Console.WriteLine(comparison == 0 ? "Same" : comparison < 0 ? "Left" : "Right");
            if (comparison < 0)
            {
                score += i + 1;
            }
        }

        return score.ToString();
    }

    private int CompareInputs(string left, string right)
    {
        if (!left.Contains("["))
        {
            left = $"[{left}]";
        }

        if (!right.Contains("["))
        {
            right = $"[{right}]";
        }

        const string pattern = @"\[(,?([0-9]+|\[(?>\[(?<c>)|[^\[\]]+|\](?<-c>))*(?(c)(?!))\]))*\]";
        var leftMatch = Regex.Match(left, pattern);
        var rightMatch = Regex.Match(right, pattern);

        var maxCapturesCount = Math.Max(leftMatch.Groups[2].Captures.Count, rightMatch.Groups[2].Captures.Count);
        for (var i = 0; i < maxCapturesCount; i++)
        {
            if (leftMatch.Groups[2].Captures.Count <= i) return -1;
            if (rightMatch.Groups[2].Captures.Count <= i) return 1;
            var leftValue = leftMatch.Groups[2].Captures[i].Value;
            var rightValue = rightMatch.Groups[2].Captures[i].Value;

            var diff = leftValue.Contains("[") || rightValue.Contains("[")
                ? CompareInputs(leftValue, rightValue)
                : int.Parse(leftValue) - int.Parse(rightValue);
            if (diff == 0) continue;
            return diff;
        }

        return 0;
    }

    public string Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
        inputs.Add("[[2]]");
        inputs.Add("[[6]]");
        inputs.Sort(CompareInputs);

        var score = (inputs.IndexOf("[[2]]") + 1) * (inputs.IndexOf("[[6]]") + 1);
        return score.ToString();
    }

    public string TestInput => @"[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]
";
}