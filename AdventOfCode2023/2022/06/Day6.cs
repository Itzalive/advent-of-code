namespace AdventOfCode2023._2022._06;

public class Day6 : IDay
{
    public int Year => 2022;
    public int Day => 6;

    public string? Part1TestSolution => null;

    public Task<string> Part1(string input)
    {
        var numberOfValuesInSuccession = 4;

        return Task.FromResult((Enumerable.Range(0, input.Length).First(i =>
                                    input[i..(i + numberOfValuesInSuccession)].ToCharArray().Distinct().Count() ==
                                    numberOfValuesInSuccession) +
                                numberOfValuesInSuccession).ToString());
    }

    public string? Part2TestSolution => null;

    public Task<string> Part2(string input)
    {
        var numberOfValuesInSuccession = 14;

        return Task.FromResult((Enumerable.Range(0, input.Length).First(i =>
                                    input[i..(i + numberOfValuesInSuccession)].ToCharArray().Distinct().Count() ==
                                    numberOfValuesInSuccession) +
                                numberOfValuesInSuccession).ToString());
    }

    public string TestInput => "";
}