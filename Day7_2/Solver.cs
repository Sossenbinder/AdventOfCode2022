using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace Day7_2
{
	[MemoryDiagnoser]
	public class Solver
	{
		private readonly string[] _input;

		public Solver()
		{
			_input = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt"));
		}

		[Benchmark]
		public void Solve()
		{
			var fileSystem = new Dictionary<string, Dir>();
			var currentDir = new Dir(new Dictionary<string, Dir>(), "/", null, new List<FsFile>());
			fileSystem.Add("/", currentDir);
			var baseDir = currentDir;
			foreach (var line in _input)
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
											currentDir.Directories.Add(newDir.Name, newDir);
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

			const int maxSize = 70000000;
			const int requiredSize = 30000000;

			var totalFsSize = baseDir.Size;
			var unusedSpace = maxSize - totalFsSize;
			var sizeToBeFreed = requiredSize - unusedSpace;

			Dir? FindFirstMatchingDirToFree(Dir dir)
			{
				if (dir.Directories.Any())
				{
					return dir.Directories.Select(x => FindFirstMatchingDirToFree(x.Value)).Where(x => x != null).MinBy(x => x!.Size);
				}

				return dir.Size > sizeToBeFreed ? dir : null;
			}

			var firstMatch = FindFirstMatchingDirToFree(baseDir)!.Size;
		}
	}

	internal record Dir(Dictionary<string, Dir> Directories, string Name, Dir Parent, List<FsFile> Files)
	{
		public int Size => Directories.Values.Select(x => x.Size).Sum() + Files.Select(x => x.Size).Sum();
	}

	internal record FsFile(string Name, int Size);
}