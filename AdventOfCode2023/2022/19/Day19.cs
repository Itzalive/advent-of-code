using System.Text.RegularExpressions;

namespace AdventOfCode2023._2022._19;

public class Day19 : IDay
{
    public int Year => 2022;

    public int Day => 19;

    public string? Part1TestSolution => null;

    public Task<string> Part1(string input)
    {
        var blueprints = input.Split(Environment.NewLine).Select(l => new BluePrint(l)).ToArray();

        var score = 0;
        foreach (var blueprint in blueprints)
        {
            var result = blueprint.SolveForMinutes(24, 0, 0, 0, 0, 1, 0, 0, 0);
            Console.WriteLine(blueprint.Id + " " + result);
            score += result * blueprint.Id;
        }

        return Task.FromResult(score.ToString());
    }

    public string? Part2TestSolution => null;

    public Task<string> Part2(string input)
    {
        var blueprints = input.Split(Environment.NewLine).Select(l => new BluePrint(l)).ToArray();

        var results = new List<int>();
        foreach (var blueprint in blueprints.Take(3))
        {
            var result = blueprint.SolveForMinutes(32, 0, 0, 0, 0, 1, 0, 0, 0);
            Console.WriteLine(blueprint.Id + " " + result);
            results.Add(result);
        }

        return Task.FromResult(results.Aggregate((a, v) => a * v).ToString());
    }

    public string? TestInput => @"Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.
Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.";

    public class BluePrint
    {
        private static Regex BlueprintRegex =
            new Regex(
                "Blueprint ([0-9]+): Each ore robot costs ([0-9]+) ore. Each clay robot costs ([0-9]+) ore. Each obsidian robot costs ([0-9]+) ore and ([0-9]+) clay. Each geode robot costs ([0-9]+) ore and ([0-9]+) obsidian.");

        public int Id { get; set; }

        public int OreRobotCostOre { get; set; }

        public int ClayRobotCostOre { get; set; }

        public int ObsidianRobotCostOre { get; set; }

        public int ObsidianRobotCostClay { get; set; }

        public int GeodeRobotCostOre { get; set; }

        public int GeodeRobotCostObsidian { get; set; }

        public int BestScore { get; set; }

        public BluePrint(string blueprint)
        {
            var match = BlueprintRegex.Match(blueprint);
            Id = int.Parse(match.Groups[1].Value);
            OreRobotCostOre = int.Parse(match.Groups[2].Value);
            ClayRobotCostOre = int.Parse(match.Groups[3].Value);
            ObsidianRobotCostOre = int.Parse(match.Groups[4].Value);
            ObsidianRobotCostClay = int.Parse(match.Groups[5].Value);
            GeodeRobotCostOre = int.Parse(match.Groups[6].Value);
            GeodeRobotCostObsidian = int.Parse(match.Groups[7].Value);
        }

        public int SolveForMinutes(int minutes, int ore, int clay, int obsidian, int geode, int oreRobots, int clayRobots, int obsidianRobots, int geodeRobots)
        {
            var bestScore = geode + minutes * geodeRobots;

            if (minutes == 0 || minutes * (geodeRobots + minutes) + geode < BestScore)
            {
                if (bestScore <= BestScore) return bestScore;
                //Console.WriteLine(bestScore);
                BestScore = bestScore;

                return bestScore;
            }

            // build geode robots
            if (obsidianRobots > 0)
            {
                var daysUntilGeodeRobotBuild = Math.Max(0, (int)Math.Ceiling(Math.Max((GeodeRobotCostOre - ore) / (double)oreRobots, (GeodeRobotCostObsidian - obsidian) / (double)obsidianRobots))) + 1;
                if (daysUntilGeodeRobotBuild < minutes)
                {
                    var geodeScore = SolveForMinutes(minutes - daysUntilGeodeRobotBuild,
                        ore + oreRobots * daysUntilGeodeRobotBuild - GeodeRobotCostOre,
                        clay + clayRobots * daysUntilGeodeRobotBuild,
                        obsidian + obsidianRobots * daysUntilGeodeRobotBuild - GeodeRobotCostObsidian,
                        geode + geodeRobots * daysUntilGeodeRobotBuild,
                        oreRobots, clayRobots, obsidianRobots, geodeRobots + 1);
                    if (geodeScore > bestScore)
                        bestScore = geodeScore;
                }
            }

            // build obsidian robot next
            if (clayRobots > 0)
            {
                var daysUntilObsidianRobotBuild = Math.Max(0, (int)Math.Ceiling(Math.Max((ObsidianRobotCostOre - ore) / (double)oreRobots, (ObsidianRobotCostClay - clay) / (double)clayRobots))) + 1;
                if (daysUntilObsidianRobotBuild < minutes)
                {
                    var obsidianScore = SolveForMinutes(minutes - daysUntilObsidianRobotBuild,
                        ore + oreRobots * daysUntilObsidianRobotBuild - ObsidianRobotCostOre,
                        clay + clayRobots * daysUntilObsidianRobotBuild - ObsidianRobotCostClay,
                        obsidian + obsidianRobots * daysUntilObsidianRobotBuild,
                        geode + geodeRobots * daysUntilObsidianRobotBuild,
                        oreRobots, clayRobots, obsidianRobots + 1, geodeRobots);
                    if (obsidianScore > bestScore)
                        bestScore = obsidianScore;
                }
            }

            // build clay robot next
            var daysUntilClayRobotBuild = Math.Max(0, (int)Math.Ceiling((ClayRobotCostOre - ore) / (double)oreRobots)) + 1;
            if (daysUntilClayRobotBuild < minutes)
            {
                var clayScore = SolveForMinutes(minutes - daysUntilClayRobotBuild,
                    ore + oreRobots * daysUntilClayRobotBuild - ClayRobotCostOre,
                    clay + clayRobots * daysUntilClayRobotBuild, obsidian + obsidianRobots * daysUntilClayRobotBuild,
                    geode + geodeRobots * daysUntilClayRobotBuild,
                    oreRobots, clayRobots + 1, obsidianRobots, geodeRobots);
                if (clayScore > bestScore)
                    bestScore = clayScore;
            }
            
            // build ore robot next
            var daysUntilOreRobotBuild = Math.Max(0, (int)Math.Ceiling((OreRobotCostOre - ore) / (double)oreRobots)) + 1;
            if (daysUntilOreRobotBuild < minutes)
            {
                var oreScore = SolveForMinutes(minutes - daysUntilOreRobotBuild,
                    ore + oreRobots * daysUntilOreRobotBuild - OreRobotCostOre,
                    clay + clayRobots * daysUntilOreRobotBuild, obsidian + obsidianRobots * daysUntilOreRobotBuild,
                    geode + geodeRobots * daysUntilOreRobotBuild,
                    oreRobots + 1, clayRobots, obsidianRobots, geodeRobots);
                if (oreScore > bestScore)
                    bestScore = oreScore;
            }

            if (bestScore <= BestScore) return bestScore;
            //Console.WriteLine(bestScore);
            BestScore = bestScore;

            return bestScore;
        }
    }
}