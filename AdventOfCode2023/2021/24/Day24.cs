using System.Linq.Expressions;

namespace AdventOfCode2023._2021._24;

public class Day24 : IDay
{
    public int Year => 2021;

    public int Day => 24;

    public string? Part1TestSolution => null;

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var registers = new List<Dictionary<string, Expression>>();
        var inputParameters = Enumerable.Range(0, 14).Select(c => Expression.Parameter(typeof(long), "Param_"+c)).ToArray();

        var parameters = new List<List<ParameterExpression>>();
        var nextParameter = -1;
        foreach (var instruction in inputs)
        {
            var parts = instruction.Split(" ").ToArray();
            var rightSide = parts.Length > 2
                ? long.TryParse(parts[2], out var addValue) ? Expression.Constant(addValue) : registers[nextParameter][parts[2]]
                : null;

            switch (parts[0])
            {
                case "inp":
                    nextParameter++;
                    var previousZ = Expression.Parameter(typeof(long), "z");
                    registers.Add(new Dictionary<string, Expression>()
                    {
                        {"w", Expression.Constant((long)0)}, {"x", Expression.Constant((long)0)},
                        {"y", Expression.Constant((long)0)}, {"z", previousZ}
                    });
                    var newParameters = new List<ParameterExpression>()
                    {
                        previousZ,
                        inputParameters[nextParameter]
                    };
                    parameters.Add(newParameters);

                    registers[nextParameter][parts[1]] = inputParameters[nextParameter];
                    break;
                case "add":
                    if (rightSide.NodeType != ExpressionType.Constant ||
                        ((long) ((ConstantExpression) rightSide).Value) != 0)
                    {
                        if (registers[nextParameter][parts[1]].NodeType != ExpressionType.Constant || ((long) ((ConstantExpression) registers[nextParameter][parts[1]]).Value) != 0)
                        {
                            registers[nextParameter][parts[1]] = Expression.Add(registers[nextParameter][parts[1]], rightSide);
                        }
                        else
                        {
                            registers[nextParameter][parts[1]] = rightSide;
                        }
                    }

                    break;
                case "mul":
                    if (parts[2] == "0")
                    {
                        registers[nextParameter][parts[1]] = Expression.Constant((long)0);
                    }
                    else
                    {
                        registers[nextParameter][parts[1]] = Expression.Multiply(registers[nextParameter][parts[1]], rightSide);
                    }

                    break;
                case "div":
                    if (registers[nextParameter][parts[1]].NodeType != ExpressionType.Constant || ((long)((ConstantExpression)registers[nextParameter][parts[1]]).Value) != 0)
                        registers[nextParameter][parts[1]] = Expression.Convert(Expression.Divide(registers[nextParameter][parts[1]], rightSide), typeof(long));
                    break;
                case "mod":
                    if (registers[nextParameter][parts[1]].NodeType != ExpressionType.Constant || ((long)((ConstantExpression)registers[nextParameter][parts[1]]).Value) != 0)
                        registers[nextParameter][parts[1]] = Expression.Modulo(registers[nextParameter][parts[1]], rightSide);
                    break;
                case "eql":
                    //registers[parts[1]] = Expression.IfThenElse(Expression.Equal(registers[parts[1]], rightSide), Expression.Constant(1), Expression.Constant(0));

                    registers[nextParameter][parts[1]] =
                        Expression.Convert(Expression.Equal(registers[nextParameter][parts[1]], rightSide), typeof(long));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(parts));
            }

