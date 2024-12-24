using System.Text.RegularExpressions;

namespace AdventOfCode2023._2023._18;

public class Day18 : IDay
{
    public int Year => 2023;

    public int Day => 18;

    public string? Part1TestSolution => "62";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(r => Regex.Match(r, "([UDLR]) ([0-9]*) \\(#([0-9a-f]{6})\\)"))
            .Select(m => new Order
                { Direction = m.Groups[1].Value, Number = int.Parse(m.Groups[2].Value), Hex = m.Groups[3].Value })
            .ToArray();
        
        var cX = 0;
        var maxX = 0;
        var minX = 0;
        
        var cY = 0;
        var maxY = 0;
        var minY = 0;
        foreach (var order in inputs)
        {
            switch (order.Direction)
            {
                case "R":
                    cX += order.Number;
                    maxX = Math.Max(maxX, cX);
                    break;
                case "L":
                    cX -= order.Number;
                    minX = Math.Min(minX, cX);
                    break;
                case "D":
                    cY += order.Number;
                    maxY = Math.Max(maxY, cY);
                    break;
                case "U":
                    cY -= order.Number;
                    minY = Math.Min(minY, cY);
                    break;
            }
        }

        var rangeY = maxY - minY + 1;
        var rangeX = maxX - minX + 1;
        var map = new bool[rangeY, rangeX];
        var pos = new[] { -minY, -minX };
        foreach (var order in inputs)
        {
            switch (order.Direction)
            {
                case "R":
                    for (var x = 0; x < order.Number; x++)
                    {
                        map[pos[0], pos[1] + x] = true;
                    }
                    pos = new[] { pos[0], pos[1] + order.Number };
                    break;
                case "D":
                    for (var y = 0; y < order.Number; y++)
                    {
                        map[pos[0] + y, pos[1]] = true;
                    }
                    pos = new[] { pos[0] + order.Number, pos[1] };
                    break;
                case "L":
                    for (var x = 0; x < order.Number; x++)
                    {
                        map[pos[0], pos[1] - x] = true;
                    }
                    pos = new[] { pos[0], pos[1] - order.Number };
                    break;
                case "U":
                    for (var y = 0; y < order.Number; y++)
                    {
                        map[pos[0] - y, pos[1]] = true;
                    }
                    
                    pos = new[] { pos[0] - order.Number, pos[1] };
                    break;
            }
        }

        
        var toFill = new Queue<int[]>();
        toFill.Enqueue(new[] { -minY + 1, -minX + 1 });
        var cardinalDirections = new List<int[]>
        {
            new[] { 0, 1 },
            new[] { 0, -1 },
            new[] { 1, 0 },
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
                if (!map[newY, newX])
                {
                    map[newY, newX] = true;

                    toFill.Enqueue(new[] { newY, newX });
                    numFilled++;
                }
            }
        }

        var score = 0;
        for (var y = 0; y < rangeY; y++)
        {
            for (var x = 0; x < rangeX; x++)
            {
                Console.Write(map[y, x] ? '#' : '.');
                if (map[y, x])
                    score++;
            }
            Console.WriteLine();
        }
        return Task.FromResult(score.ToString());
    }

    public string? Part2TestSolution => "952408144115";


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(r => Regex.Match(r, "([UDLR]) ([0-9]*) \\(#([0-9a-f]{6})\\)"))
            .Select(m => new Order
                { Direction = m.Groups[1].Value, Number = int.Parse(m.Groups[2].Value), Hex = m.Groups[3].Value })
            .Select(o => o.FromHex())
            .ToArray();
        
        long cX = 0;
        long maxX = 0;
        long minX = 0;
        
        long cY = 0;
        long maxY = 0;
        long minY = 0;
        foreach (var order in inputs)
        {
            switch (order.Direction)
            {
                case "R":
                    cX += order.Number;
                    maxX = Math.Max(maxX, cX);
                    break;
                case "L":
                    cX -= order.Number;
                    minX = Math.Min(minX, cX);
                    break;
                case "D":
                    cY += order.Number;
                    maxY = Math.Max(maxY, cY);
                    break;
                case "U":
                    cY -= order.Number;
                    minY = Math.Min(minY, cY);
                    break;
            }
        }

        long rangeY = maxY - minY + 1;
        long rangeX = maxX - minX + 1;
        var map = new bool[rangeY, rangeX];
        var pos = new[] { -minY, -minX };
        foreach (var order in inputs)
        {
            switch (order.Direction)
            {
                case "R":
                    for (var x = 0; x < order.Number; x++)
                    {
                        map[pos[0], pos[1] + x] = true;
                    }
                    pos = new[] { pos[0], pos[1] + order.Number };
                    break;
                case "D":
                    for (var y = 0; y < order.Number; y++)
                    {
                        map[pos[0] + y, pos[1]] = true;
                    }
                    pos = new[] { pos[0] + order.Number, pos[1] };
                    break;
                case "L":
                    for (var x = 0; x < order.Number; x++)
                    {
                        map[pos[0], pos[1] - x] = true;
                    }
                    pos = new[] { pos[0], pos[1] - order.Number };
                    break;
                case "U":
                    for (var y = 0; y < order.Number; y++)
                    {
                        map[pos[0] - y, pos[1]] = true;
                    }
                    
                    pos = new[] { pos[0] - order.Number, pos[1] };
                    break;
            }
        }

        
        var toFill = new Queue<long[]>();
        toFill.Enqueue(new[] { -minY + 1, -minX + 1 });
        var cardinalDirections = new List<int[]>
        {
            new[] { 0, 1 },
            new[] { 0, -1 },
            new[] { 1, 0 },
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
                if (!map[newY, newX])
                {
                    map[newY, newX] = true;

                    toFill.Enqueue(new[] { newY, newX });
                    numFilled++;
                }
            }
        }

        long score = 0;
        for (var y = 0; y < rangeY; y++)
        {
            for (var x = 0; x < rangeX; x++)
            {
                Console.Write(map[y, x] ? '#' : '.');
                if (map[y, x])
                    score++;
            }
            Console.WriteLine();
        }
        return Task.FromResult(score.ToString());
    }

    record Order
    {
        public string Direction { get; set; }

        public int Number { get; set; }

        public string Hex { get; set; }

        public Order FromHex()
        {
            return new Order
            {
                Direction = Hex[5] == '0' ? "R" : Hex[5] == '1' ? "D" : Hex[5] == '2' ? "L" : "U",
                Number = int.Parse(Hex[..5], System.Globalization.NumberStyles.HexNumber)
            };
        }
    }

    public string? TestInput => @"R 6 (#70c710)
D 5 (#0dc571)
L 2 (#5713f0)
D 2 (#d2c081)
R 2 (#59c680)
D 2 (#411b91)
L 5 (#8ceee2)
U 2 (#caa173)
L 1 (#1b58a2)
U 2 (#caa171)
R 2 (#7807d2)
U 3 (#a77fa3)
L 2 (#015232)
U 2 (#7a21e3)";
}