namespace AdventOfCode2023._2023._02;

internal class Day2 : IDay
{
    public int Year => 2023;
    public int Day => 2;

    public string? Part1TestSolution => null;

    public async Task<string> Part1(string input)
    {
        var elements = input.Split(Environment.NewLine);

        var games = elements.Select(e => new Game(e));

        var score = games.Where(g =>
                g.Picks.Max(p => p.Red) <= 12 && g.Picks.Max(p => p.Green) <= 13 & g.Picks.Max(p => p.Blue) <= 14)
            .Sum(g => g.ID);

        return score.ToString();
    }

    public string? Part2TestSolution => null;


    public async Task<string> Part2(string input)
    {
        var elements = input.Split(Environment.NewLine);
        var games = elements.Select(e => new Game(e));

        var score = games.Sum(g =>
            g.Picks.Max(p => p.Red) * g.Picks.Max(p => p.Green) * g.Picks.Max(p => p.Blue));

        return score.ToString();
    }

    public class Game
    {
        public int ID { get; set; }

        public List<Pick> Picks { get; set; }

        public Game(string line)
        {
            var splitA = line.Split(':');
            ID = int.Parse(splitA[0].Replace("Game", "").Trim());
            Picks = splitA[1].Trim().Split(";").Select(p => new Pick(p)).ToList();
        }
    }

    public class Pick
    {
        public int Red { get; set; }

        public int Green { get; set; }

        public int Blue { get; set; }

        public Pick(string picks)
        {
            var colours = picks.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim()).ToArray();
            foreach (var colour in colours)
            {
                var colourParts = colour.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (colourParts[1] == "red")
                {
                    Red = int.Parse(colourParts[0]);
                }
                else if (colourParts[1] == "green")
                {
                    Green = int.Parse(colourParts[0]);
                }
                else if (colourParts[1] == "blue")
                {
                    Blue = int.Parse(colourParts[0]);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }

    public string? TestInput => @"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green";
}