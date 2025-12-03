namespace AdventOfCode2023._2025._02;

internal class Day2 : IDay
{
    public int Year => 2025;

    public int Day => 2;

    public string? Part1TestSolution => "1227775554";

    public Task<string> Part1(string input)
    {
        var inputs = input.Replace(Environment.NewLine, "").Split(",").Select(i => i.Split("-").Select(long.Parse).ToArray()).ToArray();
        var count = 0L;
        foreach (var line in inputs)
        {
            for (var i = line[0]; i <= line[1]; i++)
            {
                var s = i.ToString();
                if (s.Length % 2 == 0)
                {
                    if (s[..(s.Length / 2)] == s[(s.Length / 2)..])
                    {
                        count += i;
                    }
                }
                else
                {
                    i = (long)Math.Pow(10L, s.Length);
                }
            }
        }

        return Task.FromResult(count.ToString());
    }

    public string? Part2TestSolution => "4174379265";

    public Task<string> Part2(string input)
    {
        var inputs = input.Replace(Environment.NewLine, "").Split(",").Select(i => i.Split("-").Select(long.Parse).ToArray()).ToArray();
       var invalidIds = new List<long>();
        foreach (var line in inputs)
        {
            for (var productId = line[0]; productId <= line[1]; productId++)
            {
                var added = false;
                var productIdString = productId.ToString();
                for (var n = 1; n <= productIdString.Length / 2; n++)
                {
                    if (productIdString.Length % n != 0) continue;
                    var template = productIdString[..n];
                    for(var m = n; m < productIdString.Length; m += n)
                    {
                        if (productIdString[m..(m + n)] != template) break;
                        if (m + n == productIdString.Length)
                        {
                            invalidIds.Add(productId);
                            added = true;
                            break;
                        }
                    }
                    if (added) break;
                }
            }
        }

        return Task.FromResult(invalidIds.Sum().ToString());
    }

    public string? TestInput => "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,\r\n1698522-1698528,446443-446449,38593856-38593862,565653-565659,\r\n824824821-824824827,2121212118-2121212124";


}