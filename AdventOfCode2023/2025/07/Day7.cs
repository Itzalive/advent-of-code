namespace AdventOfCode2023._2025._07;

internal class Day7 : IDay
{
    public int Year => 2025;

    public int Day => 7;

    public string? Part1TestSolution => "21";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        var timesSplit = 0;
        for (var lineNum = 1; lineNum < inputs.Length; lineNum++)
        {
            for (var colNum = 0; colNum < inputs[0].Length; colNum++)
            {
                if (inputs[lineNum][colNum] == '^' && inputs[lineNum - 1][colNum] == '|')
                {
                    inputs[lineNum][colNum - 1] = '|';
                    inputs[lineNum][colNum + 1] = '|';
                    timesSplit++;
                }
                else if (inputs[lineNum - 1][colNum] == 'S')
                {
                    inputs[lineNum][colNum] = '|';
                }
                else if (inputs[lineNum - 1][colNum] == '|')
                {
                    inputs[lineNum][colNum] = '|';
                }
            }
        }

        return Task.FromResult(timesSplit.ToString());
    }

    public string? Part2TestSolution => "40";

    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.ToCharArray().Select(c => c.ToString()).ToArray()).ToArray();
        for (var lineNum = 1; lineNum < inputs.Length; lineNum++)
        {
            for (var colNum = 0; colNum < inputs[0].Length; colNum++)
            {
                if (inputs[lineNum][colNum] == "^" && !new[] { "S", ".", "^" }.Contains(inputs[lineNum - 1][colNum]))
                {
                    var above = long.Parse(inputs[lineNum - 1][colNum]);
                    inputs[lineNum][colNum - 1] = long.TryParse(inputs[lineNum][colNum - 1], out var left)
                        ? (left + above).ToString()
                        : above.ToString();
                    inputs[lineNum][colNum + 1] = long.TryParse(inputs[lineNum][colNum + 1], out var right)
                        ? (right + above).ToString()
                        : above.ToString();
                }
                else if (inputs[lineNum - 1][colNum] == "S")
                {
                    inputs[lineNum][colNum] = "1";
                }
                else if (!new[] { "S", ".", "^" }.Contains(inputs[lineNum - 1][colNum]))
                {
                    inputs[lineNum][colNum] = long.TryParse(inputs[lineNum][colNum], out var already) ? (already + long.Parse(inputs[lineNum - 1][colNum])).ToString() : inputs[lineNum - 1][colNum];
                }
            }
        }

        Console.WriteLine(string.Join(Environment.NewLine, inputs.Select(l => string.Join("", l))));


        return Task.FromResult(inputs[^1].Where(i => long.TryParse(i, out _)).Select(long.Parse).Sum().ToString());
    }

    public string? TestInput => ".......S.......\r\n...............\r\n.......^.......\r\n...............\r\n......^.^......\r\n...............\r\n.....^.^.^.....\r\n...............\r\n....^.^...^....\r\n...............\r\n...^.^...^.^...\r\n...............\r\n..^...^.....^..\r\n...............\r\n.^.^.^.^.^...^.\r\n...............";


}