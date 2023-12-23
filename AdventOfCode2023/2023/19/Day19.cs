using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AdventOfCode2023._2023._19;

public class Day19 : IDay
{
    public int Year => 2023;

    public int Day => 19;

    public string? Part1TestSolution => "19114";

    public Task<string> Part1(string input)
    {
        var parts = input.Split(Environment.NewLine + Environment.NewLine).ToArray();

        var workflows = parts[0].Split(Environment.NewLine).Select(l => new Workflow(l)).ToDictionary(w => w.Name);
        var inputs = parts[1].Split(Environment.NewLine).Select(v => new Part(v)).ToArray();


        var score = 0;
        foreach(var part in inputs)
        {
            var currentWorkflow = "in";
            while (currentWorkflow != "A" && currentWorkflow != "R")
            {
                var workflow = workflows[currentWorkflow];
                foreach (var rule in workflow.Rules)
                {
                    var result = rule.TestPart(part);
                    if (result != null)
                    {
                        currentWorkflow = result;
                        break;
                    }
                }
            }
            if(currentWorkflow == "A")
            {
                score += part.Values.Values.Sum();
            }
        }

        return Task.FromResult(score.ToString());
    }

    public string? Part2TestSolution => "167409079868000";


    public Task<string> Part2(string input)
    {
        var parts = input.Split(Environment.NewLine + Environment.NewLine).ToArray();

        var workflows = parts[0].Split(Environment.NewLine).Select(l => new Workflow(l)).ToDictionary(w => w.Name);

        long score = 0;
        Parallel.For(1, 4000, (x) =>
        {
            for (var s = 1; s <= 4000; s++)
            {
                for (var m = 1; m <= 4000; m++)
                {
                    for (var a = 1; a <= 4000; a++)
                    {
                        var part = new Part($"{{x={x},s={s},a={a},m={m}}}");
                        var currentWorkflow = "in";
                        while (currentWorkflow != "A" && currentWorkflow != "R")
                        {
                            var workflow = workflows[currentWorkflow];
                            foreach (var rule in workflow.Rules)
                            {
                                var result = rule.TestPart(part);
                                if (result != null)
                                {
                                    currentWorkflow = result;
                                    break;
                                }
                            }
                        }
                        if (currentWorkflow == "A")
                        {
                            score++;
                        }
                    }
                }
                Console.WriteLine("Completed s");
            }
            Console.WriteLine("Completed x");
        });

        return Task.FromResult(score.ToString());
    }

    public string? TestInput => @"px{a<2006:qkq,m>2090:A,rfg}
pv{a>1716:R,A}
lnx{m>1548:A,A}
rfg{s<537:gd,x>2440:R,A}
qs{s>3448:A,lnx}
qkq{x<1416:A,crn}
crn{x>2662:A,R}
in{s<1351:px,qqz}
qqz{s>2770:qs,m<1801:hdj,R}
gd{a>3333:R,R}
hdj{m>838:A,pv}

{x=787,m=2655,a=1222,s=2876}
{x=1679,m=44,a=2067,s=496}
{x=2036,m=264,a=79,s=2244}
{x=2461,m=1339,a=466,s=291}
{x=2127,m=1623,a=2188,s=1013}";

    private class Workflow
    {
        public string Name { get; set; }

        public List<Rule> Rules { get; set; }

        public Workflow(string l)
        {
            var match = l.Split('{');
            Name = match[0];
            Rules = match[1].Trim('}').Split(',').Select(r => new Rule(r)).ToList();
        }
    }

    public class Rule
    {
        public string? Variable { get; set; }

        public bool? IsGreaterThan { get; set; }

        public bool? IsLessThan { get; set; }

        public int? Value { get; set; }

        public string Outcome { get; set; }

        public Rule(string r)
        {
            var ruleParts = r.Split(':');
            if (ruleParts.Length == 1) {
                Outcome = ruleParts[0];
            }
            else
            {
                Outcome = ruleParts[1];
                IsGreaterThan = ruleParts[0].Contains('>');
                IsLessThan = ruleParts[0].Contains('<');
                var strings = ruleParts[0].Split(IsGreaterThan == true ? '>' : '<');
                Variable = strings[0];
                Value = int.Parse(strings[1]);
            }
        }

        public string? TestPart(Part p)
        {
            if (Variable == null)
                return Outcome;

            var value = p.Values[Variable];
            if (IsGreaterThan== true && value > Value.Value)
            {
                return Outcome;
            }
            if (IsLessThan == true && value < Value.Value)
            {
                return Outcome;
            }
            return null;
        }
    }

    public class Part
    {
        public Dictionary<string, int> Values { get; }

        public Part(string v)
        {
            Values = v.Trim(new[] { '{', '}' }).Split(',').Select(v => v.Split('=')).ToDictionary(v => v[0], v => int.Parse(v[1]));
        }
    }
}