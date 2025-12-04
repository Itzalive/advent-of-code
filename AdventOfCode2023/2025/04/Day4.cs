namespace AdventOfCode2023._2025._04;

internal class Day4 : IDay
{
    public int Year => 2025;

    public int Day => 4;

    public string? Part1TestSolution => "13";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(c => c.Select(c => c.ToString()).ToArray()).ToArray();
        var numRolls = 0;
        for(var x =0; x < inputs.Length; x++)
        {
            for(var y = 0; y < inputs[x].Length; y++)
            {
                if(inputs[x][y] == "@")
                {
                    var countRolls = 0;
                    for (var xDiff = -1; xDiff <= 1; xDiff++)
                    {
                        for (var yDiff = -1; yDiff <= 1; yDiff++)
                        {
                            if (xDiff == 0 && yDiff == 0) continue;
                            var newX = x + xDiff;
                            var newY = y + yDiff;
                            if (newX < 0 || newX >= inputs.Length || newY < 0 || newY >= inputs[x].Length) continue;
                            if (inputs[newX][newY] == "@")
                            {
                                countRolls++;
                            }
                        }
                    }

                    if (countRolls < 4)
                    {
                        numRolls++;
                    }
                }
            }
        }
        return Task.FromResult(numRolls.ToString());
    }

    public string? Part2TestSolution => "43";

    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(c => c.Select(c => c.ToString()).ToArray()).ToArray();
        var numRolls = 0;
        var i = 0;
        var numRollsThisIteration = 0;
        do
        {
            i++;
            numRollsThisIteration = 0;
            for (var x = 0; x < inputs.Length; x++)
            {
                for (var y = 0; y < inputs[x].Length; y++)
                {
                    if (inputs[x][y] == "@")
                    {
                        var countRolls = 0;
                        for (var xDiff = -1; xDiff <= 1; xDiff++)
                        {
                            for (var yDiff = -1; yDiff <= 1; yDiff++)
                            {
                                if (xDiff == 0 && yDiff == 0) continue;
                                var newX = x + xDiff;
                                var newY = y + yDiff;
                                if (newX < 0 || newX >= inputs.Length || newY < 0 || newY >= inputs[x].Length) continue;
                                if (inputs[newX][newY] == "@" || inputs[newX][newY] == i.ToString())
                                {
                                    countRolls++;
                                }
                            }
                        }

                        if (countRolls < 4)
                        {
                            numRollsThisIteration++;
                            inputs[x][y] = i.ToString();
                        }
                    }
                }
            }

            numRolls += numRollsThisIteration;
        } while (numRollsThisIteration > 0);

        return Task.FromResult(numRolls.ToString());
    }

    public string? TestInput => "..@@.@@@@.\r\n@@@.@.@.@@\r\n@@@@@.@.@@\r\n@.@@@@..@.\r\n@@.@@@@.@@\r\n.@@@@@@@.@\r\n.@.@.@.@@@\r\n@.@@@.@@@@\r\n.@@@@@@@@.\r\n@.@.@@@.@.";


}