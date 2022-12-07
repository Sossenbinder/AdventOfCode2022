var input = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt")).First();

var inputStack = new Queue<char>(input[..4]);
for (var i = 4; i < input.Length; ++i)
{
	if (new HashSet<char>(inputStack).Count == 4)
	{
		Console.WriteLine(i);
		break;
	}

	inputStack.Dequeue();
	inputStack.Enqueue(input[i]);
}

Console.ReadLine();