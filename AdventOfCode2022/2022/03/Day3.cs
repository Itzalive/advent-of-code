namespace AdventOfCode2022._2022._03;

internal class Day3 : IDay
{
    public int Year => 2022;
    public int Day => 3;

    public string Part1(string input)
    {
        var rucksacks = input.Split(Environment.NewLine);

        var score = 0;
        foreach (var rucksack in rucksacks)
        {
            try
            {
                var left = rucksack[..(rucksack.Length / 2)];
                var right = rucksack[(rucksack.Length / 2)..];
                var leftChars = left.ToCharArray().GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
                var rightChars = right.ToCharArray().GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
                var duplicateChar = DuplicateChars(leftChars.Keys, rightChars.Keys).Single();
                score += ScoreChar(duplicateChar);
            }
            catch
            {
                Console.WriteLine(rucksack);
            }
        }

        return score.ToString();
    }

    public string Part2(string input)
    {
        var rucksacks = input.Split(Environment.NewLine);
        var score = 0;
        for (var i = 0; i < rucksacks.Length; i += 3)
        {
            try
            {
                var commonChar =
                    DuplicateChars(
                            DuplicateChars(rucksacks[i].ToCharArray().Distinct(),
                                rucksacks[i + 1].ToCharArray().Distinct()), rucksacks[i + 2].ToCharArray().Distinct())
                        .Single();
                score += ScoreChar(commonChar);
            }
            catch
            {
                Console.WriteLine(rucksacks[i]);
                Console.WriteLine(rucksacks[i + 1]);
                Console.WriteLine(rucksacks[i + 3]);
                Console.WriteLine();
            }
        }

        return score.ToString();
    }

    private static IEnumerable<char> DuplicateChars(IEnumerable<char> left, IEnumerable<char> right)
    {
        return left.Concat(right).GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToArray();
    }

    private static int ScoreChar(char value)
    {
        var intValue = (int) value;
        return intValue >= (int) 'a' ? intValue - (int) 'a' + 1 : intValue - (int) 'A' + 27;
    }

    public string? TestInput => null;
}