            if (registers[nextParameter][parts[1]].CanReduce)
            {
                registers[nextParameter][parts[1]] = registers[nextParameter][parts[1]].Reduce();
            }
        }

        var functions = new Func<long, long, long>[nextParameter + 1];
        for (var i = 0; i <= nextParameter; i++)
        {
            functions[i] = Expression.Lambda<Func<long, long, long>>(registers[i]["z"], parameters[i]).Compile();
        }

        var possiblePreviousCombinations = new Dictionary<long, long> {{0, 0}};
        for (var i = 0; i <= nextParameter; i++)
        {
            Console.WriteLine($"Processing digit {i} with {possiblePreviousCombinations.Count} variations");
            var possibleOutcomes = new Dictionary<long, long>();
            foreach (var previousCombination in possiblePreviousCombinations)
            {
                for (var number = 1; number <= 9; number++)
                {
                    var result = functions[i](previousCombination.Key, number);
                    var bestNumber = previousCombination.Value * 10 + number;
                    if (possibleOutcomes.ContainsKey(result))
                    {
                        if(bestNumber > possibleOutcomes[result])
                            possibleOutcomes[result] = bestNumber;
                    }
                    else
                    {
                        possibleOutcomes.Add(result, bestNumber);
                    }
                }
            }

            possiblePreviousCombinations = possibleOutcomes;
        }

        //for (var number = 99999999999999; number >= 11111111111111 ; number--)
        //{
        //    var numberAsString = number.ToString();
        //    var placeValues = numberAsString.ToCharArray().Select(c => long.Parse(c.ToString())).ToArray();
        //    if (placeValues.Any(p => p == 0)) continue;
        //    if(numberAsString.EndsWith("99999999"))
        //        Console.WriteLine(number);
        //    var previousW = (long)0;
        //    var previousX = (long)0;
        //    var previousY = (long)0;
        //    var previousZ = (long)0;
        //    for (var i = 0; i <= nextParameter; i++)
        //    {
        //        previousW = functions[i]["w"](previousW, previousX, previousY, previousZ, placeValues[i]);
        //        previousX = functions[i]["x"](previousW, previousX, previousY, previousZ, placeValues[i]);
        //        previousY = functions[i]["y"](previousW, previousX, previousY, previousZ, placeValues[i]);
        //        previousZ = functions[i]["z"](previousW, previousX, previousY, previousZ, placeValues[i]);
        //    }

        //    if (previousZ == 0)
        //        return Task.FromResult(numberAsString);
        //}

        return Task.FromResult(possiblePreviousCombinations[0].ToString());
    }

    public class LambdaEqualityComparer<T> : IEqualityComparer<T>
    {
        public LambdaEqualityComparer(Func<T, T, bool> equalsFunction)
        {
            _equalsFunction = equalsFunction;
        }

        public bool Equals(T x, T y)
        {
            return _equalsFunction(x, y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        private readonly Func<T, T, bool> _equalsFunction;
    }

    public string? Part2TestSolution => null;

    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var registers = new List<Dictionary<string, Expression>>();
        var inputParameters = Enumerable.Range(0, 14).Select(c => Expression.Parameter(typeof(long), "Param_"+c)).ToArray();

        var parameters = new List<List<ParameterExpression>>();
        var nextParameter = -1;
        foreach (var instruction in inputs)
        {
            var parts = instruction.Split(" ").ToArray();
            var rightSide = parts.Length > 2
                ? long.TryParse(parts[2], out var addValue) ? Expression.Constant(addValue) : registers[nextParameter][parts[2]]
                : null;

            switch (parts[0])
            {
                case "inp":
                    nextParameter++;
                    var previousZ = Expression.Parameter(typeof(long), "z");
                    registers.Add(new Dictionary<string, Expression>()
                    {
                        {"w", Expression.Constant((long)0)}, {"x", Expression.Constant((long)0)},
                        {"y", Expression.Constant((long)0)}, {"z", previousZ}
                    });
                    var newParameters = new List<ParameterExpression>()
                    {
                        previousZ,
                        inputParameters[nextParameter]
                    };
                    parameters.Add(newParameters);

                    registers[nextParameter][parts[1]] = inputParameters[nextParameter];
                    break;
                case "add":
                    if (rightSide.NodeType != ExpressionType.Constant ||
                        ((long) ((ConstantExpression) rightSide).Value) != 0)
                    {
                        if (registers[nextParameter][parts[1]].NodeType != ExpressionType.Constant || ((long) ((ConstantExpression) registers[nextParameter][parts[1]]).Value) != 0)
                        {
                            registers[nextParameter][parts[1]] = Expression.Add(registers[nextParameter][parts[1]], rightSide);
                        }
                        else
                        {
                            registers[nextParameter][parts[1]] = rightSide;
                        }
                    }

                    break;
                case "mul":
                    if (parts[2] == "0")
                    {
                        registers[nextParameter][parts[1]] = Expression.Constant((long)0);
                    }
                    else
                    {
                        registers[nextParameter][parts[1]] = Expression.Multiply(registers[nextParameter][parts[1]], rightSide);
                    }

                    break;
                case "div":
                    if (registers[nextParameter][parts[1]].NodeType != ExpressionType.Constant || ((long)((ConstantExpression)registers[nextParameter][parts[1]]).Value) != 0)
                        registers[nextParameter][parts[1]] = Expression.Convert(Expression.Divide(registers[nextParameter][parts[1]], rightSide), typeof(long));
                    break;
                case "mod":
                    if (registers[nextParameter][parts[1]].NodeType != ExpressionType.Constant || ((long)((ConstantExpression)registers[nextParameter][parts[1]]).Value) != 0)
                        registers[nextParameter][parts[1]] = Expression.Modulo(registers[nextParameter][parts[1]], rightSide);
                    break;
                case "eql":
                    //registers[parts[1]] = Expression.IfThenElse(Expression.Equal(registers[parts[1]], rightSide), Expression.Constant(1), Expression.Constant(0));

                    registers[nextParameter][parts[1]] =
                        Expression.Convert(Expression.Equal(registers[nextParameter][parts[1]], rightSide), typeof(long));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(parts));
            }

            if (registers[nextParameter][parts[1]].CanReduce)
            {
                registers[nextParameter][parts[1]] = registers[nextParameter][parts[1]].Reduce();
            }
        }

        var functions = new Func<long, long, long>[nextParameter + 1];
        for (var i = 0; i <= nextParameter; i++)
        {
            functions[i] = Expression.Lambda<Func<long, long, long>>(registers[i]["z"], parameters[i]).Compile();
        }

        var possiblePreviousCombinations = new Dictionary<long, long> {{0, 0}};
        for (var i = 0; i <= nextParameter; i++)
        {
            Console.WriteLine($"Processing digit {i} with {possiblePreviousCombinations.Count} variations");
            var possibleOutcomes = new Dictionary<long, long>();
            foreach (var previousCombination in possiblePreviousCombinations)
            {
                for (var number = 1; number <= 9; number++)
                {
                    var result = functions[i](previousCombination.Key, number);
                    var bestNumber = previousCombination.Value * 10 + number;
                    if (possibleOutcomes.ContainsKey(result))
                    {
                        if(bestNumber < possibleOutcomes[result])
                            possibleOutcomes[result] = bestNumber;
                    }
                    else
                    {
                        possibleOutcomes.Add(result, bestNumber);
                    }
                }
            }

            possiblePreviousCombinations = possibleOutcomes;
        }

        //for (var number = 99999999999999; number >= 11111111111111 ; number--)
        //{
        //    var numberAsString = number.ToString();
        //    var placeValues = numberAsString.ToCharArray().Select(c => long.Parse(c.ToString())).ToArray();
        //    if (placeValues.Any(p => p == 0)) continue;
        //    if(numberAsString.EndsWith("99999999"))
        //        Console.WriteLine(number);
        //    var previousW = (long)0;
        //    var previousX = (long)0;
        //    var previousY = (long)0;
        //    var previousZ = (long)0;
        //    for (var i = 0; i <= nextParameter; i++)
        //    {
        //        previousW = functions[i]["w"](previousW, previousX, previousY, previousZ, placeValues[i]);
        //        previousX = functions[i]["x"](previousW, previousX, previousY, previousZ, placeValues[i]);
        //        previousY = functions[i]["y"](previousW, previousX, previousY, previousZ, placeValues[i]);
        //        previousZ = functions[i]["z"](previousW, previousX, previousY, previousZ, placeValues[i]);
        //    }

        //    if (previousZ == 0)
        //        return Task.FromResult(numberAsString);
        //}

        return Task.FromResult(possiblePreviousCombinations[0].ToString());
    }

    public string? TestInput => @"";
}