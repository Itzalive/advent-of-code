namespace AdventOfCode2023._2023._03;

internal class Day3 : IDay
{
    public int Year => 2023;
    public int Day => 3;

    public string? Part1TestSolution => "4361";

    public Task<string> Part1(string input)
    {
        var map = input.Split(Environment.NewLine);

        var score = 0;

        for (int row = 0; row < map.Length; row++)
        {
            var isIncluded = false;
            string number = "";
            for (int col = 0; col < map[row].Length; col++)
            {
                if (char.IsNumber(map[row][col]))
                {
                    if (string.IsNullOrWhiteSpace(number) && col > 0)
                    {
                        if (row > 0 && IsSymbol(map[row - 1][col - 1]))
                        {
                            isIncluded = true;
                        }

                        if (IsSymbol(map[row][col - 1]))
                        {
                            isIncluded = true;
                        }

                        if (row < map.Length - 1 && IsSymbol(map[row + 1][col - 1]))
                        {
                            isIncluded = true;
                        }
                    }

                    number += map[row][col];

                    if (row > 0 && IsSymbol(map[row - 1][col]))
                    {
                        isIncluded = true;
                    }

                    if (row < map.Length - 1 && IsSymbol(map[row + 1][col]))
                    {
                        isIncluded = true;
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(number))
                    {
                        if (row > 0 && IsSymbol(map[row - 1][col]))
                        {
                            isIncluded = true;
                        }

                        if (IsSymbol(map[row][col]))
                        {
                            isIncluded = true;
                        }

                        if (row < map.Length - 1 && IsSymbol(map[row + 1][col]))
                        {
                            isIncluded = true;
                        }
                    }

                    //if (!string.IsNullOrWhiteSpace(number))
                    //    Console.WriteLine($"{number}: {isIncluded}");
                    if (isIncluded && !string.IsNullOrWhiteSpace(number))
                    {
                        score += int.Parse(number);
                    }

                    isIncluded = false;
                    number = "";
                }
            }
            
            //if (!string.IsNullOrWhiteSpace(number))
            //    Console.WriteLine($"{number}: {isIncluded}");
            if (isIncluded && !string.IsNullOrWhiteSpace(number))
            {
                score += int.Parse(number);
            }
        }

        return Task.FromResult(score.ToString());
    }

    private bool IsSymbol(char c)
    {
        return !char.IsNumber(c) && c != '.';
    }

    public string? Part2TestSolution => "467835";

    public Task<string> Part2(string input)
    {
        var map = input.Split(Environment.NewLine);

        var numberMap = new Number?[map.Length,map[0].Length];
        long score = 0;

        for (int row = 0; row < map.Length; row++)
        {
            Number number = new Number();
            var numberString = "";
            for (int col = 0; col < map[row].Length; col++)
            {
                if (char.IsNumber(map[row][col]))
                {
                    numberString += map[row][col];
                    numberMap[row, col] = number;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(numberString))
                    {
                        number.Value = int.Parse(numberString);
                    }
                    number = new Number();
                    numberString = "";
                }
            }
            
            if (!string.IsNullOrWhiteSpace(numberString))
            {
                number.Value = int.Parse(numberString);
            }
        }

        for (int row = 0; row < map.Length; row++)
        {
            for (int col = 0; col < map[row].Length; col++)
            {
                if (map[row][col] == '*')
                {
                    var numbers = new List<Number>();
                    if (row > 0)
                    {
                        if (col > 0 && numberMap[row - 1, col - 1] != null && !numbers.Contains(numberMap[row - 1, col - 1]))
                        {
                            numbers.Add(numberMap[row - 1, col - 1]);
                        }

                        if (numberMap[row - 1, col] != null && !numbers.Contains(numberMap[row - 1, col]))
                        {
                            numbers.Add(numberMap[row - 1, col]);
                        }

                        if(col < map[row].Length - 1 && numberMap[row - 1, col + 1] != null && !numbers.Contains(numberMap[row - 1, col + 1]))
                        {
                            numbers.Add(numberMap[row - 1, col + 1]);
                        }
                    }

                    
                    if (col > 0 && numberMap[row, col - 1] != null && !numbers.Contains(numberMap[row, col - 1]))
                    {
                        numbers.Add(numberMap[row, col - 1]);
                    }

                    if(col < map[row].Length - 1 && numberMap[row, col + 1] != null && !numbers.Contains(numberMap[row, col + 1]))
                    {
                        numbers.Add(numberMap[row, col + 1]);
                    }

                    if (row < map.Length - 1)
                    {
                        if (col > 0 && numberMap[row + 1, col - 1] != null && !numbers.Contains(numberMap[row + 1, col - 1]))
                        {
                            numbers.Add(numberMap[row + 1, col - 1]);
                        }

                        if (numberMap[row + 1, col] != null && !numbers.Contains(numberMap[row + 1, col]))
                        {
                            numbers.Add(numberMap[row + 1, col]);
                        }

                        if(col < map[row].Length - 1 && numberMap[row + 1, col + 1] != null && !numbers.Contains(numberMap[row + 1, col + 1]))
                        {
                            numbers.Add(numberMap[row + 1, col + 1]);
                        }
                    }

                    if (numbers.Count == 2)
                    {
                        long gearScore = 1;
                        foreach (var number in numbers)
                        {
                            gearScore *= (long)number.Value;
                        }

                        score += gearScore;
                    }
                }
            }
        }

        return Task.FromResult(score.ToString());
    }

    public string? TestInput => @"467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..";
}

internal class Number
{
    public int Value { get; set; }
}