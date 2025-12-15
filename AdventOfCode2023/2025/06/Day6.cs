namespace AdventOfCode2023._2025._06;

internal class Day6 : IDay
{
    public int Year => 2025;

    public int Day => 6;

    public string? Part1TestSolution => "4277556";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();

        var numbers = inputs[..^1]
            .Select(l => l.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()).ToArray();
        var operations = inputs[^1].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();
        var grandTotal = 0L;
        for (var i = 0; i < operations.Length; i++)
        {
            var calculation = 0L;
            for (var j = 0; j < numbers.Length; j++)
            {
                if (operations[i] == "*")
                {
                    if (j == 0)
                    {
                        calculation = 1;
                    }
                    calculation *= numbers[j][i];
                }
                else if (operations[i] == "+")
                {
                    calculation += numbers[j][i];
                }
            }

            grandTotal += calculation;
        }

        return Task.FromResult(grandTotal.ToString());
    }

    public string? Part2TestSolution => "3263827";

    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var operations = inputs[^1].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();
        int rows = inputs.Length - 1;
        int cols = inputs[0].Length;
        var pivotedInput = Enumerable.Range(0, cols)
            .Select(c =>
                new string(Enumerable.Range(0, rows)
                    .Select(r => inputs[r][c]).ToArray())
            )
            .ToArray();
        var pivotedInputString = string.Join("\r\n", pivotedInput.Select(l => string.IsNullOrWhiteSpace(l) ? "" : l));
        var pivotedInputs = pivotedInputString.Split(Environment.NewLine + Environment.NewLine).ToArray();

        var grandTotal = 0L;

        for (var i = 0; i < pivotedInputs.Length; i++)
        {
            var problem = pivotedInputs[i];
            var operation = operations[i];
            var numbers = problem.Split(Environment.NewLine).Select(l => int.Parse(l.Trim())).ToArray();
            var calculation = 0L;
            for (var j = 0; j < numbers.Length; j++)
            {
                if (operation == "*")
                {
                    if (j == 0)
                    {
                        calculation = 1;
                    }
                    calculation *= numbers[j];
                }
                else if (operation == "+")
                {
                    calculation += numbers[j];
                }
            }

            grandTotal += calculation;
        }

        return Task.FromResult(grandTotal.ToString());
    }

    public string? TestInput => "123 328  51 64 \r\n 45 64  387 23 \r\n  6 98  215 314\r\n*   +   *   + ";


}