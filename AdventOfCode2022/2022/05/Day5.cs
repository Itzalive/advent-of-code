namespace AdventOfCode2022._2022._05;

internal class Day5 : IDay
{
    public int Year => 2022;
    public int Day => 5;

    public string Part1(string input)
    {
        var puzzle = input.Split(Environment.NewLine + Environment.NewLine);
        var startingPositions = puzzle[0].Split(Environment.NewLine);
        var stacks = new List<Stack<char>>();
        for (var i = 0; i < (startingPositions.Last().Length + 1) / 4; i++)
        {
            stacks.Add(new Stack<char>());
        }

        for (var j = startingPositions.Length - 2; j >= 0; j--)
        {
            for (var i = 1; i < startingPositions[j].Length; i += 4)
            {
                if (startingPositions[j][i - 1] == '[')
                {
                    stacks[(int) (i / 4)].Push(startingPositions[j][i]);
                }
            }
        }

        var inputs = puzzle[1].Split(Environment.NewLine).ToArray();
        foreach (var line in inputs)
        {
            var splitLine = line.Split(' ');
            for (var i = 0; i < int.Parse(splitLine[1]); i++)
            {
                stacks[int.Parse(splitLine[5]) - 1].Push(stacks[int.Parse(splitLine[3]) - 1].Pop());
            }
        }

        return stacks.Aggregate("", (current, stack) => current + stack.Peek());
    }

    public string Part2(string input)
    {
        var puzzle = input.Split(Environment.NewLine + Environment.NewLine);
        var startingPositions = puzzle[0].Split(Environment.NewLine);
        var stacks = new List<Stack<char>>();
        for (var i = 0; i < (startingPositions.Last().Length + 1) / 4; i++)
        {
            stacks.Add(new Stack<char>());
        }

        for (var j = startingPositions.Length - 2; j >= 0; j--)
        {
            for (var i = 1; i < startingPositions[j].Length; i += 4)
            {
                if (startingPositions[j][i - 1] == '[')
                {
                    stacks[(int) (i / 4)].Push(startingPositions[j][i]);
                }
            }
        }

        var inputs = puzzle[1].Split(Environment.NewLine).ToArray();
        foreach (var line in inputs)
        {
            var splitLine = line.Split(' ');
            var crane = new Stack<char>();
            for (var i = 0; i < int.Parse(splitLine[1]); i++)
            {
                crane.Push(stacks[int.Parse(splitLine[3]) - 1].Pop());
            }

            for (var i = 0; i < int.Parse(splitLine[1]); i++)
            {
                stacks[int.Parse(splitLine[5]) - 1].Push(crane.Pop());
            }
        }

        return stacks.Aggregate("", (current, stack) => current + stack.Peek());
    }

    public string TestInput => @"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2";
}