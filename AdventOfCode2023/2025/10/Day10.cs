using System.Collections;

namespace AdventOfCode2023._2025._10;

internal class Day10 : IDay
{
    public int Year => 2025;

    public int Day => 10;

    public string? Part1TestSolution => "7";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.Split(' ')).ToArray();
        var total = 0;
        foreach (var machine in inputs)
        {
            var target = Convert.ToInt32(new String(machine[0].Trim('[', ']').Replace(".", "0").Replace("#", "1").Reverse().ToArray()), 2);
            var buttons = machine[1..^1].Select(b =>
                    Convert.ToInt32(b.Trim('(', ')').Split(',').Select(int.Parse).Aggregate(0, (a, b) => a | (1 << b))))
                .ToArray();
            var possibles = new List<int>(buttons);
            var pressed = 1;
            while (possibles.All(p => p != target))
            {
                possibles = possibles.SelectMany(p => buttons.Select(b => b ^ p)).ToList();
                pressed++;
            }
            total += pressed;
            Console.WriteLine(pressed);
        }

        return Task.FromResult(total.ToString());
    }

    public string? Part2TestSolution => "33";

    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.Split(' ')).ToArray();
        var total = 0;
        foreach (var machine in inputs)
        {
            var joltage = new String(machine[^1].Trim('{', '}').Split(',').Select(int.Parse).Select(i => (char)i)
                .ToArray());
            var buttons = machine[1..^1].Select(b =>
                    b.Trim('(', ')').Split(',').Select(int.Parse).ToArray())
                .ToArray();

            var maxButtonPresses = joltage.Sum();
            var minButtonPresses = joltage.Min();

            for (var buttonPresses = minButtonPresses; buttonPresses < maxButtonPresses; buttonPresses++)
            {
                Enumerable.Range(0, joltage.Length).Select(b => );
            }

            total += pressed;
            Console.WriteLine(pressed);
        }

        return Task.FromResult(total.ToString());
    }

    public string? TestInput => "[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}\r\n[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}\r\n[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}";

    sealed class StructuralComparer<T> : IEqualityComparer<T>
    {
        public static IEqualityComparer<T> Instance { get; } = new StructuralComparer<T>();

        public bool Equals(T x, T y)
            => StructuralComparisons.StructuralEqualityComparer.Equals(x, y);

        public int GetHashCode(T obj)
            => StructuralComparisons.StructuralEqualityComparer.GetHashCode(obj);
    }
}