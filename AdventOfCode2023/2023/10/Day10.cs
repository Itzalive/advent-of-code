using System.Collections;

namespace AdventOfCode2023._2023._10;

public class Day10 : IDay
{
    public int Year => 2023;
    public int Day => 10;

    public string? Part1TestSolution => "8";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();

        var startPos = new int[2];
        for (var y = 0; y < inputs.Length; y++)
        {
            for (var x = 0; x< inputs[y].Length; x++)
            {
                if (inputs[y][x] == 'S')
                {
                    startPos = [y, x];
                    break;
                }
            }
        }

        var positions = new List<(int[] position, int[] heading)>();
        foreach (var moveY in new[] { -1, 0, 1 })
        {
            foreach (var moveX in new[] { -1, 0, 1 })
            {
                var testY = startPos[0] + moveY;
                var testX = startPos[1] + moveX;
                if (Math.Abs(moveY) + Math.Abs(moveX) != 1 || testY < 0 || testY >= inputs.Length ||
                    testX < 0 || testX >= inputs[0].Length)
                {
                    continue;
                }

                var directions = connections[inputs[testY][testX]];
                if (directions.Any(h => h[0] == -moveY && h[1] == -moveX))
                {
                    positions.Add((startPos, new []{moveY, moveX}));
                }
            }
        }

        var movements = 0;
        do
        {
            var newPositions = new List<(int[] position, int[] heading)>();
            foreach (var position in positions)
            {
                var newY = position.position[0] + position.heading[0];
                var newX = position.position[1] + position.heading[1];
                var pipe = inputs[newY][newX];
                var directions = connections[pipe];
                var newHeading = directions.Single(d => d[0] != -position.heading[0] || d[1] != -position.heading[1]);
                newPositions.Add((new[] { newY, newX }, newHeading));
            }

            positions = newPositions;
            movements++;

            if (positions[0].position[0] == positions[1].position[0] &&
                positions[0].position[1] == positions[1].position[1])
                break;
        } while (true);

