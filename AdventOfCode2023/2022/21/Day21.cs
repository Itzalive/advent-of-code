namespace AdventOfCode2023._2022._21;

public class Day21 : IDay
{
    public int Year => 2022;

    public int Day => 21;

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var values = new Dictionary<string, long>();
        var queue = new Queue<Monkey>(inputs.Select(v => new Monkey(v)));

        while (queue.Count > 0)
        {
            var monkey = queue.Dequeue();
            if (monkey.Value.HasValue)
            {
                values.Add(monkey.Name, monkey.Value.Value);
            }
            else if (values.ContainsKey(monkey.Operation[0]) && values.ContainsKey(monkey.Operation[2]))
            {
                switch (monkey.Operation[1])
                {
                    case "+":
                        var resultAdd = values[monkey.Operation[0]] + values[monkey.Operation[2]];
                        values.Add(monkey.Name, resultAdd);
                        break;
                    case "-":
                        var resultSubtract = values[monkey.Operation[0]] - values[monkey.Operation[2]];
                        values.Add(monkey.Name, resultSubtract);
                        break;
                    case "*":
                        var resultMltiply = values[monkey.Operation[0]] * values[monkey.Operation[2]];
                        values.Add(monkey.Name, resultMltiply);
                        break;
                    case "/":
                        var resultDivide = values[monkey.Operation[0]] / values[monkey.Operation[2]];
                        values.Add(monkey.Name, resultDivide);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                if (monkey.Name == "root")
                    break;
            }
            else
            {
                queue.Enqueue(monkey);
            }
        }
        return Task.FromResult(values["root"].ToString());
    }

    
    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var values = new Dictionary<string, long>();
        var monkeys = inputs.Select(v => new Monkey(v));
        var queue = new Queue<Monkey>(monkeys.Where(m => m.Name != "root" && m.Name != "humn"));

        var rootMonkey = monkeys.Single(m => m.Name == "root");
        var rootVariables = new[] {rootMonkey.Operation[0], rootMonkey.Operation[2]};
        while (queue.Count > 0)
        {
            var monkey = queue.Dequeue();
            if (monkey.Value.HasValue)
            {
                values.Add(monkey.Name, monkey.Value.Value);
            }
            else if (values.ContainsKey(monkey.Operation[0]) && values.ContainsKey(monkey.Operation[2]))
            {
                switch (monkey.Operation[1])
                {
                    case "+":
                        var resultAdd = values[monkey.Operation[0]] + values[monkey.Operation[2]];
                        values.Add(monkey.Name, resultAdd);
                        break;
                    case "-":
                        var resultSubtract = values[monkey.Operation[0]] - values[monkey.Operation[2]];
                        values.Add(monkey.Name, resultSubtract);
                        break;
                    case "*":
                        var resultMltiply = values[monkey.Operation[0]] * values[monkey.Operation[2]];
                        values.Add(monkey.Name, resultMltiply);
                        break;
                    case "/":
                        var resultDivide = values[monkey.Operation[0]] / values[monkey.Operation[2]];
                        values.Add(monkey.Name, resultDivide);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                if (!rootVariables.Contains(monkey.Name)) continue;
                values.Add(rootVariables.Single(v => v != monkey.Name), values[monkey.Name]);
                break;
            }
            else
            {
                queue.Enqueue(monkey);
            }
        }

        // Reverse operations
        while (queue.Count > 0)
        {
            var monkey = queue.Dequeue();
            if (monkey.Value.HasValue)
            {
                values.Add(monkey.Name, monkey.Value.Value);
            }
            else if (values.ContainsKey(monkey.Name) && values.ContainsKey(monkey.Operation[0]))
            {
                switch (monkey.Operation[1])
                {
                    case "+":
                        var resultAdd = values[monkey.Name] - values[monkey.Operation[0]];
                        values.Add(monkey.Operation[2], resultAdd);
                        break;
                    case "-":
                        var resultSubtract = values[monkey.Operation[0]] - values[monkey.Name];
                        values.Add(monkey.Operation[2], resultSubtract);
                        break;
                    case "*":
                        var resultMltiply = values[monkey.Name] / values[monkey.Operation[0]];
                        values.Add(monkey.Operation[2], resultMltiply);
                        break;
                    case "/":
                        var resultDivide = values[monkey.Operation[0]] / values[monkey.Name];
                        values.Add(monkey.Operation[2], resultDivide);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                if (!rootVariables.Contains(monkey.Name)) continue;
                values.Add(rootVariables.Single(v => v != monkey.Name), values[monkey.Name]);
                break;
            }
            else if (values.ContainsKey(monkey.Name) && values.ContainsKey(monkey.Operation[2]))
            {
                switch (monkey.Operation[1])
                {
                    case "+":
                        var resultAdd = values[monkey.Name] - values[monkey.Operation[2]];
                        values.Add(monkey.Operation[0], resultAdd);
                        break;
                    case "-":
                        var resultSubtract = values[monkey.Name] + values[monkey.Operation[2]];
                        values.Add(monkey.Operation[0], resultSubtract);
                        break;
                    case "*":
                        var resultMltiply = values[monkey.Name] / values[monkey.Operation[2]];
                        values.Add(monkey.Operation[0], resultMltiply);
                        break;
                    case "/":
                        var resultDivide = values[monkey.Name] * values[monkey.Operation[2]];
                        values.Add(monkey.Operation[0], resultDivide);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                if (!values.ContainsKey("humn")) continue;
                break;
            }
            else
            {
                queue.Enqueue(monkey);
            }
        }

        return Task.FromResult(values["humn"].ToString());
    }

    public class Monkey
    {
        public Monkey(string input)
        {
            var data = input.Split(": ");
            Name = data[0];
            if (long.TryParse(data[1], out var result))
            {
                Value = result;
            }
            else
            {
                Operation = data[1].Split(" ").ToArray();
            }
        }

        public string[] Operation { get; set; }

        public long? Value { get; set; }

        public string Name { get; set; }
    }

    public string? TestInput => @"root: pppw + sjmn
dbpl: 5
cczh: sllz + lgvd
zczc: 2
ptdq: humn - dvpt
dvpt: 3
lfqf: 4
humn: 5
ljgn: 2
sjmn: drzm * dbpl
sllz: 4
pppw: cczh / lfqf
lgvd: ljgn * ptdq
drzm: hmdt - zczc
hmdt: 32";
}