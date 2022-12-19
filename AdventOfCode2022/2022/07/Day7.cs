namespace AdventOfCode2022._2022._07;

public class Day7 : IDay
{
    public int Year => 2022;
    public int Day => 7;

    public Task<string> Part1(string input)
    {
        var topLevelDirectory = ReadTreeStructure(input);

        var directories = topLevelDirectory.GetDirectoriesLessThan(100000);
        return Task.FromResult(directories.Sum(d => d.Size).ToString());
    }

    public Task<string> Part2(string input)
    {
        var topLevelDirectory = ReadTreeStructure(input);

        var targetSize = 30000000 - (70000000 - topLevelDirectory.Size);
        return Task.FromResult(topLevelDirectory.FindBestDirectory(targetSize, topLevelDirectory).Size.ToString());
    }

    private static Directory ReadTreeStructure(string input)
    {
        var topLevelDirectory = new Directory();
        Directory currentDirectory = null;

        var inputs = input.Split(Environment.NewLine).ToArray();
        for (var i = 0; i < inputs.Length; i++)
        {
            var line = inputs[i].Split(' ');
            if (line[0] != "$") throw new NotImplementedException();
            switch (line[1])
            {
                case "cd":
                    if (line[2] == "/")
                    {
                        currentDirectory = topLevelDirectory;
                    }
                    else if (line[2] == "..")
                    {
                        currentDirectory = currentDirectory.Parent;
                    }
                    else
                    {
                        currentDirectory = currentDirectory.Directories.Single(d => d.Name == line[2]);
                    }

                    break;
                case "ls":
                    while (i + 1 < inputs.Length && !inputs[i + 1].StartsWith("$"))
                    {
                        i++;
                        line = inputs[i].Split(" ");
                        if (line[0] != "dir")
                        {
                            var size = int.Parse(line[0]);
                            currentDirectory.Files.Add(new File {Name = line[1], Size = size});
                            currentDirectory.IncreaseSize(size);
                        }
                        else
                        {
                            currentDirectory.Directories.Add(new Directory {Name = line[1], Parent = currentDirectory});
                        }
                    }

                    break;
            }
        }

        return topLevelDirectory;
    }

    public string TestInput => @"$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k";

    class Directory
    {
        public string? Name { get; set; }

        public List<Directory> Directories { get; set; } = new List<Directory>();

        public List<File> Files { get; set; } = new List<File>();

        public Directory? Parent { get; set; }

        public int Size { get; set; } = 0;

        public void IncreaseSize(int size)
        {
            Size += size;
            Parent?.IncreaseSize(size);
        }

        public IEnumerable<Directory> GetDirectoriesLessThan(int i)
        {
            var result = new List<Directory>();
            if (this.Size < i) result.Add(this);
            foreach (var directory in Directories)
            {
                result.AddRange(directory.GetDirectoriesLessThan(i));
            }
            return result;
        }

        public void Output(string tabbing)
        {
            Console.WriteLine(tabbing + "- ");
        }

        public Directory FindBestDirectory(int targetSize, Directory currentBest)
        {
            foreach (var directory in Directories)
            {
                currentBest = directory.FindBestDirectory(targetSize, currentBest);
            }

            if (this.Size > targetSize && this.Size < currentBest.Size)
            {
                return this;
            }

            return currentBest;
        }
    }

    class File
    {
        public int Size { get; set; }

        public string Name { get; set; }
    }
}