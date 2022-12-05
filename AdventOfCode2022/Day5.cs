namespace AdventOfCode2022;

internal class Day5
{
    public static void Part1(string input)
    {
        Console.WriteLine();
        Console.WriteLine("Day5 Part1");

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

        foreach (var stack in stacks)
        {
            Console.Write(stack.Peek());
        }
        Console.WriteLine();
    }

    public static void Part2(string input)
    {
        Console.WriteLine();
        Console.WriteLine("Day5 Part2");
        
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

        foreach (var stack in stacks)
        {
            Console.Write(stack.Peek());
        }
        Console.WriteLine();
    }

    public static string TestInput = @"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2";

    public static string Input = @"[M]                     [N] [Z]    
[F]             [R] [Z] [C] [C]    
[C]     [V]     [L] [N] [G] [V]    
[W]     [L]     [T] [H] [V] [F] [H]
[T]     [T] [W] [F] [B] [P] [J] [L]
[D] [L] [H] [J] [C] [G] [S] [R] [M]
[L] [B] [C] [P] [S] [D] [M] [Q] [P]
[B] [N] [J] [S] [Z] [W] [F] [W] [R]
 1   2   3   4   5   6   7   8   9 

move 5 from 3 to 6
move 2 from 2 to 5
move 1 from 9 to 1
move 1 from 3 to 1
move 5 from 7 to 5
move 2 from 9 to 8
move 1 from 2 to 8
move 1 from 4 to 2
move 8 from 1 to 6
move 4 from 6 to 9
move 1 from 2 to 1
move 2 from 4 to 8
move 2 from 8 to 4
move 3 from 7 to 5
move 6 from 5 to 3
move 1 from 1 to 8
move 1 from 5 to 7
move 5 from 6 to 9
move 3 from 5 to 8
move 2 from 4 to 3
move 1 from 7 to 8
move 2 from 8 to 6
move 2 from 1 to 8
move 8 from 3 to 8
move 11 from 6 to 3
move 1 from 4 to 7
move 1 from 3 to 7
move 2 from 6 to 1
move 7 from 9 to 7
move 10 from 3 to 5
move 1 from 9 to 3
move 2 from 9 to 5
move 5 from 5 to 2
move 19 from 8 to 6
move 1 from 9 to 6
move 1 from 3 to 8
move 4 from 2 to 6
move 1 from 1 to 4
move 5 from 8 to 9
move 1 from 2 to 1
move 6 from 7 to 2
move 3 from 5 to 8
move 3 from 8 to 1
move 2 from 9 to 6
move 1 from 7 to 8
move 6 from 2 to 7
move 1 from 4 to 8
move 3 from 8 to 4
move 2 from 1 to 5
move 7 from 7 to 6
move 1 from 7 to 2
move 3 from 4 to 6
move 2 from 9 to 2
move 1 from 1 to 8
move 2 from 1 to 3
move 1 from 8 to 7
move 3 from 2 to 5
move 5 from 5 to 8
move 4 from 5 to 3
move 1 from 7 to 8
move 2 from 8 to 1
move 1 from 8 to 5
move 5 from 3 to 5
move 13 from 5 to 1
move 1 from 3 to 4
move 2 from 8 to 3
move 3 from 1 to 4
move 1 from 3 to 1
move 1 from 8 to 1
move 5 from 1 to 9
move 1 from 3 to 7
move 2 from 9 to 6
move 2 from 1 to 7
move 3 from 1 to 5
move 3 from 1 to 5
move 1 from 6 to 1
move 4 from 4 to 3
move 3 from 9 to 1
move 5 from 1 to 7
move 7 from 7 to 8
move 1 from 3 to 9
move 28 from 6 to 8
move 5 from 5 to 9
move 6 from 6 to 1
move 4 from 1 to 8
move 5 from 9 to 1
move 12 from 8 to 7
move 1 from 3 to 8
move 6 from 1 to 4
move 5 from 4 to 1
move 3 from 6 to 4
move 2 from 3 to 4
move 3 from 1 to 5
move 6 from 7 to 1
move 2 from 4 to 9
move 2 from 5 to 4
move 19 from 8 to 1
move 4 from 9 to 5
move 5 from 4 to 3
move 4 from 1 to 4
move 5 from 5 to 1
move 3 from 8 to 5
move 7 from 7 to 3
move 14 from 1 to 8
move 5 from 4 to 2
move 12 from 8 to 7
move 1 from 3 to 6
move 3 from 5 to 9
move 1 from 7 to 8
move 8 from 1 to 2
move 5 from 1 to 2
move 9 from 3 to 4
move 8 from 4 to 6
move 2 from 1 to 9
move 3 from 6 to 1
move 5 from 6 to 7
move 14 from 7 to 1
move 1 from 4 to 7
move 6 from 8 to 2
move 14 from 1 to 4
move 13 from 4 to 9
move 2 from 3 to 5
move 3 from 1 to 7
move 1 from 8 to 4
move 1 from 4 to 1
move 1 from 1 to 3
move 1 from 3 to 4
move 1 from 4 to 1
move 1 from 6 to 9
move 1 from 7 to 6
move 1 from 4 to 5
move 11 from 9 to 3
move 6 from 3 to 8
move 5 from 3 to 1
move 2 from 8 to 4
move 1 from 6 to 2
move 7 from 9 to 2
move 1 from 7 to 2
move 1 from 9 to 8
move 2 from 8 to 6
move 30 from 2 to 3
move 2 from 7 to 2
move 2 from 8 to 2
move 3 from 8 to 7
move 6 from 2 to 5
move 1 from 2 to 5
move 3 from 1 to 8
move 2 from 6 to 7
move 1 from 1 to 9
move 1 from 9 to 3
move 7 from 3 to 1
move 6 from 7 to 8
move 8 from 3 to 9
move 7 from 9 to 1
move 1 from 5 to 8
move 7 from 5 to 9
move 2 from 4 to 2
move 11 from 3 to 6
move 2 from 2 to 7
move 11 from 1 to 8
move 2 from 5 to 4
move 11 from 6 to 4
move 12 from 4 to 9
move 4 from 1 to 5
move 3 from 7 to 9
move 12 from 8 to 4
move 1 from 1 to 7
move 6 from 8 to 3
move 2 from 3 to 5
move 3 from 8 to 4
move 3 from 3 to 7
move 9 from 9 to 7
move 5 from 3 to 9
move 1 from 3 to 2
move 13 from 7 to 5
move 1 from 2 to 6
move 1 from 6 to 1
move 1 from 1 to 6
move 16 from 4 to 5
move 1 from 5 to 6
move 16 from 5 to 4
move 13 from 4 to 5
move 3 from 4 to 2
move 1 from 6 to 7
move 3 from 2 to 1
move 8 from 5 to 2
move 3 from 1 to 4
move 1 from 7 to 9
move 14 from 5 to 1
move 10 from 1 to 5
move 1 from 2 to 8
move 19 from 9 to 1
move 1 from 9 to 1
move 6 from 2 to 7
move 4 from 1 to 7
move 1 from 8 to 6
move 16 from 5 to 3
move 1 from 5 to 4
move 2 from 5 to 2
move 1 from 5 to 6
move 1 from 6 to 5
move 1 from 2 to 4
move 7 from 7 to 2
move 4 from 4 to 7
move 2 from 6 to 2
move 8 from 2 to 9
move 4 from 9 to 2
move 16 from 3 to 7
move 4 from 9 to 7
move 14 from 1 to 3
move 26 from 7 to 8
move 1 from 5 to 4
move 20 from 8 to 4
move 5 from 1 to 8
move 2 from 4 to 6
move 4 from 3 to 2
move 1 from 6 to 5
move 8 from 2 to 4
move 1 from 6 to 5
move 1 from 7 to 8
move 8 from 3 to 1
move 6 from 1 to 9
move 1 from 3 to 6
move 14 from 4 to 1
move 1 from 3 to 8
move 2 from 2 to 1
move 1 from 6 to 8
move 1 from 2 to 8
move 5 from 8 to 1
move 2 from 1 to 6
move 2 from 5 to 9
move 1 from 6 to 3
move 1 from 6 to 1
move 5 from 9 to 2
move 5 from 4 to 1
move 5 from 4 to 2
move 16 from 1 to 8
move 9 from 1 to 4
move 24 from 8 to 6
move 1 from 8 to 7
move 7 from 6 to 5
move 1 from 3 to 4
move 3 from 1 to 8
move 3 from 5 to 8
move 10 from 4 to 8
move 3 from 4 to 6
move 1 from 7 to 4
move 20 from 6 to 7
move 1 from 4 to 9
move 1 from 4 to 9
move 7 from 2 to 3
move 13 from 8 to 9
move 4 from 5 to 9
move 4 from 8 to 5
move 18 from 9 to 2
move 14 from 7 to 5
move 6 from 3 to 8
move 1 from 3 to 2
move 1 from 8 to 6
move 4 from 8 to 2
move 1 from 2 to 3
move 17 from 5 to 3
move 18 from 3 to 5
move 6 from 7 to 2
move 3 from 9 to 7
move 1 from 8 to 6
move 5 from 2 to 5
move 26 from 2 to 7
move 1 from 6 to 9
move 29 from 7 to 9
move 15 from 5 to 2
move 1 from 6 to 7
move 8 from 9 to 2
move 14 from 2 to 6
move 16 from 9 to 1
move 6 from 9 to 1
move 1 from 7 to 1
move 3 from 2 to 1
move 5 from 2 to 6
move 15 from 1 to 4
move 1 from 2 to 8
move 1 from 9 to 7
move 1 from 8 to 6
move 19 from 6 to 7
move 10 from 1 to 8
move 4 from 8 to 3
move 1 from 7 to 5
move 3 from 5 to 3
move 13 from 7 to 6
move 2 from 8 to 9
move 7 from 3 to 6
move 5 from 5 to 3
move 1 from 1 to 6
move 2 from 5 to 1
move 4 from 4 to 8
move 7 from 8 to 7
move 8 from 7 to 3
move 1 from 8 to 4
move 2 from 9 to 2
move 8 from 6 to 5
move 1 from 4 to 5
move 4 from 5 to 4
move 2 from 2 to 8
move 9 from 4 to 5
move 2 from 1 to 9
move 2 from 8 to 9
move 14 from 6 to 4
move 5 from 3 to 4
move 3 from 9 to 7
move 3 from 5 to 3
move 2 from 4 to 8
move 2 from 4 to 7
move 2 from 8 to 9
move 4 from 5 to 8
move 16 from 4 to 6
move 1 from 9 to 6
move 3 from 7 to 5
move 7 from 7 to 5
move 10 from 5 to 1
move 6 from 3 to 8
move 2 from 9 to 3
move 3 from 6 to 9
move 3 from 3 to 6
move 2 from 1 to 7
move 13 from 6 to 2
move 2 from 4 to 5
move 2 from 7 to 6
move 2 from 6 to 7
move 2 from 4 to 1
move 3 from 9 to 5
move 1 from 1 to 4
move 3 from 2 to 5
move 2 from 4 to 1
move 2 from 3 to 2
move 5 from 8 to 5
move 1 from 7 to 2
move 1 from 7 to 1
move 1 from 3 to 5
move 1 from 8 to 7
move 1 from 6 to 7
move 1 from 3 to 5
move 12 from 5 to 6
move 6 from 6 to 2
move 1 from 7 to 4
move 1 from 5 to 7
move 2 from 8 to 9
move 1 from 9 to 6
move 1 from 8 to 9
move 5 from 6 to 9
move 1 from 8 to 1
move 14 from 2 to 4
move 1 from 7 to 1
move 1 from 7 to 2
move 3 from 2 to 3
move 2 from 3 to 4
move 1 from 2 to 4
move 4 from 6 to 2
move 8 from 5 to 8
move 15 from 4 to 8
move 3 from 4 to 8
move 7 from 8 to 4
move 6 from 1 to 3
move 1 from 6 to 1
move 5 from 4 to 8
move 7 from 9 to 1
move 1 from 5 to 6
move 4 from 2 to 6
move 10 from 1 to 8
move 29 from 8 to 3
move 1 from 4 to 5
move 1 from 4 to 6
move 6 from 1 to 4
move 1 from 5 to 8
move 3 from 4 to 2
move 27 from 3 to 7
move 18 from 7 to 9
move 5 from 6 to 3
move 7 from 7 to 4
move 1 from 7 to 8
move 9 from 3 to 5
move 5 from 3 to 6
move 3 from 4 to 2
move 1 from 7 to 2
move 2 from 8 to 4
move 2 from 8 to 6
move 2 from 8 to 6
move 8 from 2 to 1
move 7 from 5 to 4
move 1 from 8 to 9
move 4 from 1 to 5
move 1 from 2 to 9
move 8 from 6 to 3
move 3 from 1 to 8
move 1 from 1 to 7
move 8 from 3 to 6
move 2 from 8 to 3
move 1 from 3 to 6
move 4 from 6 to 7
move 16 from 4 to 2
move 1 from 3 to 5
move 2 from 6 to 4
move 1 from 2 to 3
move 2 from 7 to 3
move 2 from 7 to 8
move 3 from 6 to 7
move 4 from 5 to 2
move 2 from 4 to 2
move 4 from 9 to 8
move 3 from 5 to 1
move 3 from 1 to 6
move 6 from 9 to 1
move 4 from 7 to 9
move 8 from 9 to 5
move 4 from 5 to 2
move 7 from 8 to 6
move 11 from 6 to 8
move 4 from 1 to 2
move 3 from 8 to 9
move 5 from 8 to 7
move 2 from 1 to 6
move 4 from 5 to 6
move 2 from 7 to 9
move 2 from 7 to 3
move 5 from 6 to 2
move 4 from 3 to 1
move 1 from 7 to 2
move 1 from 3 to 2
move 2 from 6 to 7
move 1 from 1 to 6
move 6 from 9 to 6
move 1 from 7 to 6
move 1 from 7 to 6
move 2 from 1 to 7
move 2 from 8 to 6
move 4 from 9 to 2
move 17 from 2 to 6
move 1 from 9 to 4
move 1 from 1 to 3
move 1 from 4 to 1
move 20 from 2 to 8
move 2 from 7 to 6
move 2 from 2 to 5
move 1 from 3 to 1
move 1 from 2 to 5
move 6 from 8 to 6
move 2 from 5 to 6
move 3 from 6 to 4
move 1 from 1 to 4
move 15 from 8 to 2
move 11 from 2 to 9
move 1 from 1 to 3
move 10 from 9 to 4
move 1 from 9 to 8
move 12 from 6 to 3
move 1 from 8 to 7
move 1 from 5 to 4
move 8 from 4 to 7
move 5 from 3 to 4
move 7 from 6 to 4
move 3 from 3 to 6
move 3 from 3 to 2
move 1 from 3 to 6
move 17 from 4 to 3
move 1 from 3 to 4
move 2 from 4 to 9
move 14 from 3 to 6
move 2 from 2 to 7
move 1 from 4 to 9
move 8 from 7 to 6
move 1 from 3 to 4
move 9 from 6 to 2
move 1 from 4 to 2
move 26 from 6 to 2
move 27 from 2 to 6
move 10 from 2 to 4
move 1 from 7 to 6
move 28 from 6 to 2
move 21 from 2 to 4
move 2 from 6 to 7
move 3 from 2 to 1
move 5 from 6 to 5
move 3 from 5 to 2
move 1 from 7 to 4
move 11 from 2 to 4
move 21 from 4 to 9
move 1 from 5 to 8
move 1 from 8 to 6
move 18 from 9 to 7
move 1 from 5 to 7
move 3 from 9 to 8
move 1 from 6 to 7
move 1 from 3 to 5
move 1 from 8 to 3
move 22 from 7 to 5
move 13 from 5 to 1
move 16 from 4 to 5
move 3 from 1 to 4
move 2 from 3 to 9
move 3 from 9 to 7
move 6 from 4 to 6
move 1 from 4 to 2
move 2 from 7 to 3";
}