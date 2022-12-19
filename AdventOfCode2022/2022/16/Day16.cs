using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;

namespace AdventOfCode2022._2022._16;

public class Day16 : IDay
{
    public int Year => 2022;

    public int Day => 16;

    private Regex lineRegex =
        new("Valve ([A-Z]{2}) has flow rate=([0-9]*); tunnels? leads? to valves? (([A-Z]{2})(, )?)*");

    private int bestBestScore = 0;

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var valves = inputs.Select(l =>
        {
            var regex = lineRegex.Match(l);
            return new Valve(regex.Groups[1].Value, int.Parse(regex.Groups[2].Value),
                regex.Groups[4].Captures.Select(c => c.Value));
        }).ToDictionary(v => v.Name);

        var addedConnection = false;
        do
        {
            addedConnection = false;
            foreach (var v in valves.Values)
            {
                var vConnections = v.Connections.ToArray();
                foreach (var c in vConnections)
                {
                    foreach (var newC in valves[c.Key].Connections)
                    {
                        if (v.Name != newC.Key && !v.Connections.ContainsKey(newC.Key))
                        {
                            v.Connections.Add(newC.Key, c.Value + newC.Value);
                            addedConnection = true;
                        }
                    }
                }
            }
        }while (addedConnection);

        bestBestScore = 0;
        var currentValve = "AA";
        var remainingTime = 30;

        var bestScore = Go(valves, new List<string>(), currentValve, remainingTime,
            valves.Select(v => v.Value.FlowRate).Sum(), 0);

