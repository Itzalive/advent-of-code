namespace AdventOfCode2023._2022._20;

public class Day20 : IDay
{
    public int Year => 2022;

    public int Day => 20;

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();

        var list = new List<InitialState>(inputs.Length);
        for (var i = 0; i < inputs.Length; i++)
        {
            list.Add(new InitialState {Value = int.Parse(inputs[i]), OriginalPosition = i});
        }

        MixList(list, input.Length < 100);

        var zeroIndex = list.IndexOf(list.Single(i => i.Value == 0));
        var onethou = list[(zeroIndex + 1000) % list.Count].Value;
        var twothou = list[(zeroIndex + 2000) % list.Count].Value;
        var threethou = list[(zeroIndex + 3000) % list.Count].Value;
        Console.WriteLine($"1000: {onethou}");
        Console.WriteLine($"2000: {twothou}");
        Console.WriteLine($"3000: {threethou}");
        return Task.FromResult((onethou + twothou + threethou).ToString());
    }

    
    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();

        var list = new List<InitialState>(inputs.Length);
        for (var i = 0; i < inputs.Length; i++)
        {
            list.Add(new InitialState {Value = int.Parse(inputs[i]) * (long)811589153, OriginalPosition = i});
        }

        if(inputs.Length < 100)
            Console.WriteLine(string.Join(", ", list.Select(l => l.Value)));
        for (var n = 0; n < 10; n++)
        {
            MixList(list, false);

            Console.WriteLine(inputs.Length < 100 ? string.Join(", ", list.Select(l => l.Value)) : $"Mixed {n} times");
        }

        //Console.WriteLine(string.Join(", ", list.Select(l => l.Value)));
        
        var zeroIndex = list.IndexOf(list.Single(i => i.Value == 0));
        var onethou = list[(zeroIndex + 1000) % list.Count].Value;
        var twothou = list[(zeroIndex + 2000) % list.Count].Value;
        var threethou = list[(zeroIndex + 3000) % list.Count].Value;
        Console.WriteLine($"1000: {onethou}");
        Console.WriteLine($"2000: {twothou}");
        Console.WriteLine($"3000: {threethou}");
        return Task.FromResult((onethou + twothou + threethou).ToString());
    }

    private static void MixList(List<InitialState> list, bool printEachMix)
    {
        var length = list.Count;
        for (var x = 0; x < length; x++)
        {
            var item = list.Single(i => i.OriginalPosition == x);
            var oldIndex = list.IndexOf(item);
            var newIndex = oldIndex + item.Value;

            newIndex %= length - 1;
            while (newIndex <= 0)
                newIndex += length - 1;
            while (newIndex >= length - 1)
                newIndex -= length - 1;
            list.MoveItem(oldIndex, (int) newIndex);

            if(printEachMix)
                Console.WriteLine(string.Join(", ", list.Select(l => l.Value)));
        }
    }

    public class InitialState
    {
        public long Value { get; set; }

        public int OriginalPosition { get; set; }
    }

    public string? TestInput => @"1
2
-3
3
-2
0
4";
}

public static class ListExtensions{
    public static void MoveItem<T>(this List<T> list, int oldIndex, int newIndex)
    {
        var item = list[oldIndex];

        list.RemoveAt(oldIndex);
        
        list.Insert(newIndex, item);
    }
}