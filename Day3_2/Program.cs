var input = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt"));

var chunks = input.Chunk(3);

var matches = chunks.Select(chunk =>
{
	var hashSets = chunk.Select(x => x.ToHashSet()).ToList();

	return hashSets
		.Skip(1)
		.Aggregate(hashSets.First(), (current, prev) =>
		{
			current.IntersectWith(prev);
			return current;
		})
		.First();
});
static int GetPriority(char item)
{
	return item switch
	{
		>= 'A' and <= 'Z' => item - 'A' + 27,
		_ => item - 'a' + 1,
	};
}

Console.WriteLine(matches.Sum(GetPriority));
Console.ReadLine();