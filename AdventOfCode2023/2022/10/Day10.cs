namespace AdventOfCode2023._2022._10;

public class Day10 : IDay
{
    public int Year => 2022;
    public int Day => 10;

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var cycle = 0;
        var xReg = 1;
        var score = 0;
        var nextInterestingCycle = 20;
        foreach (var line in inputs)
        {
            var values = line.Split(" ");
            var xDiff = 0;
            switch (values[0])
            {
                case "addx":
                    cycle += 2;
                    xDiff = int.Parse(values[1]);
                    break;
                case "noop":
                    cycle++;
                    break;
            }

            if (cycle >= nextInterestingCycle)
            {
                score += nextInterestingCycle * xReg;
                nextInterestingCycle += 40;
            }

            xReg += xDiff;
        }

        return Task.FromResult(score.ToString());
    }

    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var xReg = 1;
        var result = "";
        foreach (var line in inputs)
        {
            var values = line.Split(" ");
            var xDiff = 0;
            switch (values[0])
            {
                case "addx":
                    result = WritePixel(xReg, result);
                    result = WritePixel(xReg, result);
                    xReg += int.Parse(values[1]);
                    break;
                case "noop":
                    result =WritePixel(xReg, result);
                    break;
            }

            xReg += xDiff;
        }

        return Task.FromResult(result);
    }

    private static int position = 0;

    private static string WritePixel(int xReg, string output)
    {
        if (position >= xReg - 1 && position <= xReg + 1)
        {
            output += "#";
        }
        else
        {
            output += ".";
        }
        
        position++;
        if (position >= 40)
        {
            position = 0;
            output += Environment.NewLine;
        }

        return output;
    }

    public string TestInput => @"addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop";
}