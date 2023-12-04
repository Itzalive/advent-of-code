namespace AdventOfCode2023._2021._25;

public class Day25 : IDay
{
    public int Year => 2021;

    public int Day => 25;

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();

        if (inputs.Length < 20)
        {
            foreach (var line in inputs)
                Console.WriteLine(string.Join("", line));
            Console.WriteLine();
        }

        var i = 0;
        bool hasMoved;
        do
        {
            hasMoved = false;
            var nextRound = new char[inputs.Length][];
            for (var y = 0; y < inputs.Length; y++)
            {
                nextRound[y] = new char[inputs[y].Length];
            }
            for (var y = 0; y < inputs.Length; y++)
            {
                for (var x = 0; x < inputs[y].Length; x++)
                {
                    if (inputs[y][x] == '>' && inputs[y][(x + 1) % inputs[y].Length] == '.')
                    {
                        nextRound[y][x] = '.';
                        x++;
                        nextRound[y][x % inputs[y].Length] = '>';
                        hasMoved = true;
                    }
                    else
                    {
                        nextRound[y][x] = inputs[y][x];
                    }
                }
            }
            inputs = nextRound;

            nextRound = new char[inputs.Length][];
            for (var y = 0; y < inputs.Length; y++)
            {
                nextRound[y] = new char[inputs[y].Length];
            }
            for (var x = 0; x < inputs[0].Length; x++)
            {
                for (var y = 0; y < inputs.Length; y++)
                {
                    
                    if (inputs[y][x] == 'v' && inputs[(y + 1) % inputs.Length][x] == '.')
                    {
                        nextRound[y][x] = '.';
                        y++;
                        nextRound[y % inputs.Length][x] = 'v';
                        hasMoved = true;
                    }
                    else
                    {
                        nextRound[y][x] = inputs[y][x];
                    }
                }
            }

            inputs = nextRound;

            if (inputs.Length < 20)
            {
                foreach (var line in inputs)
                    Console.WriteLine(string.Join("", line));
                Console.WriteLine();
            }

            i++;
        } while (hasMoved);

        return Task.FromResult(i.ToString());
    }

    
    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        return Task.FromResult("");
    }

    public string? TestInput => @"v...>>.vv>
.vv>>.vv..
>>.>v>...v
>>v>>.>.v.
v>v.vv.v..
>.>>..v...
.vv..>.>v.
v.v..>>v.v
....v..v.>";
}