namespace AdventOfCode2023._2021._21;

public class Day21 : IDay
{
    public int Year => 2021;

    public int Day => 21;

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.Split(":")[1].Trim()).Select(int.Parse).ToArray();
        return Task.FromResult("");
    }

    
    public Task<string> Part2(string input)
    {
        var startingPositions = input.Split(Environment.NewLine).Select(l => l.Split(":")[1].Trim()).Select(int.Parse).ToArray();

        var placesMovedCount = new Dictionary<int, int>
        {
            {3, 0},
            { 4, 0 },
            { 5, 0 },
            { 6, 0 },
            { 7, 0 },
            { 8, 0 },
            { 9, 0 }, 
        };

        for (var d1 = 1; d1 <= 3; d1++)
        {
            for (var d2 = 1; d2 <= 3; d2++)
            {
                for (var d3 = 1; d3 <= 3; d3++)
                {
                    var places = d1 + d2 + d3;
                    placesMovedCount[places]++;
                }
            }
        }

        var placesMoved = placesMovedCount.Select(p => (Places: p.Key, Count: p.Value)).ToList();

        var completeGameVariants = new List<Game>();
        var runningGameVariants = new List<Game>()
        {
            new Game
            {
                Player1Position = startingPositions[0] - 1,
                Player2Position = startingPositions[1] - 1
            }
        };

        while (runningGameVariants.Any())
        {
            // Player 1 moves
            var afterPlayer1GameVariants = new List<Game>();
            foreach (var runningGameVariant in runningGameVariants)
            {
                foreach (var placeMoved in placesMoved)
                {
                    var game = runningGameVariant.Player1Move(placeMoved.Places, placeMoved.Count);
                    if(game.Player1Wins)
                        completeGameVariants.Add(game);
                    else
                        afterPlayer1GameVariants.Add(game);
                }
            }

            // Player 2 moves
            var afterPlayer2GameVariants = new List<Game>();
            foreach (var runningGameVariant in afterPlayer1GameVariants)
            {
                foreach (var placeMoved in placesMoved)
                {
                    var game = runningGameVariant.Player2Move(placeMoved.Places, placeMoved.Count);
                    if (game.Player2Wins)
                        completeGameVariants.Add(game);
                    else
                        afterPlayer2GameVariants.Add(game);
                }
            }

            runningGameVariants = afterPlayer2GameVariants;
        }

        var player1Wins = completeGameVariants.Where(g => g.Player1Wins).Sum(g => g.Occurences);
        var player2Wins = completeGameVariants.Where(g => g.Player2Wins).Sum(g => g.Occurences);
        Console.WriteLine("Player 1 wins: " + player1Wins);
        Console.WriteLine("Player 2 wins: " + player2Wins);
        return Task.FromResult(Math.Max(player1Wins, player2Wins).ToString());
    }

    public string? TestInput => @"Player 1 starting position: 4
Player 2 starting position: 8";
}

public record Game
{
    public int Player1Position { get; set; }

    public int Player2Position { get; set; }

    public int Player1Score { get; set; }

    public int Player2Score { get; set; }

    public long Occurences { get; set; } = 1;

    public bool Player1Wins { get; set; }

    public bool Player2Wins { get; set; }

    public Game Player1Move(int places, int occurences)
    {
        var player1Position = (Player1Position + places) % 10;
        var player1Score = Player1Score + player1Position + 1;
        return new Game
        {
            Player1Position = player1Position,
            Player2Position = Player2Position,
            Player1Score = player1Score,
            Player2Score = Player2Score,
            Occurences = Occurences * occurences,
            Player1Wins = player1Score >= 21
        };
    }
    
    public Game Player2Move(int places, int occurences)
    {
        var player2Position = (Player2Position + places) % 10;
        var player2Score = Player2Score + player2Position + 1;
        return new Game
        {
            Player2Position = player2Position,
            Player1Position = Player1Position,
            Player2Score = player2Score,
            Player1Score = Player1Score,
            Occurences = Occurences * occurences,
            Player2Wins = player2Score >= 21
        };
    }


}