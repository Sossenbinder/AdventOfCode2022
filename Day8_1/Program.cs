var input = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt"));

var spottedTrees = new HashSet<Point>();

// Horizontal
for (var i = 1; i < input.Length - 1; ++i)
{
	var slider = 0;
	for (var j = 0; j < input[i].Length; ++j)
	{
		var value = input[i][j] - '0';
		if (value > slider && j > 0 && j < input[i].Length - 1)
		{
			spottedTrees.Add(new(j, i));
		}

		if (value > slider)
		{
			slider = value;
		}
	}

	slider = 0;
	for (var j = input[i].Length - 1; j >= 0; --j)
	{
		var value = input[i][j] - '0';
		if (value > slider && j > 0 && j < input[i].Length - 1)
		{
			spottedTrees.Add(new(j, i));
		}

		if (value > slider)
		{
			slider = value;
		}
	}
}

// Vertical
for (var i = 1; i < input[0].Length - 1; ++i)
{
	var slider = 0;
	for (var j = 0; j < input.Length; ++j)
	{
		var value = input[j][i] - '0';
		if (value > slider && j > 0 && j < input.Length - 1)
		{
			spottedTrees.Add(new(i, j));
		}

		if (value > slider)
		{
			slider = value;
		}
	}

	slider = 0;
	for (var j = input.Length - 1; j >= 0; --j)
	{
		var value = input[j][i] - '0';
		if (value > slider && j > 0 && j < input.Length - 1)
		{
			spottedTrees.Add(new(i, j));
		}

		if (value > slider)
		{
			slider = value;
		}
	}
}
// Sliding window algorithm

var outerRing = (input.Length + input[0].Length) * 2 - 4;

Console.WriteLine(outerRing + spottedTrees.Count);
Console.ReadLine();

record Point(int X, int Y);