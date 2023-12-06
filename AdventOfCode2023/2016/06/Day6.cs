namespace AdventOfCode2023._2016._06;

public class Day6 : IDay
{
    public int Year => 2016;

    public int Day => 6;

    public string? Part1TestSolution => "easter";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        var output = "";
        for (var i = 0; i < inputs[0].Length; i++)
        {
            var charCount = new Dictionary<char, int>();
            for (var j = 0; j < inputs.Length; j++)
            {
                if (charCount.ContainsKey(inputs[j][i]))
                {
                    charCount[inputs[j][i]]++;
                }
                else
                {
                    charCount.Add(inputs[j][i], 1);
                }
            }

            output += charCount.MaxBy(kvp => kvp.Value).Key;
        }
        return Task.FromResult(output);
    }

    public string? Part2TestSolution => "advent";


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        var output = "";
        for (var i = 0; i < inputs[0].Length; i++)
        {
            var charCount = new Dictionary<char, int>();
            for (var j = 0; j < inputs.Length; j++)
            {
                if (charCount.ContainsKey(inputs[j][i]))
                {
                    charCount[inputs[j][i]]++;
                }
                else
                {
                    charCount.Add(inputs[j][i], 1);
                }
            }

            output += charCount.MinBy(kvp => kvp.Value).Key;
        }
        return Task.FromResult(output);
    }

    public string? TestInput => @"eedadn
drvtee
eandsr
raavrd
atevrs
tsrnev
sdttsa
rasrtv
nssdts
ntnada
svetve
tesnvt
vntsnd
vrdear
dvrsen
enarar";
}