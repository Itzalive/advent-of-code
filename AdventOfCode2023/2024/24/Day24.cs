using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace AdventOfCode2023._2024._24;

public class Day24 : IDay
{
    public int Year => 2024;

    public int Day => 24;

    public string? Part1TestSolution => "2024";

    public Task<string> Part1(string input)
    {
        var splitInput = input.Split(Environment.NewLine + Environment.NewLine).ToArray();
        var inputInputs = splitInput[0].Split(Environment.NewLine).ToArray();
        var inputGates = splitInput[1].Split(Environment.NewLine).ToArray();

        var inputs = new ConcurrentDictionary<string, Wire>();
        var gates = new List<Gate>();

        foreach (var gate in inputGates)
        {
            var match = Regex.Match(gate, "([a-z0-9]+) (OR|AND|XOR) ([a-z0-9]+) -> ([a-z0-9]+)");
            var inputA = inputs.GetOrAdd(match.Groups[1].Value, name => new Wire(name));
            var inputB = inputs.GetOrAdd(match.Groups[3].Value, name => new Wire(name));
            var output = inputs.GetOrAdd(match.Groups[4].Value, name => new Wire(name));
            gates.Add(new Gate(match.Groups[2].Value, [inputA, inputB], output));
        }

        foreach (var inputInput in inputInputs)
        {
            var split = inputInput.Split(": ");
            inputs[split[0]].SetInput(split[1] == "1");
        }

        var result = 0L;
        foreach (var (key, wire) in inputs.Where(i => i.Value.Value && Regex.IsMatch(i.Key, "z[0-9]{2}")))
        {
            result += 1L << int.Parse(key[1..]);
        }

        return Task.FromResult(result.ToString());
    }

    public class Wire
    {
        public string Name { get; }

        public bool HasValue { get; private set; }

        public bool Value { get; private set; }

        public EventHandler IsSet { get; set; }

        public Wire(string name)
        {
            Name = name;
        }

        public void SetInput(bool value)
        {
            Value = value;
            HasValue = true;
            IsSet?.Invoke(this, EventArgs.Empty);
        }
    }

    public class Gate
    {
        public string Type { get; }

        public Wire[] Inputs { get; private set; }

        public Wire Output { get; }

        public Gate(string type, Wire[] inputs, Wire output)
        {
            Type = type;
            Inputs = inputs;
            Output = output;
            if (inputs.All(i => i.HasValue))
            {
                SetOutput(this, EventArgs.Empty);
            }
            else
            {
                foreach (var input in inputs.Where(i => !i.HasValue))
                {
                    input.IsSet += SetOutput;
                }
            }
        }

        private void SetOutput(object? o, EventArgs e)
        {
            if (Inputs.All(i => i.HasValue))
            {
                var output = Type switch
                {
                    "OR" => Inputs.Any(i => i.Value),
                    "AND" => Inputs.All(i => i.Value),
                    "XOR" => Inputs.Count(i => i.Value) == 1,
                    _ => throw new ArgumentOutOfRangeException()
                };
                Output.SetInput(output);
            }
        }
    }

    public string? Part2TestSolution => null;


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        return Task.FromResult("");
    }

    public string? TestInput => @"x00: 1
x01: 0
x02: 1
x03: 1
x04: 0
y00: 1
y01: 1
y02: 1
y03: 1
y04: 1

ntg XOR fgs -> mjb
y02 OR x01 -> tnw
kwq OR kpj -> z05
x00 OR x03 -> fst
tgd XOR rvg -> z01
vdt OR tnw -> bfw
bfw AND frj -> z10
ffh OR nrd -> bqk
y00 AND y03 -> djm
y03 OR y00 -> psh
bqk OR frj -> z08
tnw OR fst -> frj
gnj AND tgd -> z11
bfw XOR mjb -> z00
x03 OR x00 -> vdt
gnj AND wpb -> z02
x04 AND y00 -> kjc
djm OR pbm -> qhw
nrd AND vdt -> hwm
kjc AND fst -> rvg
y04 OR y02 -> fgs
y01 AND x02 -> pbm
ntg OR kjc -> kwq
psh XOR fgs -> tgd
qhw XOR tgd -> z09
pbm OR djm -> kpj
x03 XOR y03 -> ffh
x00 XOR y04 -> ntg
bfw OR bqk -> z06
nrd XOR fgs -> wpb
frj XOR qhw -> z04
bqk OR frj -> z07
y03 OR x01 -> nrd
hwm AND bqk -> z03
tgd XOR rvg -> z12
tnw OR pbm -> gnj";
}