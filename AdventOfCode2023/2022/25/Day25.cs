namespace AdventOfCode2023._2022._25;

public class Day25 : IDay
{
    public int Year => 2022;

    public int Day => 25;

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();

        foreach (var i in inputs)
        {
            if (ToSnafu(FromSnafu(i)) != i)
            {
                Console.WriteLine($"{i} => {FromSnafu(i)} => {ToSnafu(FromSnafu(i))}");
            }
        }

        return Task.FromResult(ToSnafu(inputs.Select(FromSnafu).Sum()));
    }

    
    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        return Task.FromResult("");
    }

    public long FromSnafu(string snafu)
    {
        long value = 0;
        var snafuChars = snafu.ToCharArray().Reverse().ToArray();
        for(var i = 0; i < snafuChars.Length; i++)
        {
            value += (long) Math.Pow(5, i) * snafuChars[i] switch
            {
                '2' => 2,
                '1' => 1,
                '0' => 0,
                '-' => -1,
                '=' => -2,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        return value;
    }

    public string ToSnafu(long value)
    {
        var componentParts = new long[20];
        for(var n = 0; n < componentParts.Length; n++)
        {
            var placeValue = (long)Math.Pow(5, n);
            componentParts[n] += (value % (placeValue * 5)) / placeValue;
            if (componentParts[n] > 2)
                value += placeValue * 5;
            value -= componentParts[n] * placeValue;
        }

        return string.Join("", componentParts.Reverse()).Replace("4", "-").Replace("3", "=").TrimStart('0');
    }

    public string? TestInput => @"1=-0-2
12111
2=0=
21
2=01
111
20012
112
1=-1=
1-12
12
1=
122";
}