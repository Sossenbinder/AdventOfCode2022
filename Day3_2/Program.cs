Console.WriteLine(File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt"))
	.Chunk(3)
	.Select(chunk => chunk.Select(x => x.ToHashSet())
	.Aggregate((current, prev) => current.Intersect(prev).ToHashSet())
	.First())
	.Sum(item => item is >= 'A' and <= 'Z' ? item - 'A' + 27 : item - 'a' + 1));

Console.ReadLine();