        return Task.FromResult(movements.ToString());
    }

    public string? Part2TestSolution => "8";

    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();

        var startPos = new int[2];

        var map = new Marker[inputs.Length, inputs[0].Length];
        for (var y = 0; y < inputs.Length; y++)
        {
            for (var x = 0; x< inputs[y].Length; x++)
            {
                if (inputs[y][x] == 'S')
                {
                    startPos = [y, x];
                }

                map[y, x] = new Marker();
            }
        }

        var position = startPos;
        var heading = GetHeadings(startPos, inputs)[0];
        var isLeftOffMap = false;
        var isRightOffMap = false;
        do
        {
            var newY = position[0] + heading[0];
            var newX = position[1] + heading[1];
            
            map[newY, newX].LoopChar = inputs[newY][newX];
            var directions = GetHeadings(new[] { newY, newX }, inputs);
            var newHeading = directions.Single(d => d[0] != -heading[0] || d[1] != -heading[1]);

            // Rotate left from newHeading to heading
            var leftIndex = AntiClockwise.IndexOf(AntiClockwise.Single(i => i[0] == newHeading[0] && i[1] == newHeading[1]));
            do
            {
                leftIndex = (leftIndex + 1) % AntiClockwise.Count;
                if (AntiClockwise[leftIndex][0] == -heading[0] && AntiClockwise[leftIndex][1] == -heading[1])
                {
                    break;
                }

                var leftPosY = newY + AntiClockwise[leftIndex][0];
                var leftPosX = newX + AntiClockwise[leftIndex][1];
                if (leftPosY >= 0 && leftPosY < inputs.Length && leftPosX >= 0 && leftPosX < inputs[0].Length)
                {
                    map[leftPosY, leftPosX].IsLeft = true;
                }
                else
                {
                    // if any off map must be outside loop
                    isLeftOffMap = true;
                }
            } while (true);

            // Rotate right from newHeading to heading
            var rightIndex = AntiClockwise.IndexOf(AntiClockwise.Single(i => i[0] == newHeading[0] && i[1] == newHeading[1]));
            do
            {
                rightIndex = (rightIndex + AntiClockwise.Count - 1) % AntiClockwise.Count;
                if (AntiClockwise[rightIndex][0] == -heading[0] && AntiClockwise[rightIndex][1] == -heading[1])
                {
                    break;
                }

                var rightPosY = newY + AntiClockwise[rightIndex][0];
                var rightPosX = newX + AntiClockwise[rightIndex][1];
                if (rightPosY >= 0 && rightPosY < inputs.Length && rightPosX >= 0 && rightPosX < inputs[0].Length)
                {
                    map[rightPosY, rightPosX].IsRight = true;
                }
                else
                {
                    // if any off map must be outside loop
                    isRightOffMap = true;
                }
            } while (true);

            position = new[] { newY, newX };
            heading = newHeading;
        } while (inputs[position[0]][position[1]] != 'S');
        
        Console.WriteLine($"Left off: {isLeftOffMap}");
        Console.WriteLine($"Right off: {isRightOffMap}");

        var toFill = new Queue<int[]>();
        for (var y = 0; y < inputs.Length; y++)
        {
            for (var x = 0; x < inputs[0].Length; x++)
            {
                if (!map[y, x].LoopChar.HasValue &&
                    (!isLeftOffMap && map[y, x].IsLeft || !isRightOffMap && map[y, x].IsRight))
                {
                    toFill.Enqueue(new[] { y, x });
                }
            }
        }

        var cardinalDirections = new List<int[]>
        {
            new[] { 0, 1 },
            new[] { 0, -1 },
            new[] { 1, 1 },
            new[] { -1, 0 },
        };
        var numFilled = 0;
        while(toFill.Count > 0)
        {
            var filler = toFill.Dequeue();
            foreach (var cardinal in cardinalDirections)
            {
                var newY = filler[0] + cardinal[0];
                var newX = filler[1] + cardinal[1];
                if (!map[newY, newX].IsFilled && !map[newY, newX].LoopChar.HasValue)
                {
                    map[newY, newX].IsRight = true;
                    map[newY, newX].IsFilled = true;

                    toFill.Enqueue(new[] { newY, newX });
                    numFilled++;
                }
            }
        }

        Console.WriteLine($"Filled an extra {numFilled} squares");

        DrawMap(inputs, map , isRightOffMap, isLeftOffMap);
        Console.WriteLine();
        
        var numRight = 0;
        for (var y = 0; y < inputs.Length; y++)
        {
            for (var x = 0; x < inputs[0].Length; x++)
            {
                if (!map[y, x].LoopChar.HasValue &&
                    (!isLeftOffMap && map[y, x].IsLeft || !isRightOffMap && map[y, x].IsRight))
                {
                    numRight++;
                }
            }
        }

        return Task.FromResult(numRight.ToString());
    }

    private static void DrawMap(char[][] inputs, Marker[,] map, bool isRightOff = true, bool isLeftOff = false)
    {
        for (var y = 0; y < inputs.Length; y++)
        {
            for (var x = 0; x < inputs[0].Length; x++)
            {
                if (map[y, x].LoopChar.HasValue)
                {
                    Console.Write(map[y, x].LoopChar);
                }
                else if (map[y, x].IsRight && !isRightOff || map[y, x].IsLeft && !isLeftOff)
                {
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write('I');
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.WriteLine();
        }
    }

    private IList<int[]> GetHeadings(int[] position, char[][] inputs)
    {
        var pipe = inputs[position[0]][position[1]];
        if (pipe != 'S')
        {
            return connections[pipe];
        }

        var startHeadings = new List<int[]>();
        foreach (var moveY in new[] { -1, 0, 1 })
        {
            foreach (var moveX in new[] { -1, 0, 1 })
            {
                var testY = position[0] + moveY;
                var testX = position[1] + moveX;
                if (Math.Abs(moveY) + Math.Abs(moveX) != 1 || testY < 0 || testY >= inputs.Length ||
                    testX < 0 || testX >= inputs[0].Length)
                {
                    continue;
                }

                var directions = connections[inputs[testY][testX]];
                if (directions.Any(h => h[0] == -moveY && h[1] == -moveX))
                {
                    startHeadings.Add(new[] { moveY, moveX });
                }
            }
        }

        return startHeadings;
    }

    public class Marker
    {
        public char? LoopChar { get; set; }

        public bool IsLeft { get; set; }

        public bool IsRight { get; set; }
        public bool IsFilled { get; set; }
    }

    private Dictionary<char, IList<int[]>> connections = new()
    {
        {'|', new List<int[]> { new [] {1, 0}, new []{-1, 0} } },
        {'-', new List<int[]> { new [] {0, -1}, new []{0, 1} } },
        {'F', new List<int[]> { new [] {0, 1}, new []{1, 0} } },
        {'L', new List<int[]> { new [] {0, 1}, new []{-1, 0} } },
        {'J', new List<int[]> { new [] {0, -1}, new []{-1, 0} } },
        {'7', new List<int[]> { new [] {0, -1}, new []{1, 0} } },
        { '.', new List<int[]>()}
    };

    private List<int[]> AntiClockwise = new()
    {
        new[] { -1, 0 },
        new[] { -1, -1 },
        new[] { 0, -1 },
        new[] { 1, -1 },
        new[] { 1, 0 },
        new[] { 1, 1 },
        new[] { 0, 1 },
        new[] { -1, 1 }
    };

    public string TestInput => @"OF----7F7F7F7F-7OOOO
O|F--7||||||||FJOOOO
O||OFJ||||||||L7OOOO
FJL7L7LJLJ||LJIL-7OO
L--JOL7IIILJS7F-7L7O
OOOOF-JIIF7FJ|L7L7L7
OOOOL7IF7||L7|IL7L7|
OOOOO|FJLJ|FJ|F7|OLJ
OOOOFJL-7O||O||||OOO
OOOOL---JOLJOLJLJOOO";

    public string? TestInputPt1 => @"..F7.
.FJ|.
SJ.L7
|F--J
LJ...";
}