        return Task.FromResult(bestScore.ToString());
    }

    private int Go(IReadOnlyDictionary<string, Valve> valves, IReadOnlyList<string> openValves, string currentValve, int remainingTime, int maxFlowRate, int currentScore)
    {
        if (remainingTime == 0) return currentScore;
        
        var currentFlowRate = openValves.Select(o => valves[o].FlowRate).Sum();
        
        var bestScore = currentScore + remainingTime * currentFlowRate;
        if (currentFlowRate == maxFlowRate)
            return bestScore;

        if (maxFlowRate * remainingTime + currentScore <= bestBestScore)
            return bestScore;
        
        if (!openValves.Contains(currentValve) && valves[currentValve].FlowRate > 0)
        {
            var open = new List<string>(openValves) {currentValve};
            var score = Go(valves, open, currentValve, remainingTime - 1, maxFlowRate, currentScore + currentFlowRate);
            if (score > bestScore)
            {
                bestScore = score;
            }
        }

        foreach (var connection in valves[currentValve].Connections.Where(c => !openValves.Contains(c.Key) && valves[c.Key].FlowRate > 0))
        {
            if (remainingTime <= connection.Value) continue;
            
            var score = Go(valves, openValves, connection.Key, remainingTime - connection.Value, maxFlowRate, currentScore + connection.Value * currentFlowRate);

            if (score > bestScore)
            {
                bestScore = score;
            }
        }

        if (bestScore > bestBestScore)
        {
            bestBestScore = bestScore;
            Console.WriteLine($"Remaining: {remainingTime}, Score: {bestScore}");
        }
        return bestScore;
    }


    public async Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var valves = inputs.Select(l =>
        {
            var regex = lineRegex.Match(l);
            return new Valve(regex.Groups[1].Value, int.Parse(regex.Groups[2].Value),
                regex.Groups[4].Captures.Select(c => c.Value));
        }).ToDictionary(v => v.Name);

        var addedConnection = false;
        do
        {
            addedConnection = false;
            foreach (var v in valves.Values)
            {
                var vConnections = v.Connections.ToArray();
                foreach (var c in vConnections)
                {
                    foreach (var newC in valves[c.Key].Connections)
                    {
                        if (v.Name != newC.Key && !v.Connections.ContainsKey(newC.Key))
                        {
                            v.Connections.Add(newC.Key, c.Value + newC.Value);
                            addedConnection = true;
                        }
                    }
                }
            }
        } while (addedConnection);

        foreach (var v in valves.Values)
        {
            v.Connections = v.Connections.Where(c => valves[c.Key].FlowRate > 0).OrderBy(c => c.Value).ThenByDescending(c => valves[c.Key].FlowRate)
                .ToDictionary(d => d.Key, d => d.Value);
        }

        bestBestScore = 0;
        var currentValve = "AA";
        var remainingTime = 26;

        var bestScore = Go2(new List<string>(), new List<string>(), valves, new List<string>(), currentValve, currentValve, 0, 0, remainingTime,
            valves.Select(v => v.Value.FlowRate).Sum(), 0);

        return bestScore.ToString();
    }

    private int Go2(List<string> meHistory, List<string> elephantHistory, IReadOnlyDictionary<string, Valve> valves, IReadOnlyList<string> openValves, string currentMeValve, string currentElephantValve, int remainingMeTravel, int remainingElephantTravel,  int remainingTime, int maxFlowRate, int currentScore)
    {
        if (remainingTime == 0)
        {
            if (currentScore > bestBestScore)
            {
                bestBestScore = currentScore;
                
                Console.WriteLine($"Score: {currentScore}");
                //for (int i = 0; i < meHistory.Count; i++)
                //{
                //    Console.WriteLine($"{meHistory[i]} - {elephantHistory[i]}");
                //}
                //Console.WriteLine();
            }
            return currentScore;
        }
        
        var currentFlowRate = openValves.Select(o => valves[o].FlowRate).Sum();
        
        var bestScore = currentScore + remainingTime * currentFlowRate;
        if (currentFlowRate == maxFlowRate || maxFlowRate * remainingTime + currentScore <= bestBestScore)
        {
            if (bestScore > bestBestScore)
            {
                bestBestScore = bestScore;
                
                Console.WriteLine($"Score: {bestScore}");
                //for (int i = 0; i < meHistory.Count; i++)
                //{
                //    Console.WriteLine($"{meHistory[i]} - {elephantHistory[i]}");
                //}
                //Console.WriteLine();
            }
            return bestScore;
        }

        var personNextMoves = new List<(string, int)>();
        var elephantNextMoves = new List<(string, int)>();
        if (remainingMeTravel <= 0)
        {
            if (!openValves.Contains(currentMeValve) && valves[currentMeValve].FlowRate > 0)
                personNextMoves.Add((currentMeValve, -1));
            personNextMoves.AddRange(valves[currentMeValve].Connections.Where(c => c.Key != currentElephantValve && !openValves.Contains(c.Key)).Select(connection => (connection.Key, connection.Value - 1)));
        }
        else
        {
            personNextMoves.Add((currentMeValve, remainingMeTravel - 1));
        }

        if (personNextMoves.Count == 0)
        {
            personNextMoves.Add((currentMeValve, 0));
        }

        if (remainingElephantTravel <= 0)
        {
            if (currentElephantValve != currentMeValve && !openValves.Contains(currentElephantValve) && valves[currentElephantValve].FlowRate > 0)
                elephantNextMoves.Add((currentElephantValve, -1));
            elephantNextMoves.AddRange(valves[currentElephantValve].Connections.Where(c => c.Key != currentMeValve && !openValves.Contains(c.Key)).Select(connection => (connection.Key, connection.Value - 1)));
        }
        else
        {
            elephantNextMoves.Add((currentElephantValve, remainingElephantTravel - 1));
        }

        if (elephantNextMoves.Count == 0)
        {
            elephantNextMoves.Add((currentElephantValve, 0));
        }

        foreach (var personNextMove in personNextMoves)
        {
            foreach (var elephantNextMove in elephantNextMoves)
            {
                if (personNextMove.Item1 == elephantNextMove.Item1) continue;

                var open = new List<string>(openValves);
                if (personNextMove.Item2 < 0)
                    open.Add(personNextMove.Item1);
                if(elephantNextMove.Item2 < 0)
                    open.Add(elephantNextMove.Item1);

                var meNewHistory = new List<string>(meHistory) {$"{personNextMove.Item1}:{personNextMove.Item2}"};
                var elephantNewHistory = new List<string>(elephantHistory) {$"{elephantNextMove.Item1}:{elephantNextMove.Item2}"};

                var score = Go2(meNewHistory, elephantNewHistory, valves, open, personNextMove.Item1,
                    elephantNextMove.Item1, personNextMove.Item2,
                    elephantNextMove.Item2, remainingTime - 1, maxFlowRate, currentScore + currentFlowRate);
                if (score > bestScore)
                    bestScore = score;
            }
        }

        return bestScore;
    }

    public string? TestInput => @"Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
Valve BB has flow rate=13; tunnels lead to valves CC, AA
Valve CC has flow rate=2; tunnels lead to valves DD, BB
Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
Valve EE has flow rate=3; tunnels lead to valves FF, DD
Valve FF has flow rate=0; tunnels lead to valves EE, GG
Valve GG has flow rate=0; tunnels lead to valves FF, HH
Valve HH has flow rate=22; tunnel leads to valve GG
Valve II has flow rate=0; tunnels lead to valves AA, JJ
Valve JJ has flow rate=21; tunnel leads to valve II";
}

public class Valve
{
    public string Name { get; }
    public int FlowRate { get; }
    public Dictionary<string, int> Connections { get; set; }

    public Valve(string name, int flowRate, IEnumerable<string> connections)
    {
        Name = name;
        FlowRate = flowRate;
        Connections = connections.ToDictionary(k => k, k => 1);
    }
}