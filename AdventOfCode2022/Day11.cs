﻿namespace AdventOfCode2022;

public class Day11
{
    public static void Part1(Monkey[] input)
    {
        Console.WriteLine();
        Console.WriteLine("Day11 Part1");

        for (var n = 0; n < 20; n++)
        {
            foreach (var monkey in input)
            {
                while (monkey.Items.Count > 0)
                {
                    monkey.InspectNext(input);
                }
            }

            for(var i = 0; i < input.Length; i++)
            {
                Console.WriteLine($"Monkey {i}: {string.Join(", ", input[i].Items.ToArray().Select(i => i.ToString()))}");
            }
            Console.WriteLine();
        }

        foreach (var monkey in input)
        {
            Console.WriteLine(monkey.ItemsSeen);
        }

        var seen = input.Select(m => m.ItemsSeen).OrderByDescending(e => e).ToArray();
        Console.WriteLine(seen[0] * seen[1]);
    }

    public static void Part2(Monkey[] input)
    {
        Console.WriteLine();
        Console.WriteLine("Day11 Part2");

        for (var n = 0; n < 10000; n++)
        {
            foreach (var monkey in input)
            {
                while (monkey.Items.Count > 0)
                {
                    monkey.InspectNext(input);
                }
            }
        }

        foreach (var monkey in input)
        {
            Console.WriteLine(monkey.ItemsSeen);
        }

        var seen = input.Select(m => m.ItemsSeen).OrderByDescending(e => e).ToArray();
        Console.WriteLine(seen[0] * seen[1]);
    }

    public class Monkey
    {
        public Queue<long> Items { get; set; } = new Queue<long>();

        public long ItemsSeen { get; set; }

        public long Test { get; set; }

        public Func<long, long> Operation { get; set; }

        public int FalseMonkey { get; set; }

        public int TrueMonkey { get; set; }

        public Monkey(params long[] items)
        {
            foreach (var item in items)
            {
                Items.Enqueue(item);
            }
        }

        public void InspectNext(Monkey[] monkeys)
        {
            var item = Items.Dequeue();
            ItemsSeen++;
            item = Operation(item);
            var testEasy = monkeys.Aggregate((long)1, (o, m) => o * m.Test);
            item %= testEasy;
            //item = (int) Math.Floor((double) Operation(item) / 3);
            if (item % Test == 0)
            {
                monkeys[TrueMonkey].Items.Enqueue(item);
            }
            else
            {
                monkeys[FalseMonkey].Items.Enqueue(item);
            }
        }
    }

    public static Monkey[] TestInput = new[]
    {
        new Monkey(79, 98)
        {
            Operation = o => o * 19,
            Test = 23,
            TrueMonkey = 2,
            FalseMonkey = 3
        },

        new Monkey(54, 65, 75, 74)
        {
            Operation = o => o + 6,
            Test = 19,
            TrueMonkey = 2,
            FalseMonkey = 0
        },

        new Monkey(79, 60, 97)
        {
            Operation = o => o * o,
            Test = 13,
            TrueMonkey = 1,
            FalseMonkey = 3
        },

        new Monkey(74)
        {
            Operation = o => o + 3,
            Test = 17,
            TrueMonkey = 0,
            FalseMonkey = 1
        },
    };

    
    public static Monkey[] Input = new[]
    {
        new Monkey(80)
        {
            Operation = o => o * 5,
            Test = 2,
            TrueMonkey = 4,
            FalseMonkey = 3
        },

        new Monkey(75, 83, 74)
        {
            Operation = o => o + 7,
            Test = 7,
            TrueMonkey = 5,
            FalseMonkey = 6
        },

        new Monkey(86, 67, 61, 96, 52, 63, 73)
        {
            Operation = o => o + 5,
            Test = 3,
            TrueMonkey = 7,
            FalseMonkey = 0
        },

        new Monkey(85, 83, 55, 85, 57, 70, 85, 52)
        {
            Operation = o => o + 8,
            Test = 17,
            TrueMonkey = 1,
            FalseMonkey = 5
        },

        new Monkey(67, 75, 91, 72, 89)
        {
            Operation = o => o + 4,
            Test = 11,
            TrueMonkey = 3,
            FalseMonkey = 1
        },

        new Monkey(66, 64, 68, 92, 68, 77)
        {
            Operation = o => o * 2,
            Test = 19,
            TrueMonkey = 6,
            FalseMonkey = 2
        },

        new Monkey(97, 94, 79, 88)
        {
            Operation = o => o * o,
            Test = 5,
            TrueMonkey = 2,
            FalseMonkey = 7
        },

        new Monkey(77, 85)
        {
            Operation = o => o + 6,
            Test = 13,
            TrueMonkey = 4,
            FalseMonkey = 0
        },
    };

    //public static string TestInput = @"";

    //public static string Input = @"";
}