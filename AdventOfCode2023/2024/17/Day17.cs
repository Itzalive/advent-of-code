namespace AdventOfCode2023._2024._17;

public class Day17 : IDay
{
    public int Year => 2024;

    public int Day => 17;

    public string? Part1TestSolution => "4,6,3,5,6,3,5,2,1,0";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine + Environment.NewLine).ToArray();
        var registers = inputs[0].Split(Environment.NewLine).Select(l => long.Parse(l.Split(": ")[1])).ToArray();
        var program = inputs[1].Split(": ")[1].Split(",").Select(byte.Parse).ToArray();

        var outputs = new List<byte>();
        for(var i = 0L; i< program.LongLength; i+=2)
        {
            var opcode = program[i];
            var operand = program[i + 1];
            switch (opcode)
            {
                // adv - division
                case 0:
                    registers[0] = (int)Math.Truncate(registers[0] / Math.Pow(2, comboOperand(operand, registers)));
                    break;

                // bxl - bitwise XOR
                case 1:
                    registers[1] ^= operand;
                    break;

                // bst - modulo
                case 2:
                    registers[1] = comboOperand(operand, registers) % 8;
                    break;

                //jnz
                case 3:
                    if (registers[0] != 0)
                    {
                        i = operand - 2;
                    }

                    break;

                // bxc - bitwise XOR
                case 4:
                    registers[1] = registers[1] ^ registers[2];
                    break;

                // out - output
                case 5:
                    outputs.Add((byte)(comboOperand(operand, registers) % 8));
                    break;

                //bdv
                case 6:
                    registers[1] = (int)Math.Truncate(registers[0] / Math.Pow(2, comboOperand(operand, registers)));
                    break;

                //cdv
                case 7:
                    registers[2] = (int)Math.Truncate(registers[0] / Math.Pow(2, comboOperand(operand, registers)));
                    break;

            }
        }

        return Task.FromResult(string.Join(",", outputs));
    }

    private long comboOperand(byte operand, long[] registers)
    {
        return operand switch
        {
            0 => 0,
            1 => 1,
            2 => 2,
            3 => 3,
            4 => registers[0],
            5 => registers[1],
            6 => registers[2],
            _ => throw new NotImplementedException()
        };
    }

    public string? Part2TestSolution => "117440";


    public async Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine + Environment.NewLine).ToArray();
        var program = inputs[1].Split(": ")[1].Split(",").Select(byte.Parse).ToArray();

        var cancellationToken = new CancellationTokenSource();
            var result = long.MaxValue;
            var resultLock = new object();
            try
            {
                await Parallel.ForAsync(program.Length > 10 ? 1000 : 0, int.MaxValue, cancellationToken.Token, (m, _) =>
                {
                    var registers = new[] { 0L, 0L, 0L };
                    var outputCount = 0;
                    var isMatch = true;
                    for (var n = 0; n < 100000000L; n++)
                    {
                        registers[0] = m * 100000000L + n;
                        registers[1] = 0;
                        registers[2] = 0;
                        outputCount = 0;
                        isMatch = true;
                        for (var i = 0L; i < program.LongLength; i += 2)
                        {
                            switch (program[i])
                            {
                                // adv - division
                                case 0:
                                    registers[0] =
                                        (long)Math.Truncate(registers[0] /
                                                           Math.Pow(2, comboOperand(program[i + 1], registers)));
                                    break;

                                // bxl - bitwise XOR
                                case 1:
                                    registers[1] ^= program[i + 1];
                                    break;

                                // bst - modulo
                                case 2:
                                    registers[1] = comboOperand(program[i + 1], registers) % 8;
                                    break;

                                //jnz
                                case 3:
                                    if (registers[0] != 0)
                                    {
                                        i = program[i + 1] - 2;
                                    }

                                    break;

                                // bxc - bitwise XOR
                                case 4:
                                    registers[1] ^= registers[2];
                                    break;

                                // out - output
                                case 5:
                                    var comboOperandResult = (byte)(comboOperand(program[i + 1], registers) % 8);
                                    outputCount++;
                                    if (outputCount > program.Length || program[outputCount - 1] != comboOperandResult)
                                    {
                                        i = program.LongLength;
                                        isMatch = false;
                                    }

                                    break;

                                //bdv
                                case 6:
                                    registers[1] =
                                        (long)Math.Truncate(registers[0] /
                                                            Math.Pow(2, comboOperand(program[i + 1], registers)));
                                    break;

                                //cdv
                                case 7:
                                    registers[2] =
                                        (long)Math.Truncate(registers[0] /
                                                            Math.Pow(2, comboOperand(program[i + 1], registers)));
                                    break;

                            }
                        }

                        if (outputCount != program.Length || !isMatch) continue;

                        lock (resultLock)
                        {
                            var a = m * 100000000L + n;
                            if (a < result)
                                result = a;
                        }
                        cancellationToken.Cancel();
                    }
                    Console.WriteLine(m * 100000000L);

                    return ValueTask.CompletedTask;
                });
            }
            catch (Exception _)
            {

            }

            return result.ToString();
    }

    public string? TestInput => @"Register A: 2024
Register B: 0
Register C: 0

Program: 0,3,5,4,3,0";
}