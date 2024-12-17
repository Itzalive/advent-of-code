using System.Text.RegularExpressions;

namespace AdventOfCode2023._2024._04;

public class Day4 : IDay
{
    public int Year => 2024;

    public int Day => 4;

    public string? Part1TestSolution => "18";

    public Task<string> Part1(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var map = lines.Select(l => l.ToCharArray()).ToArray();
        var matches = 0;

        // Check for horizontal
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length - 3; x++)
            {
                if (map[y][x] == 'X' && map[y][x + 1] == 'M' && map[y][x + 2] == 'A' && map[y][x + 3] == 'S') matches++;
                if (map[y][x] == 'S' && map[y][x + 1] == 'A' && map[y][x + 2] == 'M' && map[y][x + 3] == 'X') matches++;
            }
        }

        // Check for vertical
        for (var y = 0; y < map.Length - 3; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == 'X' && map[y + 1][x] == 'M' && map[y + 2][x] == 'A' && map[y + 3][x] == 'S') matches++;
                if (map[y][x] == 'S' && map[y + 1][x] == 'A' && map[y + 2][x] == 'M' && map[y + 3][x] == 'X') matches++;
            }
        }

        // Check for diagonal /
        for (var y = 3; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length - 3; x++)
            {
                if (map[y][x] == 'X' && map[y - 1][x + 1] == 'M' && map[y - 2][x + 2] == 'A' && map[y - 3][x + 3] == 'S') matches++;
                if (map[y][x] == 'S' && map[y - 1][x + 1] == 'A' && map[y - 2][x + 2] == 'M' && map[y - 3][x + 3] == 'X') matches++;
            }
        }

        // Check for diagonal \
        for (var y = 0; y < map.Length - 3; y++)
        {
            for (var x = 0; x < map[y].Length - 3; x++)
            {
                if (map[y][x] == 'X' && map[y + 1][x + 1] == 'M' && map[y + 2][x + 2] == 'A' && map[y + 3][x + 3] == 'S') matches++;
                if (map[y][x] == 'S' && map[y + 1][x + 1] == 'A' && map[y + 2][x + 2] == 'M' && map[y + 3][x + 3] == 'X') matches++;
            }
        }

        return Task.FromResult(matches.ToString());
    }

    public string? Part2TestSolution => "9";


    public Task<string> Part2(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var map = lines.Select(l => l.ToCharArray()).ToArray();
        var matches = 0;

        // Check for horizontal
        for (var y = 1; y < map.Length - 1; y++)
        {
            for (var x = 1; x < map[y].Length - 1; x++)
            {
                if (map[y][x] != 'A') continue;

                var masses = 0;
                if (map[y - 1][x - 1] == 'M' && map[y + 1][x + 1] == 'S') masses++;
                if (map[y + 1][x + 1] == 'M' && map[y - 1][x - 1] == 'S') masses++;
                if (map[y - 1][x + 1] == 'M' && map[y + 1][x - 1] == 'S') masses++;
                if (map[y + 1][x - 1] == 'M' && map[y - 1][x + 1] == 'S') masses++;

                if(masses ==2)
                    matches++;
            }
        }

        return Task.FromResult(matches.ToString());
    }

    public string? TestInput => "MMMSXXMASM\r\nMSAMXMSMSA\r\nAMXSXMAAMM\r\nMSAMASMSMX\r\nXMASAMXAMM\r\nXXAMMXXAMA\r\nSMSMSASXSS\r\nSAXAMASAAA\r\nMAMMMXMMMM\r\nMXMXAXMASX";
}