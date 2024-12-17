using AdventOfCode2023._2023._21;

namespace AdventOfCode2023._2024._06;

public class Day6 : IDay
{
    public int Year => 2024;

    public int Day => 6;

    public string? Part1TestSolution => "41";

    public Task<string> Part1(string input)
    {
        var positions = new[] { '^', '>', 'V', '<' };
        var map = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        var guardX = -1;
        var guardY = -1;

        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                if (positions.Contains(map[y][x])){
                    guardX = x;
                    guardY = y;
                    break;
                }
            }

            if (guardX != -1) break;
        }

        var direction = map[guardY][guardX];

        var positionsCovered = 0;
        while (true)
        {
            var newX = guardX;
            var newY = guardY;
            if (direction == '^') newY--;
            if (direction == 'V') newY++;
            if (direction == '>') newX++;
            if (direction == '<') newX--;

            if (newY < 0 || newY >= map.Length || newX < 0 || newX >= map[0].Length)
            {
                if (map[guardY][guardX] == '.')
                {
                    positionsCovered++;
                }
                map[guardY][guardX] = direction;
                break;
            }

            if (map[newY][newX] == '#')
            {
                direction = positions[(Array.IndexOf(positions, direction) + 1) % 4];
            }
            else
            {
                if (map[guardY][guardX] == '.')
                {
                    positionsCovered++;
                }

                map[guardY][guardX] = direction;
                guardY = newY;
                guardX = newX;
            }
        }

        foreach (var line in map)
        {
            Console.WriteLine(string.Join("", line));
        }

        return Task.FromResult(positionsCovered.ToString());
    }

    public string? Part2TestSolution => "6";


    public Task<string> Part2(string input)
    {
        var positions = new[] { '^', '>', 'V', '<' };
        var map = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();

        var guardStartX = -1;
        var guardStartY = -1;
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                if (positions.Contains(map[y][x]))
                {
                    guardStartX = x;
                    guardStartY = y;
                    break;
                }
            }

            if (guardStartX != -1) break;
        }

        var loops = 0;

        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                map = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
                var guardX = guardStartX;
                var guardY = guardStartY;
                if (map[y][x] != '.') continue;
                
                map[y][x] = '#';

                var direction = map[guardY][guardX];
                var eternalLoop = false;
                while (true)
                {
                    var newX = guardX;
                    var newY = guardY;
                    if (direction == '^') newY--;
                    if (direction == 'V') newY++;
                    if (direction == '>') newX++;
                    if (direction == '<') newX--;

                    if (newY < 0 || newY >= map.Length || newX < 0 || newX >= map[0].Length)
                    {
                        break;
                    }

                    if (map[newY][newX] == direction)
                    {
                        eternalLoop = true;
                        break;
                    }

                    if (map[newY][newX] == '#')
                    {
                        direction = positions[(Array.IndexOf(positions, direction) + 1) % 4];

                        if (map[guardY][guardX] == direction)
                        {
                            eternalLoop = true;
                            break;
                        }
                    }
                    else
                    {
                        if (map[guardY][guardX] == '.')
                        {
                            map[guardY][guardX] = direction;
                        }

                        guardY = newY;
                        guardX = newX;
                    }
                }

                if (!eternalLoop) continue;

                loops ++;

                //foreach (var line in map)
                //{
                //    Console.WriteLine(string.Join("", line));
                //}
            }
        }

        return Task.FromResult(loops.ToString());
    }

    public string? TestInput => @"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...";
}