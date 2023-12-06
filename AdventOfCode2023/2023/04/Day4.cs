namespace AdventOfCode2023._2023._04;

internal class Day4 : IDay
{
    public int Year => 2023;
    public int Day => 4;

    public async Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var cards = inputs.Select(l => new Card(l));
        var score = 0.0;
        foreach (var card in cards)
        {
            var winnersCount = card.Picks.Count(number => card.Winners.Contains(number));
            if (winnersCount > 0)
                score += Math.Pow(2, winnersCount - 1) ;
        }

        return score.ToString();
    }

    public string? Part1TestSolution => "13";

    public async Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var cards = inputs.Select(l => new Card(l)).ToArray();
        var score = 0.0;
        for (int i = 0; i < cards.Length; i++)
        {
            var winnersCount = cards[i].Picks.Count(number => cards[i].Winners.Contains(number));
            for (int j = i + 1; j <= i + winnersCount; j++)
            {
                cards[j].Copies += cards[i].Copies;
            }
            score += cards[i].Copies;
        }

        return score.ToString();
    }

    public string? Part2TestSolution => "30";

    public class Card
    {
        public Card(string line)
        {
            var card = line.Split(':');
            Number = int.Parse(card[0].Replace("Card", "").Trim());
            var numbers = card[1].Split("|");
            Winners = numbers[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            Picks = numbers[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        }

        public int Number { get; set; }

        public int[] Picks { get; set; }

        public int[] Winners { get; set; }

        public int Copies { get; set; } = 1;
    }

    public string TestInput => @"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11";
}