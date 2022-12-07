using System.Text.RegularExpressions;

var input = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt"));

var cargoLines = input.TakeWhile(line => line != "").ToArray();

var cargoIndexMap = new Dictionary<int, int>();
for (var i = 0; i < cargoLines.Last().Length; i++)
{
	var chr = cargoLines.Last()[i];
	if (chr != ' ')
	{
		cargoIndexMap.Add(i, chr - '0');
	}
}

var stackMap = cargoIndexMap.ToDictionary(x => x.Value, x => new Stack<char>());
foreach (var line in cargoLines[..^1].Reverse())
{
	foreach (var (index, value) in cargoIndexMap)
	{
		var charAt = line[index];
		if (charAt != ' ')
		{
			stackMap[value].Push(charAt);
		}
	}
}

var regexPattern = new Regex("move ([0-9].*) from ([0-9].*) to ([0-9].*)", RegexOptions.Compiled);

foreach (var line in input.Skip(cargoLines.Length + 1))
{
	var match = regexPattern.Match(line);

	var amount = int.Parse(match.Groups[1].Value);
	var from = int.Parse(match.Groups[2].Value);
	var to = int.Parse(match.Groups[3].Value);

	var poppedStack = new Stack<char>();

	for (var i = 0; i < amount; ++i)
	{
		poppedStack.Push(stackMap[from].Pop());
	}

	foreach (var stackItem in poppedStack)
	{
		stackMap[to].Push(stackItem);
	}
}

Console.WriteLine(string.Join("", stackMap.Values.Select(x => x.Pop())));
Console.ReadLine();