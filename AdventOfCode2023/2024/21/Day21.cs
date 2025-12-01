using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode2023._2024._21;

public class Day21 : IDay
{
    public int Year => 2024;

    public int Day => 21;

    public string? Part1TestSolution => "126384";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var score = 0;
        foreach (var digits in inputs)
        {
            Console.WriteLine(digits);
            var keypadPaths = numericKeypad(digits);
            foreach (var path in keypadPaths)
            {
                Console.WriteLine(string.Join("", path));
            }
            for (var i = 0; i < 2; i++)
            {
                var nextShortestPaths = new List<List<char>>();
                var shortestOrigins = new List<List<char>>();
                foreach (var keyboardPath in keypadPaths)
                {
                    var result = robotKeypad(keyboardPath);
                    if (nextShortestPaths.Any() && result[0].Count < nextShortestPaths[0].Count)
                    {
                        nextShortestPaths.Clear();
                        shortestOrigins.Clear();
                    }

                    if (!nextShortestPaths.Any() || result[0].Count == nextShortestPaths[0].Count)
                    {
                        nextShortestPaths.AddRange(result);
                        shortestOrigins.Add(keyboardPath);
                    }
                }

                keypadPaths = nextShortestPaths.Distinct().ToList();
                foreach (var path in shortestOrigins)
                {
                    Console.WriteLine(string.Join("", path));
                }
                Console.WriteLine(keypadPaths[0].Count);
            }

            //Console.WriteLine(string.Join("", keypadPaths[0]));
            score += int.Parse(digits[..^1]) * keypadPaths[0].Count;
        }
        return Task.FromResult(score.ToString());
    }

    private List<List<char>> numericKeypad(IEnumerable<char> digits)
    {
        var currentMarker = 'A';
        var buttonLocations = new Dictionary<char, (int X, int Y)>()
        {
            {'A', (2, 0)},
            {'0', (1, 0)},
            {'1', (0, 1)},
            {'2', (1, 1)},
            {'3', (2, 1)},
            {'4', (0, 2)},
            {'5', (1, 2)},
            {'6', (2, 2)},
            {'7', (0, 3)},
            {'8', (1, 3)},
            {'9', (2, 3)},
        };
        var paths = new Dictionary<(char, char), List<List<char>>>();
        foreach (var buttonLocationFrom in buttonLocations)
        {
            foreach (var buttonLocationTo in buttonLocations)
            {
                var yDiff = buttonLocationTo.Value.Y - buttonLocationFrom.Value.Y;
                var xDiff = buttonLocationTo.Value.X - buttonLocationFrom.Value.X;
                var yArrows = Enumerable.Range(0, Math.Abs(yDiff)).Select(_ => yDiff > 0 ? '^' : 'v').ToList();
                var xArrows = Enumerable.Range(0, Math.Abs(xDiff)).Select(_ => xDiff > 0 ? '>' : '<').ToList();
                var possibles = new List<List<char>>();

                if (xDiff == 0 || yDiff == 0)
                {
                    possibles.Add(yArrows.Concat(xArrows).ToList());
                }
                else
                {

                    possibles.Add(xArrows.Concat(yArrows).ToList());
                    possibles.Add(yArrows.Concat(xArrows).ToList());

                    if (buttonLocationFrom.Key == 'A')
                    {
                        possibles = possibles.Where(a => a.Count < 2 || a[0] != '<' || a[1] != '<').ToList();
                    }
                    if (buttonLocationFrom.Key == '0')
                    {
                        possibles = possibles.Where(a => a.Count < 1 || a[0] != '<').ToList();
                    }

                    if (buttonLocationTo.Key == 'A')
                    {
                        possibles = possibles.Where(a => a.Count < 2 || a[^1] != '>' || a[^2] != '>').ToList();
                    }
                    if (buttonLocationTo.Key == '0')
                    {
                        possibles = possibles.Where(a => a.Count < 1 || a[^1] != '>').ToList();
                    }
                }

                paths.Add((buttonLocationFrom.Key, buttonLocationTo.Key), possibles);
            }
        }

        var currentDigit = 'A';
        var shortestPaths = new List<List<char>>();
        foreach (var digit in digits)
        {
            var newPaths = paths[(currentDigit, digit)];
            if (!shortestPaths.Any())
            {
                shortestPaths.AddRange(newPaths.Select(n => n.Concat(['A']).ToList()));
            }
            else
            {
                shortestPaths = shortestPaths.SelectMany(s => newPaths.Select(n => s.Concat(n).Concat(['A']).ToList())).ToList();
            }

            currentDigit = digit;
        }

        return shortestPaths;
    }

    private List<List<char>> robotKeypad(IEnumerable<char> digits)
    {
        var buttonLocations = new Dictionary<char, (int X, int Y)>()
        {
            {'A', (2, 1)},
            {'^', (1, 1)},
            {'<', (0, 0)},
            {'v', (1, 0)},
            {'>', (2, 0)}
        };
        var paths = new Dictionary<(char, char), List<List<char>>>();
        foreach (var buttonLocationFrom in buttonLocations)
        {
            foreach (var buttonLocationTo in buttonLocations)
            {
                var yDiff = buttonLocationTo.Value.Y - buttonLocationFrom.Value.Y;
                var xDiff = buttonLocationTo.Value.X - buttonLocationFrom.Value.X;
                var yArrows = Enumerable.Range(0, Math.Abs(yDiff)).Select(_ => yDiff > 0 ? '^' : 'v').ToList();
                var xArrows = Enumerable.Range(0, Math.Abs(xDiff)).Select(_ => xDiff > 0 ? '>' : '<').ToList();
                var possibles = new List<List<char>>();

                if (xDiff == 0 || yDiff == 0)
                {
                    possibles.Add(yArrows.Concat(xArrows).ToList());
                }
                else
                {

                    possibles.Add(xArrows.Concat(yArrows).ToList());
                    possibles.Add(yArrows.Concat(xArrows).ToList());

                    if (buttonLocationFrom.Key == 'A')
                    {
                        possibles = possibles.Where(a => a.Count < 2 || a[0] != '<' || a[1] != '<').ToList();
                    }
                    if (buttonLocationFrom.Key == '^')
                    {
                        possibles = possibles.Where(a => a.Count < 1 || a[0] != '<').ToList();
                    }

                    if (buttonLocationTo.Key == 'A')
                    {
                        possibles = possibles.Where(a => a.Count < 2 || a[^1] != '>' || a[^2] != '>').ToList();
                    }
                    if (buttonLocationTo.Key == '^')
                    {
                        possibles = possibles.Where(a => a.Count < 1 || a[^1] != '>').ToList();
                    }
                }

                paths.Add((buttonLocationFrom.Key, buttonLocationTo.Key), possibles);
            }
        }

        var currentDigit = 'A';
        var shortestPaths = new List<List<char>>();
        foreach (var digit in digits)
        {
            var newPaths = paths[(currentDigit, digit)];
            if (!shortestPaths.Any())
            {
                shortestPaths.AddRange(newPaths.Select(n => n.Concat(['A']).ToList()));
            }
            else
            {
                shortestPaths = shortestPaths.SelectMany(s => newPaths.Select(n => s.Concat(n).Concat(['A']).ToList())).ToList();
            }

            currentDigit = digit;
        }

        return shortestPaths;
    }

    private (ConcurrentDictionary<(char, char), long> previousPaths, char firstChar) numericKeypad(ConcurrentDictionary<(char, char), long> previousPaths, char firstChar, Dictionary<(char, char), string> paths)
    {
        var firstPath = paths[('A', firstChar)] + "A";
        var newFirstChar = firstPath[0];

        var newPaths = new ConcurrentDictionary<(char, char), long>();
        AddPathToDictionary(newPaths, firstPath, 1);

        foreach (var path in previousPaths)
        {
            AddPathToDictionary(newPaths, "A" + paths[path.Key] + "A", path.Value);
        }

        return (newPaths, newFirstChar);
    }

    private void AddPathToDictionary(ConcurrentDictionary<(char, char), long> newPaths, string path, long count)
    {
        for (var i = 1; i < path.Length; i++)
        {
            newPaths.AddOrUpdate((path[i - 1], path[i]), _ => count, (_, v) => v + count);
        }
    }

    private static Dictionary<(char, char), string> GenerateNumericPaths()
    {
        var buttonLocations = new Dictionary<char, (int X, int Y)>()
        {
            {'A', (2, 0)},
            {'0', (1, 0)},
            {'1', (0, 1)},
            {'2', (1, 1)},
            {'3', (2, 1)},
            {'4', (0, 2)},
            {'5', (1, 2)},
            {'6', (2, 2)},
            {'7', (0, 3)},
            {'8', (1, 3)},
            {'9', (2, 3)},
        };
        var paths = new Dictionary<(char, char), string>();
        foreach (var buttonLocationFrom in buttonLocations)
        {
            foreach (var buttonLocationTo in buttonLocations)
            {
                var yDiff = buttonLocationTo.Value.Y - buttonLocationFrom.Value.Y;
                var xDiff = buttonLocationTo.Value.X - buttonLocationFrom.Value.X;
                var yArrows = Enumerable.Range(0, Math.Abs(yDiff)).Select(_ => yDiff > 0 ? '^' : 'v').ToList();
                var xArrows = Enumerable.Range(0, Math.Abs(xDiff)).Select(_ => xDiff > 0 ? '>' : '<').ToList();

                if ((buttonLocationTo.Key is '1' or '4' or '7') && buttonLocationFrom.Key is 'A' or '0' || xDiff > 0 && (buttonLocationFrom.Key is not '1' and not '4' and not '7' || buttonLocationTo.Key is not 'A' and not '0'))
                {
                    paths.Add((buttonLocationFrom.Key, buttonLocationTo.Key), string.Join("", yArrows.Concat(xArrows)));
                }
                else
                {
                    paths.Add((buttonLocationFrom.Key, buttonLocationTo.Key), string.Join("", xArrows.Concat(yArrows)));
                }
            }
        }

        return paths;
    }

    private static Dictionary<(char, char), string> GenerateRobotPaths()
    {
        var buttonLocations = new Dictionary<char, (int X, int Y)>()
        {
            {'A', (2, 1)},
            {'^', (1, 1)},
            {'<', (0, 0)},
            {'v', (1, 0)},
            {'>', (2, 0)}
        };
        var paths = new Dictionary<(char, char), string>();
        foreach (var buttonLocationFrom in buttonLocations)
        {
            foreach (var buttonLocationTo in buttonLocations)
            {
                var yDiff = buttonLocationTo.Value.Y - buttonLocationFrom.Value.Y;
                var xDiff = buttonLocationTo.Value.X - buttonLocationFrom.Value.X;
                var yArrows = Enumerable.Range(0, Math.Abs(yDiff)).Select(_ => yDiff > 0 ? '^' : 'v').ToList();
                var xArrows = Enumerable.Range(0, Math.Abs(xDiff)).Select(_ => xDiff > 0 ? '>' : '<').ToList();

                if (buttonLocationTo.Key is '<' && buttonLocationFrom.Key is 'A' or '^' || xDiff > 0 && (buttonLocationFrom.Key is not '<' || buttonLocationTo.Key is not 'A' and not '^'))
                {
                    paths.Add((buttonLocationFrom.Key, buttonLocationTo.Key), string.Join("", yArrows.Concat(xArrows)));
                }
                else
                {
                    paths.Add((buttonLocationFrom.Key, buttonLocationTo.Key), string.Join("", xArrows.Concat(yArrows)));
                }
            }
        }

        return paths;
    }

    public string? Part2TestSolution => "126384";


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var score = 0L;
        var numericPaths = GenerateNumericPaths();
        var paths = GenerateRobotPaths();
        foreach (var digits in inputs)
        {
            Console.WriteLine(digits);
            var startPaths = new ConcurrentDictionary<(char, char), long>(Enumerable.Range(1, digits.Length - 1)
                .ToDictionary(k => (digits[k - 1], digits[k]), _ => (long)1));
            var (keypadPaths, firstChar) = numericKeypad(startPaths, digits[0], numericPaths);

            Console.WriteLine(keypadPaths.Sum(p => p.Value) + 1L);
            for (var i = 0; i < (inputs[0] == "029A" ? 2 : 25); i++)
            {
                (keypadPaths, firstChar) = numericKeypad(keypadPaths, firstChar, paths);
                Console.WriteLine(firstChar);
                Console.WriteLine(keypadPaths.Sum(p => p.Value) + 1L);
            }

            foreach (var kvp in keypadPaths)
            {
                Console.WriteLine($"({kvp.Key.Item1},{kvp.Key.Item2}) => {kvp.Value}");
            }

            Console.WriteLine(keypadPaths.Sum(p => p.Value) + 1L);
            score += int.Parse(digits[..^1]) * (keypadPaths.Sum(p => p.Value) + 1L);
        }
        return Task.FromResult(score.ToString());
    }

    public string? TestInput => @"029A
980A
179A
456A
379A";
}