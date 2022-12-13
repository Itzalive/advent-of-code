using System.Drawing;

namespace AdventOfCode2022._2022._09;

public class Day9 : IDay
{
    public int Year => 2022;
    public int Day => 9;

    public string Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();

        var head = new MutablePoint();
        var tail = new MutablePoint();
        var tailHistory = new HashSet<Point> {new(tail.X, tail.Y)};
        foreach (var line in inputs)
        {
            var xDir = 0;
            var yDir = 0;
            var values = line.Split(" ");
            switch (values[0])
            {
                case "L":
                    xDir = -1;
                    break;
                case "R":
                    xDir = 1;
                    break;
                case "U":
                    yDir = 1;
                    break;
                case "D":
                    yDir = -1;
                    break;
            }

            head.X += xDir * int.Parse(values[1]);
            head.Y += yDir * int.Parse(values[1]);

            while (!IsTouching(head, tail))
            {
                tail.X += Math.Clamp(head.X - tail.X, -1, 1);
                tail.Y += Math.Clamp(head.Y - tail.Y, -1, 1);
                tailHistory.Add(new Point(tail.X, tail.Y));
            }
        }

        return tailHistory.Count.ToString();
    }

    private static bool IsTouching(MutablePoint head, MutablePoint tail)
    {
        return Math.Sqrt(Math.Pow(head.X - tail.X, 2) + Math.Pow(head.Y - tail.Y, 2)) < 2;
    }

    public string Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();

        var rope = new List<MutablePoint>();
        for (var i = 0; i < 10; i++)
        {
            rope.Add(new MutablePoint());
        }

        var tailHistory = new HashSet<MutablePoint> {new(rope.Last().X, rope.Last().Y)};
        foreach (var line in inputs)
        {
            var xDir = 0;
            var yDir = 0;
            var values = line.Split(" ");
            switch (values[0])
            {
                case "L":
                    xDir = -1;
                    break;
                case "R":
                    xDir = 1;
                    break;
                case "U":
                    yDir = 1;
                    break;
                case "D":
                    yDir = -1;
                    break;
            }

            for (var j = 0; j < int.Parse(values[1]); j++)
            {
                rope[0].X += xDir;
                rope[0].Y += yDir;

                for (var i = 1; i < rope.Count; i++)
                    while (!IsTouching(rope[i - 1], rope[i]))
                    {
                        rope[i].X += Math.Clamp(rope[i - 1].X - rope[i].X, -1, 1);
                        rope[i].Y += Math.Clamp(rope[i - 1].Y - rope[i].Y, -1, 1);
                        if (i == rope.Count - 1)
                            tailHistory.Add(new MutablePoint(rope[i].X, rope[i].Y));
                    }
                //PrintRope(rope, 6);
            }
        }

        //for (var y = 60; y > -60; y--)
        //{
        //    for (var x = -60; x < 60; x++)
        //    {
        //        var display = ".";
        //        if (tailHistory.Any(t => t.X == x && t.Y == y))
        //        {
        //            display = "#";
        //        }

        //        Console.Write(display);
        //    }

        //    Console.WriteLine();
        //}

        //Console.WriteLine();

        return tailHistory.Count.ToString();
    }

    private static void PrintRope(IReadOnlyList<MutablePoint> rope, int boardSize = 18)
    {
        for (var y = boardSize; y > -boardSize; y--)
        {
            for (var x = -boardSize; x < boardSize; x++)
            {
                var display = ".";
                for (var i = 0; i < rope.Count; i++)
                {
                    if (rope[i].X != x || rope[i].Y != y) continue;
                    display = i.ToString();
                    break;
                }

                Console.Write(display);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    public record MutablePoint
    {
        public int X { get; set; }

        public int Y { get; set; }

        public MutablePoint()
        {
        }

        public MutablePoint(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    public string TestInput => @"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2";

    public string TestInput2 => @"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20";
}