namespace AdventOfCode2023._2024._15;

public class Day15 : IDay
{
    public int Year => 2024;

    public int Day => 15;

    public string? Part1TestSolution => "10092";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine + Environment.NewLine).ToArray();
        var map = inputs[0].Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        var commands = inputs[1].Replace("\r\n", "").ToCharArray();
        var posY = -1;
        var posX = -1;
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == '@')
                {
                    posY = y;
                    posX = x;
                    break;
                }
            }

            if (posY != -1)
                break;
        }

        var commandDirection = new Dictionary<char, (int X, int Y)>()
        {
            { '<', (-1, 0) },
            { '>', (1, 0) },
            { '^', (0, -1) },
            { 'v', (0, 1) },
        };

        foreach (var command in commands)
        {
            var direction = commandDirection[command];
            var searchX = posX;
            var searchY = posY;
            do
            {
                searchX += direction.X;
                searchY += direction.Y;
                if (map[searchY][searchX] == '#')
                {
                    break;
                }

                if (map[searchY][searchX] == '.')
                {
                    map[searchY][searchX] = 'O';
                    map[posY][posX] = '.';
                    posY += direction.Y;
                    posX += direction.X;
                    map[posY][posX] = '@';
                    break;
                }
            } while (true);


        }

        var score = 0;
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                Console.Write(map[y][x]);
                if (map[y][x] == 'O')
                {
                    score += 100 * y + x;
                }
            }
            Console.WriteLine();
        }

        return Task.FromResult(score.ToString());
    }

    public string? Part2TestSolution => null;


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine + Environment.NewLine).ToArray();
        var map = inputs[0].Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        var bigMap = new char[map.Length][];
        var commands = inputs[1].Replace("\r\n", "").ToCharArray();
        var posY = -1;
        var posX = -1;
        for (var y = 0; y < map.Length; y++)
        {
            bigMap[y] = new char[map[y].Length * 2];
            for (var x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == '#')
                {
                    bigMap[y][x * 2] = '#';
                    bigMap[y][x * 2 + 1] = '#';
                }
                else if (map[y][x] == 'O')
                {
                    bigMap[y][x * 2] = '[';
                    bigMap[y][x * 2 + 1] = ']';
                }
                else if (map[y][x] == '.')
                {
                    bigMap[y][x * 2] = '.';
                    bigMap[y][x * 2 + 1] = '.';
                }
                else if (map[y][x] == '@')
                {
                    bigMap[y][x * 2] = '@';
                    bigMap[y][x * 2 + 1] = '.';
                    posY = y;
                    posX = x * 2;
                }
            }
        }

        var commandDirection = new Dictionary<char, (int X, int Y)>()
        {
            { '<', (-1, 0) },
            { '>', (1, 0) },
            { '^', (0, -1) },
            { 'v', (0, 1) },
        };

        foreach (var command in commands)
        {
            //foreach (var t in bigMap)
            //{
            //    foreach (var t1 in t)
            //    {
            //        Console.Write(t1);
            //    }

            //    Console.WriteLine();
            //}

            var direction = commandDirection[command];
            var searchX = posX;
            var searchY = posY;
            var boxToMove = new List<(int X, int Y, char value)>();
            var toCheck = new Queue<(int X, int Y)>();
            toCheck.Enqueue((posX + direction.X, posY + direction.Y));
            var canMove = true;
            while (toCheck.Any())
            {
                var positionToCheck = toCheck.Dequeue();
                if (bigMap[positionToCheck.Y][positionToCheck.X] == ']')
                {
                    boxToMove.Add((positionToCheck.X + direction.X - 1, positionToCheck.Y + direction.Y, '['));
                    boxToMove.Add((positionToCheck.X + direction.X, positionToCheck.Y + direction.Y, ']'));
                    boxToMove.Add((positionToCheck.X - 1, positionToCheck.Y, '.'));
                    boxToMove.Add((positionToCheck.X, positionToCheck.Y, '.'));

                    if (direction.X <= 0)
                    {
                        toCheck.Enqueue((positionToCheck.X + direction.X - 1, positionToCheck.Y + direction.Y));
                    }

                    if (direction.X >= 0)
                    {
                        toCheck.Enqueue((positionToCheck.X + direction.X, positionToCheck.Y + direction.Y));
                    }
                }
                else if (bigMap[positionToCheck.Y][positionToCheck.X] == '[')
                {
                    boxToMove.Add((positionToCheck.X + direction.X, positionToCheck.Y + direction.Y, '['));
                    boxToMove.Add((positionToCheck.X + direction.X + 1, positionToCheck.Y + direction.Y, ']'));
                    boxToMove.Add((positionToCheck.X, positionToCheck.Y, '.'));
                    boxToMove.Add((positionToCheck.X + 1, positionToCheck.Y, '.'));

                    if (direction.X <= 0)
                    {
                        toCheck.Enqueue((positionToCheck.X + direction.X, positionToCheck.Y + direction.Y));
                    }

                    if (direction.X >= 0)
                    {
                        toCheck.Enqueue((positionToCheck.X + direction.X + 1, positionToCheck.Y + direction.Y));
                    }
                }
                else if (bigMap[positionToCheck.Y][positionToCheck.X] == '#')
                {
                    canMove = false;
                    break;
                }
            }

            if (canMove)
            {
                boxToMove.Reverse();
                foreach (var toMove in boxToMove)
                {
                    bigMap[toMove.Y][toMove.X] = toMove.value;
                }

                bigMap[posY][posX] = '.';
                posX += direction.X;
                posY += direction.Y;
                bigMap[posY][posX] = '@';
            }
        }

        var score = 0;
        for (var y = 0; y < bigMap.Length; y++)
        {
            for (var x = 0; x < bigMap[y].Length; x++)
            {
                Console.Write(bigMap[y][x]);
                if (bigMap[y][x] == '[')
                {
                    score += 100 * y + x;
                }
            }
            Console.WriteLine();
        }

        return Task.FromResult(score.ToString());
    }

    public string? TestInput => @"##########
#..O..O.O#
#......O.#
#.OO..O.O#
#..O@..O.#
#O#..O...#
#O..O..O.#
#.OO.O.OO#
#....O...#
##########

<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^";
}