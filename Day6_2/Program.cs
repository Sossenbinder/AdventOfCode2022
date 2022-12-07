var input = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt")).First();

const int markerPosition = 14;
var inputStack = new Queue<char>(input[..markerPosition]);
for (var i = markerPosition; i < input.Length; ++i)
{
	if (new HashSet<char>(inputStack).Count == markerPosition)
	{
		Console.WriteLine(i);
		break;
	}

	inputStack.Dequeue();
	inputStack.Enqueue(input[i]);
}

Console.ReadLine();