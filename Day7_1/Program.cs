var input = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt"));

var fileSystem = new Dictionary<string, Dir>();
var currentDir = new Dir(new Dictionary<string, Dir>(), "/", null, new List<FsFile>());
fileSystem.Add("/", currentDir);
var baseDir = currentDir;
foreach (var line in input)
{
	var commandSplit = line.Split(' ');

	switch (commandSplit[0])
	{
		case "$":
			switch (commandSplit[1])
			{
				case "ls":
					break;

				case "cd":
					var target = commandSplit[2];
					switch (target)
					{
						case "..":
							currentDir = currentDir!.Parent;
							break;

						case "/":
							currentDir = baseDir;
							break;

						default:
							{
								var newDir = new Dir(new Dictionary<string, Dir>(), target, currentDir, new List<FsFile>());
								currentDir!.Directories.Add(newDir.Name, newDir);
								currentDir = newDir;
								break;
							}
					}
					break;
			}
			break;

		case "dir":
			break;

		default:
			currentDir.Files.Add(new FsFile(commandSplit[1], int.Parse(commandSplit[0])));
			break;
	}
}

var smallDirs = new List<Dir>();
void CalculateSmallDir(Dir dir)
{
	foreach (var (_, value) in dir.Directories)
	{
		CalculateSmallDir(value);
	}

	if (dir.Size > 100000)
	{
		return;
	}

	smallDirs.Add(dir);
}

CalculateSmallDir(baseDir);

Console.WriteLine(smallDirs.Sum(x => x.Size));
Console.ReadLine();

internal record Dir(Dictionary<string, Dir> Directories, string Name, Dir Parent, List<FsFile> Files)
{
	public int Size => Directories.Values.Select(x => x.Size).Sum() + Files.Select(x => x.Size).Sum();
}

internal record FsFile(string Name, int Size);