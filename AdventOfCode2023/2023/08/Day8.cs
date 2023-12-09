using System;
using System.Numerics;
using System.Runtime.InteropServices.JavaScript;

namespace AdventOfCode2023._2023._08;

public class Day8 : IDay
{
    public int Year => 2023;
    public int Day => 8;

    public string? Part1TestSolution => "2";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var rl = inputs[0].ToCharArray();
        var nodes = inputs[2..].Select(l => new Node(l)).ToDictionary(n => n.Name);
        var currenNode = "AAA";
        var currentMove = 0;
        do
        {
            var rlIndex = currentMove % rl.Length;
            if (rl[rlIndex] == 'R')
            {
                currenNode = nodes[currenNode].Right;
            }
            else
            {
                currenNode = nodes[currenNode].Left;
            }
            currentMove++;
        } while (currenNode != "ZZZ");
        return Task.FromResult(currentMove.ToString());
    }

    public string? Part2TestSolution => "6";


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var rl = inputs[0].ToCharArray();
        var nodes = inputs[2..].Select(l => new Node(l)).ToDictionary(n => n.Name);
        foreach (var node in nodes.Values)
        {
            node.LeftNode = nodes[node.Left];
            node.RightNode = nodes[node.Right];
        }

        var winningCombinations = Enumerable.Range(0, rl.Length).Select(_ => new List<string>()).ToList();
        foreach (var startingNode in nodes.Values)
        {
            var currentNode = startingNode;
            for (var t = 0; t < rl.Length; t++)
            {
                currentNode = rl[t] == 'R' ? currentNode.RightNode : currentNode.LeftNode;
                if (currentNode.IsEnd)
                {
                    winningCombinations[t].Add(startingNode.Name);
                }
            }

            startingNode.OneCycleOn = currentNode;
        }

        Console.WriteLine($"{nodes.Values.Count} => {nodes.Values.Select(n => n.OneCycleOn).Distinct().Count()}");

        long currentCount = 0;
        var currentNodes = nodes.Values.Where(k => k.Name.EndsWith('A')).ToArray();
        var timesToEnd = new List<BigInteger>();
        var cycleTime = new List<List<BigInteger>>();
        foreach (var startingNode in currentNodes)
        {
            var currenNode = startingNode;
            long currentMove = 0;
            do
            {
                var rlIndex = currentMove % rl.Length;
                if (rl[rlIndex] == 'R')
                {
                    currenNode = currenNode.RightNode;
                }
                else
                {
                    currenNode = currenNode.LeftNode;
                }
                currentMove++;
            } while (!currenNode.IsEnd);

            var firstTimeToEnd = currentMove;
            //var firstEnd = currenNode;
            //var firstRlIndex = (currentMove -1) % rl.Length;
            //long rlIndex2 = 0;
            timesToEnd.Add(firstTimeToEnd);
            //var cycleTimes = new List<BigInteger>();
            //{
            //    rlIndex2 = currentMove % rl.Length;
            //    if (rl[rlIndex2] == 'R')
            //    {
            //        currenNode = currenNode.RightNode;
            //    }
            //    else
            //    {
            //        currenNode = currenNode.LeftNode;
            //    }
            //    currentMove++;

            //    if (currenNode.Name == firstEnd.Name)
            //    {
            //        cycleTimes.Add(currentMove - firstTimeToEnd);
            //    }
            //} while (currenNode.Name != firstEnd.Name || firstRlIndex != rlIndex2);

            //cycleTimes.Add(currentMove - firstTimeToEnd);
            //cycleTime.Add(cycleTimes);
        }

        //var sumFirst = timesToEnd[1..].Aggregate<BigInteger, BigInteger>(0, (current, timeToEnd) => current + timeToEnd) - timesToEnd[0] * (timesToEnd.Count - 1);
        //var cycleDivisor = cycleTime[0] * (cycleTime.Count - 1) - cycleTime[1..]
        //    .Aggregate<BigInteger, BigInteger>(0, (current, timeToEnd) => current + timeToEnd);

        BigInteger answer = timesToEnd.Aggregate<BigInteger, BigInteger>(1, lcm);

        //while (true)
        //{
        //    for (var t = 0; t < winningCombinations.Count; t++)
        //    {
        //        if (currentNodes.All(n => winningCombinations[t].Contains(n.Name)))
        //        {
        //            return Task.FromResult((currentCount + t + 1).ToString());
        //        }
        //    }

        //    currentNodes = currentNodes.Select(n => n.OneCycleOn).ToArray();
        //    currentCount += rl.Length;
        //    if((currentCount/rl.Length) % 100000 == 0)
        //        Console.WriteLine($"{currentNodes.Length} in {currentCount}");
        //}
        return Task.FromResult(answer.ToString());
    }

    static BigInteger gcf(BigInteger a, BigInteger b)
    {
        while (b != 0)
        {
            BigInteger temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    static BigInteger lcm(BigInteger a, BigInteger b)
    {
        return (a / gcf(a, b)) * b;
    }

    public class Node
    {
        public string Name { get; set; }

        public string Left { get; set; }

        public string Right { get; set; }

        public bool IsEnd { get; set; }
        public Node LeftNode { get; set; }
        public Node RightNode { get; set; }

        public Node OneCycleOn { get; set; }

        public Node(string s)
        {
            var parts = s.Split(" = ");
            Name = parts[0];
            var rl = parts[1][1..(parts[1].Length - 1)].Split(", ");
            Left = rl[0];
            Right = rl[1];
            IsEnd = Name.EndsWith('Z');
        }
    }

    public string? TestInput => @"LR

11A = (11B, XXX)
11B = (XXX, 11Z)
11Z = (11B, XXX)
22A = (22B, XXX)
22B = (22C, 22C)
22C = (22Z, 22Z)
22Z = (22B, 22B)
XXX = (XXX, XXX)";

    public string? TestInput1 => @"RL

AAA = (BBB, CCC)
BBB = (DDD, EEE)
CCC = (ZZZ, GGG)
DDD = (DDD, DDD)
EEE = (EEE, EEE)
GGG = (GGG, GGG)
ZZZ = (ZZZ, ZZZ)";
}