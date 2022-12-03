var input = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt"));

var matches = new List<char>();
foreach (var line in input)
{
	var lineHalf = line.Length / 2;

	var firstCompartment = new HashSet<char>(line[..lineHalf]);
	var secondCompartment = new HashSet<char>(line[lineHalf..]);

	matches.Add(firstCompartment.Intersect(secondCompartment).First());
}

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