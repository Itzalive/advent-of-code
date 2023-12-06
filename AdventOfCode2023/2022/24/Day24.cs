namespace AdventOfCode2023._2022._24;

public class Day24 : IDay
{
    public int Year => 2022;

    public int Day => 24;

    public string? Part1TestSolution => null;

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        var mapHeight = inputs.Length;
        var mapWidth = inputs[0].Length;
        var blizzardCycleLength = mapWidth == mapHeight ? mapWidth : (mapWidth - 2) * (mapHeight - 2);
        var blizzardMaps = new List<char[][]>(blizzardCycleLength);
        blizzardMaps.Add(inputs);
        var startX = Array.IndexOf(inputs.First(), '.');
        var endX = Array.IndexOf(inputs.Last(), '.');
        var blizzards = new List<Blizzard>();
        for (var y = 1; y < inputs.Length - 1; y++)
        {
            for (var x = 1; x < inputs[y].Length - 1; x++)
            {
                if (inputs[y][x] == '.') continue;
                blizzards.Add(new Blizzard {X = x, Y = y, Direction = inputs[y][x]});
            }
        }

        for (var i = 1; i < blizzardCycleLength; i++)
        {
            var map = new char[mapHeight][];
            map[0] = inputs[0];
            for (var y = 1; y < inputs.Length - 1; y++)
            {
                map[y] = new char[mapWidth];
                map[y][0] = '#';
                for (var x = 1; x < mapWidth - 1; x++)
                {
                    map[y][x] = '.';
                }
                map[y][mapWidth - 1] = '#';
            }
            map[mapHeight - 1] = inputs[mapHeight - 1];

            foreach (var blizzard in blizzards)
            {
                blizzard.Move(mapWidth, mapHeight);
                if (map[blizzard.Y][blizzard.X] == '.')
                    map[blizzard.Y][blizzard.X] = blizzard.Direction;
                else if (map[blizzard.Y][blizzard.X] is not '>' and not 'v' and not '<' and not '^')
                {
                    map[blizzard.Y][blizzard.X] =
                        (int.Parse(map[blizzard.Y][blizzard.X].ToString()) + 1).ToString().Last();
                }
                else
                {
                    map[blizzard.Y][blizzard.X] = '2';
                }
            }

            blizzardMaps.Add(map);
            if (mapHeight < 10)
            {
                Console.WriteLine(i);
                foreach (var row in map)
                    Console.WriteLine(string.Join("", row));
            }
        }

        var positions = new List<(int X, int Y)> {(startX, 0)};
        var minute = 0;
        while (positions.All(p => p.X != endX || p.Y != mapHeight - 1))
        {
            minute++;
            var map = blizzardMaps[minute % blizzardCycleLength];
            var newPositions = new List<(int X, int Y)>();
            foreach (var position in positions)
            {
                if (map[position.Y][position.X] == '.')
                    newPositions.Add((position.X, position.Y));
                if (position.Y > 0 && map[position.Y - 1][position.X] == '.')
                    newPositions.Add((position.X, position.Y - 1));
                if(position.X > 1 && map[position.Y][position.X - 1] == '.')
                    newPositions.Add((position.X - 1, position.Y));
                if(position.Y < mapHeight - 1 && map[position.Y + 1][position.X] == '.')
                    newPositions.Add((position.X, position.Y + 1));
                if(position.X < mapWidth - 2 && map[position.Y][position.X + 1] == '.')
                    newPositions.Add((position.X + 1, position.Y));
            }

            positions = newPositions.Distinct().ToList();
        }

