namespace AdventOfCode2023._2023._07;

public class Day7 : IDay
{
    public int Year => 2023;
    public int Day => 7;

    public string? Part1TestSolution => "6440";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.Split(' '))
            .Select(l => new Hand(l[0], l[1], cardValuePt1))
            .ToArray();
        var ordered = inputs.OrderBy(h => h.Scores[0]).ThenBy(h => h.Scores[1]).ThenBy(h => h.Scores[2])
            .ThenBy(h => h.Scores[3]).ThenBy(h => h.Scores[4]).ThenBy(h => h.Scores[5]).ToArray();
        long score = 0;
        for (var i = 0; i < ordered.Length; i++)
        {
            score += int.Parse(ordered[i].Bid) * (i + 1);
        }

        return Task.FromResult(score.ToString());
    }

    private static Dictionary<char, int> cardValuePt1 = new()
    {
        { 'A', 14 },
        { 'K', 13 },
        { 'Q', 12 },
        { 'J', 11 },
        { 'T', 10 },
        { '9', 9 },
        { '8', 8 },
        { '7', 7 },
        { '6', 6 },
        { '5', 5 },
        { '4', 4 },
        { '3', 3 },
        { '2', 2 }
    };

    public string? Part2TestSolution => "5905";


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.Split(' '))
            .Select(l => new Hand(l[0], l[1], cardValuePt2))
            .ToArray();
        var ordered = inputs.OrderBy(h => h.Scores[0]).ThenBy(h => h.Scores[1]).ThenBy(h => h.Scores[2])
            .ThenBy(h => h.Scores[3]).ThenBy(h => h.Scores[4]).ThenBy(h => h.Scores[5]).ToArray();
        long score = 0;
        for (var i = 0; i < ordered.Length; i++)
        {
            //Console.WriteLine(new String(ordered[i].Cards));
            score += int.Parse(ordered[i].Bid) * (i + 1);
        }

        return Task.FromResult(score.ToString());
    }

    private static Dictionary<char, int> cardValuePt2 = new()
    {
        { 'A', 14 },
        { 'K', 13 },
        { 'Q', 12 },
        { 'J', 1 },
        { 'T', 10 },
        { '9', 9 },
        { '8', 8 },
        { '7', 7 },
        { '6', 6 },
        { '5', 5 },
        { '4', 4 },
        { '3', 3 },
        { '2', 2 }
    };

    private class Hand
    {
        private readonly Dictionary<char, int> cardValue;

        private char[] Cards { get; }

        public string Bid { get; }

        public int[] Scores { get; }

        public Hand(string cards, string bid, Dictionary<char, int> values)
        {
            cardValue = values;
            Cards = cards.ToCharArray();
            Bid = bid;
            Scores = ScoreHand();
        }

        private int[] ScoreHand()
        {
            var cardValues = Cards.Select(c => cardValue[c]).ToArray();
            var groupedNumbers = cardValues.GroupBy(v => v).ToDictionary(v => v.Key, v => v.Count()).ToArray();
            var scoreOrder = cardValues;
            var numJokers = groupedNumbers.SingleOrDefault(g => g.Key == 1).Value;
            var nonJokers = groupedNumbers.Where(k => k.Key != 1).OrderByDescending(g => g.Value).ToArray();
            var maxCount = (nonJokers.Length > 0 ? nonJokers[0].Value : 0) + numJokers;
            if (maxCount >= 4)
            {
                return new[] { (maxCount + 1) }.Concat(scoreOrder).ToArray();
            }

            if (maxCount == 3)
            {
                return new[] { (nonJokers[1].Value == 2 ? 4 : 3) }.Concat(scoreOrder).ToArray();
            }

            if (maxCount == 2)
            {
                return new[] { (nonJokers[1].Value == 2 ? 2 : 1) }.Concat(scoreOrder).ToArray();
            }

            return new[] { 0 }.Concat(scoreOrder).ToArray();
        }
    }

    public string? TestInput => @"32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483";
}