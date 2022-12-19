namespace AdventOfCode2022._2022._02;

internal class Day2 : IDay
{
    public int Year => 2022;
    public int Day => 2;

    public async Task<string> Part1(string input)
    {
        var elements = input.Split(Environment.NewLine);
        var score = 0;
        foreach (var element in elements)
        {
            var inputs = element.Split(" ");
            var roundScore = 0;
            switch (inputs[1])
            {
                case "X":
                    roundScore += 1;
                    break;

                case "Y":
                    roundScore += 2;
                    break;

                case "Z":
                    roundScore += 3;
                    break;
            }

            var leftValue = (int) inputs[0][0] - (int) 'B';
            var rightValue = (int) inputs[1][0] - (int) 'Y';

            if (leftValue == rightValue)
            {
                roundScore += 3;
            }
            else if ((leftValue - rightValue) == -1 || (leftValue - rightValue) == 2)
            {
                roundScore += 6;
            }

            //Console.WriteLine($"{element} - {leftValue} {rightValue} - {roundScore}");
            score += roundScore;
        }

        return score.ToString();
    }


    public async Task<string> Part2(string input)
    {
        var elements = input.Split(Environment.NewLine);
        var score = 0;
        foreach (var element in elements)
        {
            var inputs = element.Split(" ");
            var charScore = (int) inputs[0][0] - (int) 'A' + 1;
            switch (inputs[1])
            {
                case "X":
                    //Console.WriteLine($"Lose {inputs[0]} {((charScore + 1) % 3) + 1}");
                    score += ((charScore + 1) % 3) + 1;
                    break;
                case "Y":
                    score += 3;
                    score += charScore;
                    break;
                case "Z":
                    //Console.WriteLine($"Win {inputs[0]} {((charScore) % 3) + 1}");
                    score += 6;
                    score += ((charScore) % 3) + 1;
                    break;
            }
        }

        return score.ToString();
    }

    public string? TestInput => null;
}