        return Task.FromResult(minute.ToString());
    }

    public string? Part2TestSolution => null;

    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        var mapHeight = inputs.Length;
        var mapWidth = inputs[0].Length;
        var blizzardCycleLength = mapWidth == mapHeight ? mapWidth : (mapWidth - 2) * (mapHeight - 2);
        var blizzardMaps = new List<char[][]>(blizzardCycleLength);
        blizzardMaps.Add(inputs);
        var startX = Array.IndexOf(inputs.First(), '.');
        var endX = Array.IndexOf(inputs.Last(), '.');
        var blizzards = new List<Blizzard>();
        for (var y = 1; y < inputs.Length - 1; y++)
        {
            for (var x = 1; x < inputs[y].Length - 1; x++)
            {
                if (inputs[y][x] == '.') continue;
                blizzards.Add(new Blizzard {X = x, Y = y, Direction = inputs[y][x]});
            }
        }

        for (var i = 1; i < blizzardCycleLength; i++)
        {
            var map = new char[mapHeight][];
            map[0] = inputs[0];
            for (var y = 1; y < inputs.Length - 1; y++)
            {
                map[y] = new char[mapWidth];
                map[y][0] = '#';
                for (var x = 1; x < mapWidth - 1; x++)
                {
                    map[y][x] = '.';
                }
                map[y][mapWidth - 1] = '#';
            }
            map[mapHeight - 1] = inputs[mapHeight - 1];

            foreach (var blizzard in blizzards)
            {
                blizzard.Move(mapWidth, mapHeight);
                if (map[blizzard.Y][blizzard.X] == '.')
                    map[blizzard.Y][blizzard.X] = blizzard.Direction;
                else if (map[blizzard.Y][blizzard.X] is not '>' and not 'v' and not '<' and not '^')
                {
                    map[blizzard.Y][blizzard.X] =
                        (int.Parse(map[blizzard.Y][blizzard.X].ToString()) + 1).ToString().Last();
                }
                else
                {
                    map[blizzard.Y][blizzard.X] = '2';
                }
            }

            blizzardMaps.Add(map);
            if (mapHeight < 10)
            {
                Console.WriteLine(i);
                foreach (var row in map)
                    Console.WriteLine(string.Join("", row));
            }
        }

        var positions = new List<(int X, int Y)> {(startX, 0)};
        var minute = 0;
        while (positions.All(p => p.X != endX || p.Y != mapHeight - 1))
        {
            minute++;
            var map = blizzardMaps[minute % blizzardCycleLength];
            var newPositions = new List<(int X, int Y)>();
            foreach (var position in positions)
            {
                if (map[position.Y][position.X] == '.')
                    newPositions.Add((position.X, position.Y));
                if (position.Y > 0 && map[position.Y - 1][position.X] == '.')
                    newPositions.Add((position.X, position.Y - 1));
                if(position.X > 1 && map[position.Y][position.X - 1] == '.')
                    newPositions.Add((position.X - 1, position.Y));
                if(position.Y < mapHeight - 1 && map[position.Y + 1][position.X] == '.')
                    newPositions.Add((position.X, position.Y + 1));
                if(position.X < mapWidth - 2 && map[position.Y][position.X + 1] == '.')
                    newPositions.Add((position.X + 1, position.Y));
            }

            positions = newPositions.Distinct().ToList();
        }

        positions = new List<(int X, int Y)>{(endX, mapHeight - 1)};
        while (positions.All(p => p.X != startX || p.Y != 0))
        {
            minute++;
            var map = blizzardMaps[minute % blizzardCycleLength];
            var newPositions = new List<(int X, int Y)>();
            foreach (var position in positions)
            {
                if (map[position.Y][position.X] == '.')
                    newPositions.Add((position.X, position.Y));
                if (position.Y > 0 && map[position.Y - 1][position.X] == '.')
                    newPositions.Add((position.X, position.Y - 1));
                if(position.X > 1 && map[position.Y][position.X - 1] == '.')
                    newPositions.Add((position.X - 1, position.Y));
                if(position.Y < mapHeight - 1 && map[position.Y + 1][position.X] == '.')
                    newPositions.Add((position.X, position.Y + 1));
                if(position.X < mapWidth - 2 && map[position.Y][position.X + 1] == '.')
                    newPositions.Add((position.X + 1, position.Y));
            }

            positions = newPositions.Distinct().ToList();
        }
        
        positions = new List<(int X, int Y)>{(startX, 0)};
        while (positions.All(p => p.X != endX || p.Y != mapHeight - 1))
        {
            minute++;
            var map = blizzardMaps[minute % blizzardCycleLength];
            var newPositions = new List<(int X, int Y)>();
            foreach (var position in positions)
            {
                if (map[position.Y][position.X] == '.')
                    newPositions.Add((position.X, position.Y));
                if (position.Y > 0 && map[position.Y - 1][position.X] == '.')
                    newPositions.Add((position.X, position.Y - 1));
                if(position.X > 1 && map[position.Y][position.X - 1] == '.')
                    newPositions.Add((position.X - 1, position.Y));
                if(position.Y < mapHeight - 1 && map[position.Y + 1][position.X] == '.')
                    newPositions.Add((position.X, position.Y + 1));
                if(position.X < mapWidth - 2 && map[position.Y][position.X + 1] == '.')
                    newPositions.Add((position.X + 1, position.Y));
            }

            positions = newPositions.Distinct().ToList();
        }

        return Task.FromResult(minute.ToString());
    }

    public class Blizzard
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char Direction { get; set; }

        public void Move(int maxX, int maxY)
        {
            switch (Direction)
            {
                case '>':
                    X++;
                    if (X == maxX - 1)
                    {
                        X = 1;
                    }
                    break;
                case 'v':
                    Y++;
                    if (Y == maxY - 1)
                    {
                        Y = 1;
                    }
                    break;
                case '<':
                    X--;
                    if (X == 0)
                    {
                        X = maxX - 2;
                    }
                    break;
                case '^':
                    Y--;
                    if (Y == 0)
                    {
                        Y = maxY - 2;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Direction));
            }
        }
    }

    public string? TestInput => @"#.######
#>>.<^<#
#.<..<<#
#>v.><>#
#<^v^^>#
######.#";
}