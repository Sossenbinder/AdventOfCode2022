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

var max = spottedTrees.Select(CalculateScenicScore).Max();

Console.WriteLine(max);
Console.ReadLine();

int CalculateScenicScore(Point point)
{
	var (x, y) = point;
	var originHeight = input[y][x] - '0';

	var up = 1;
	for (var yPos = y - 1; yPos > 0; yPos--)
	{
		if (originHeight <= input[yPos][x] - '0')
		{
			break;
		}

		up++;
	}

	var down = 1;
	for (var yPos = y + 1; yPos < input.Length - 1; yPos++)
	{
		if (originHeight <= input[yPos][x] - '0')
		{
			break;
		}
		down++;
	}

	var left = 1;
	for (var xPos = x - 1; xPos > 0; xPos--)
	{
		if (originHeight <= input[y][xPos] - '0')
		{
			break;
		}

		left++;
	}

	var right = 1;
	for (var xPos = x + 1; xPos < input.Length - 1; xPos++)
	{
		if (originHeight <= input[y][xPos] - '0')
		{
			break;
		}

		right++;
	}

	Console.WriteLine($"Pos ({x}, {y}) -> Up: {up}, Down: {down}, Left: {left}, Right: {right} => {up * down * left * right}");
	return up * down * left * right;
}

record Point(int X, int Y);