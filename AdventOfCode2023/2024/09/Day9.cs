namespace AdventOfCode2023._2024._09;

public class Day9 : IDay
{
    public int Year => 2024;

    public int Day => 9;

    public string? Part1TestSolution => "1928";

    public Task<string> Part1(string input)
    {
        var values = input.ToCharArray().Select(v => int.Parse(v.ToString())).ToArray();
        var numFiles = (values.Length + 1) / 2;
        var blocksMoved = 0;
        var currentEndFile = numFiles - 1;
        var endFileBlocksMoved = 0;
        var currentStartFile = 0;
        var cursor = 0;
        long checksum = 0;
        while (blocksMoved < values.Where((x, i) => i % 2 == 0 && (i - i % 2) / 2 >= currentStartFile).Sum())
        {
            var enumerable = values.Where((x, i) => i % 2 == 0 && (i - i % 2) / 2 > currentStartFile);
            var fileSizeAfterThisFile = enumerable.Sum();

            for (var i = 0; i < values[currentStartFile * 2] && (blocksMoved < fileSizeAfterThisFile || i < values[currentStartFile * 2] - blocksMoved + fileSizeAfterThisFile); i++)
            {
                checksum += currentStartFile * cursor;
                cursor++;
                Console.Write(currentStartFile);
            }

            if (blocksMoved > fileSizeAfterThisFile)
            {
                break;
            }

            // gaps
            for (var i = 0; i < values[currentStartFile * 2 + 1]; i++)
            {
                checksum += currentEndFile * cursor;
                cursor++;
                Console.Write(currentEndFile);
                blocksMoved++;
                endFileBlocksMoved++;
                if (endFileBlocksMoved >= values[currentEndFile * 2])
                {
                    currentEndFile--;
                    endFileBlocksMoved = 0;
                }
            }

            currentStartFile++;
        }
        return Task.FromResult(checksum.ToString());
    }

    public string? Part2TestSolution => "2858";


    public Task<string> Part2(string input)
    {
        var values = input.ToCharArray().Select(v => int.Parse(v.ToString())).ToList();
        var files = new List<(int size, int fileNum)>();
        for (var i = 0; i < values.Count; i++)
        {
            files.Add((values[i], i % 2 == 0 ? i / 2 : -1));
        }

        var numFiles = (files.Count + 1) / 2;
        for (var fileToMove = numFiles - 1; fileToMove > 0; fileToMove--)
        {
            var fileSize = files[fileToMove * 2].size;
            var fileNum = files[fileToMove * 2].fileNum;
            for (var gapToCheck = 0; gapToCheck < fileToMove; gapToCheck++)
            {
                var gapSize = files[gapToCheck * 2 + 1].size;
                if (gapSize >= fileSize)
                {
                    files.RemoveAt(gapToCheck *2 + 1);
                    files.Insert(gapToCheck * 2 + 1, (gapSize - fileSize, -1));
                    files.Insert(gapToCheck * 2 + 1, (fileSize, fileNum));
                    files.Insert(gapToCheck * 2 + 1, (0, -1));
                    fileToMove++;
                    if (fileToMove * 2 == files.Count - 1)
                    {
                        files.RemoveAt(fileToMove * 2);
                        files.RemoveAt(fileToMove * 2 - 1);
                    }
                    else
                    {
                        var newGap = files[fileToMove * 2 - 1].size + fileSize + files[fileToMove * 2 + 1].size;
                        files.RemoveAt(fileToMove * 2 - 1);
                        files.RemoveAt(fileToMove * 2 - 1);
                        files.RemoveAt(fileToMove * 2 - 1);
                        files.Insert(fileToMove * 2 - 1, (newGap, -1));
                    }
                    break;
                }
            }

            //for (var i = 0; i < numFiles; i++)
            //{
            //    for (var j = 0; j < files[i * 2].size; j++)
            //    {
            //        Console.Write(files[i*2].fileNum);
            //    }

            //    if (i < numFiles - 1)
            //    {
                 
            //        Console.Write(string.Join("", Enumerable.Range(0, files[i * 2 + 1].size).Select(v => '.')));
            //    }
            //}

            //Console.WriteLine();
        }

        long checksum = 0;
        var cursor = 0;
        for (var i = 0; i < numFiles; i++)
        {
            for (var j = 0; j < files[i * 2].size; j++)
            {
                checksum += cursor * files[i * 2].fileNum;
                cursor++;
                Console.Write(files[i * 2].fileNum);
            }

            if (i < numFiles - 1)
            {
                Console.Write(string.Join("", Enumerable.Range(0, files[i * 2 + 1].size).Select(v => '.')));
                cursor += files[i * 2 + 1].size;
            }
        }

        return Task.FromResult(checksum.ToString());
    }

    public string? TestInput => "2333133121414131402";
}