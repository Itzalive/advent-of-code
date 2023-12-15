using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2023._2023._15;

public class Day15 : IDay
{
    public int Year => 2023;

    public int Day => 15;

    public string? Part1TestSolution => "1320";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(",").ToArray();
        var result = inputs.Sum(i => HolidayAscii(i));
        return Task.FromResult(result.ToString());
    }

    private int HolidayAscii(string s)
    {
        var current = 0;
        foreach (var character in  Encoding.ASCII.GetBytes(s))
        {
            current += character;
            current *= 17;
            current %= 256;
        }

        return current;
    }

    public string? Part2TestSolution => "145";


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(",").ToArray();
        var boxes = new List<(int, string)>[256];
        for (var i = 0; i<boxes.Length; i++)
        {
            boxes[i] = new List<(int, string)>();
        }

        foreach (var rule in inputs)
        {
            var label = Regex.Match(rule, "^(.*)([=-])([0-9]*)$");
            var box = HolidayAscii(label.Groups[1].Value);
            var operation = label.Groups[2].Value;
            switch (operation)
            {
                case "=":
                    var focalLength = int.Parse(label.Groups[3].Value);;
                    if (boxes[box].All(v => v.Item2 != label.Groups[1].Value))
                    {
                        boxes[box].Add((focalLength, label.Groups[1].Value));
                    }
                    else
                    {
                        var index = boxes[box].FindIndex(v => v.Item2 == label.Groups[1].Value);
                        boxes[box][index] = (focalLength, label.Groups[1].Value);
                    }

                    break;
                case "-":

                    if (boxes[box].Any(v => v.Item2 == label.Groups[1].Value))
                    {
                        var findIndex = boxes[box].FindIndex(v => v.Item2 == label.Groups[1].Value);
                        boxes[box].RemoveAt(findIndex);
                    }

                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        var score = 0;
        for (var i = 0; i < boxes.Length; i++)
        {
            var box = boxes[i];
            for (var slot = 0; slot < box.Count; slot++)
            {
                score += (i + 1) * (slot + 1) * box[slot].Item1;
            }
        }
        return Task.FromResult(score.ToString());
    }

    public string? TestInput => @"rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7";
}