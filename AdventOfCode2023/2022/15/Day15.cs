using System.Drawing;
using System.Text.RegularExpressions;

namespace AdventOfCode2023._2022._15;

public class Day15 : IDay
{
    public int Year => 2022;

    public int Day => 15;

    public string? Part1TestSolution => null;

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var scoreLineY = int.Parse(inputs[0]);
        var sensedBeacons = new List<(Point, Point)>();
        foreach (var line in inputs.Skip(2))
        {
            var matches = Regex.Matches(line, "x=(-?[0-9]*), y=(-?[0-9]*)");
            var sensor = new Point(int.Parse(matches[0].Groups[1].Value), int.Parse(matches[0].Groups[2].Value));
            var beacon = new Point(int.Parse(matches[1].Groups[1].Value), int.Parse(matches[1].Groups[2].Value));
            sensedBeacons.Add((sensor, beacon));
        }

        var minX = sensedBeacons.Min(s => s.Item1.X - Math.Abs(s.Item2.X - s.Item1.X) -
                                          Math.Abs(s.Item2.Y - s.Item1.Y)) - 2;
        var maxX = sensedBeacons.Max(s => s.Item1.X + Math.Abs(s.Item2.X - s.Item1.X) +
                                          Math.Abs(s.Item2.Y - s.Item1.Y)) + 2;
        var minY = sensedBeacons.Min(s => s.Item1.Y - Math.Abs(s.Item2.X - s.Item1.X) -
                                          Math.Abs(s.Item2.Y - s.Item1.Y)) - 2;
        var maxY = sensedBeacons.Max(s => s.Item1.Y + Math.Abs(s.Item2.X - s.Item1.X) +
                                          Math.Abs(s.Item2.Y - s.Item1.Y)) + 2;

        var scoreLine = new List<char>(maxX - minX + 1);
        for (var x = minX; x < maxX; x++)
        {
            scoreLine.Add('.');
        }

        foreach (var sensedBeacon in sensedBeacons)
        {
            if (sensedBeacon.Item1.Y == scoreLineY)
            {
                scoreLine[sensedBeacon.Item1.X - minX] = 'S';
            }

            if (sensedBeacon.Item2.Y == scoreLineY)
            {
                scoreLine[sensedBeacon.Item2.X - minX] = 'B';
            }

            FillManhattanDistance(scoreLine, scoreLineY - minY,
                new Point(sensedBeacon.Item1.X - minX, sensedBeacon.Item1.Y - minY),
                Math.Abs(sensedBeacon.Item2.X - sensedBeacon.Item1.X) +
                Math.Abs(sensedBeacon.Item2.Y - sensedBeacon.Item1.Y));
        }

        return Task.FromResult(scoreLine.Count(c => c == '#').ToString());
    }

    private void FillManhattanDistance(List<char> scoreLine, int scoreLineY, Point point, int distance)
    {
        var y = scoreLineY;
        for (var x = point.X - distance; x <= point.X + distance; x++)
        {
            if (Math.Abs(x - point.X) + Math.Abs(y - point.Y) > distance) continue;
            if (y == scoreLineY && scoreLine[x] == '.')
            {
                scoreLine[x] = '#';
            }
        }
    }

    private void FillManhattanDistance(List<List<char>> map, Point point, int distance)
    {
        for (var x = point.X - distance; x <= point.X + distance; x++)
        {
            if (x < 0 || x >= map[0].Count) continue;
            for (var y = point.Y - distance; y <= point.Y + distance; y++)
            {
                if (y < 0 || y >= map.Count) continue;
                if (Math.Abs(x - point.X) + Math.Abs(y - point.Y) > distance) continue;
                if (map[y][x] == '.')
                {
                    map[y][x] = '#';
                }
            }
        }
    }

    private void PrintMap(List<List<char>> map)
    {
        foreach (var row in map)
        {
            Console.WriteLine(string.Join("", row));
        }
    }

    public string? Part2TestSolution => null;

    public async Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var maxDimension = int.Parse(inputs[1]);
        var sensedBeacons = new List<(Point, Point, int)>();
        foreach (var line in inputs.Skip(2))
        {
            var matches = Regex.Matches(line, "x=(-?[0-9]*), y=(-?[0-9]*)");
            var sensor = new Point(int.Parse(matches[0].Groups[1].Value), int.Parse(matches[0].Groups[2].Value));
            var beacon = new Point(int.Parse(matches[1].Groups[1].Value), int.Parse(matches[1].Groups[2].Value));
            sensedBeacons.Add((sensor, beacon, Math.Abs(beacon.X - sensor.X) +
                                               Math.Abs(beacon.Y - sensor.Y)));
        }

        var minX = 0;
        var maxX = maxDimension;
        var minY = 0;
        var maxY = maxDimension;

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x < maxX; x++)
            {
                var isScanned = false;
                foreach (var sensedBeacon in sensedBeacons)
                {
                    var distanceToBeacon = sensedBeacon.Item3;
                    var distanceYFromSensor = Math.Abs(y - sensedBeacon.Item1.Y);
                    var distanceFromSensor = Math.Abs(x - sensedBeacon.Item1.X) + distanceYFromSensor;
                    if (distanceFromSensor <= distanceToBeacon)
                    {
                        isScanned = true;
                        x = sensedBeacon.Item1.X + (distanceToBeacon - distanceYFromSensor);
                        break;
                    }
                }

                if (!isScanned)
                    return ((long) x * 4000000 + y).ToString();
            }
        }

        return "";
    }

    public string? TestInput => @"10
20
Sensor at x=2, y=18: closest beacon is at x=-2, y=15
Sensor at x=9, y=16: closest beacon is at x=10, y=16
Sensor at x=13, y=2: closest beacon is at x=15, y=3
Sensor at x=12, y=14: closest beacon is at x=10, y=16
Sensor at x=10, y=20: closest beacon is at x=10, y=16
Sensor at x=14, y=17: closest beacon is at x=10, y=16
Sensor at x=8, y=7: closest beacon is at x=2, y=10
Sensor at x=2, y=0: closest beacon is at x=2, y=10
Sensor at x=0, y=11: closest beacon is at x=2, y=10
Sensor at x=20, y=14: closest beacon is at x=25, y=17
Sensor at x=17, y=20: closest beacon is at x=21, y=22
Sensor at x=16, y=7: closest beacon is at x=15, y=3
Sensor at x=14, y=3: closest beacon is at x=15, y=3
Sensor at x=20, y=1: closest beacon is at x=15, y=3";
}