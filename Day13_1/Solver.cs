using BenchmarkDotNet.Attributes;

namespace Day13_1
{
	[MemoryDiagnoser]
	public class Solver
	{
		private readonly string[] _input;

		private readonly bool _print;

		public Solver()
		{
			_input = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt"));
		}

		public Solver(bool doPrint)
			: this()
		{
			_print = doPrint;
		}

		[Benchmark]
		public void Solve()
		{
			var rightIndices = new HashSet<int>();
			var index = 0;
			foreach (var input in _input.Chunk(3))
			{
				index++;

				var left = Parse(input[0]);
				var right = Parse(input[1]);

				var result = Compare(left, right);

				Console.WriteLine(result == Result.InOrder ? "In order" : "Out of order");

				if (result == Result.InOrder)
				{
					rightIndices.Add(index);
				}
			}

			if (_print)
			{
				Console.WriteLine(rightIndices.Sum());
				Console.ReadLine();
			}
		}

		private IItem Parse(string line)
		{
			var item = new ListItem(new List<IItem>());

			var subItemStack = new Stack<IItem>();
			subItemStack.Push(item);

			List<char> currentChar = new();
			foreach (var chr in line[1..^1])
			{
				var currentItem = subItemStack.Peek();

				if (chr == ',')
				{
					if (currentChar.Count > 0)
					{
						var number = int.Parse(new string(currentChar.ToArray()));
						((ListItem)currentItem).Items.Add(new IntegerItem(number));
						currentChar.Clear();
					}
					continue;
				}
				switch (chr)
				{
					case '[':
						var newItem = new ListItem(new List<IItem>());
						((ListItem)currentItem).Items.Add(newItem);
						subItemStack.Push(newItem);
						break;

					case ']':
						if (currentChar.Count > 0)
						{
							var number = int.Parse(new string(currentChar.ToArray()));
							((ListItem)currentItem).Items.Add(new IntegerItem(number));
							currentChar.Clear();
						}
						subItemStack.Pop();
						break;

					default:
						currentChar.Add(chr);
						break;
				}
			}

			return item;
		}

		private Result Compare(IItem left, IItem right)
		{
			if (left is IntegerItem leftInt && right is IntegerItem rightInt)
			{
				if (leftInt.Entry < rightInt.Entry)
				{
					return Result.InOrder;
				}

				if (leftInt.Entry > rightInt.Entry)
				{
					return Result.OutOfOrder;
				}

				return Result.Indecisive;
			}

			if (left is IntegerItem)
			{
				return Compare(new ListItem(new List<IItem>() { left }), right);
			}

			if (right is IntegerItem)
			{
				return Compare(left, new ListItem(new List<IItem>() { right }));
			}

			var leftQueue = new Queue<IItem>(((ListItem)left).Items);
			var rightQueue = new Queue<IItem>(((ListItem)right).Items);

			while (leftQueue.TryDequeue(out var leftItem))
			{
				if (!rightQueue.TryDequeue(out var rightItem))
				{
					return Result.OutOfOrder;
				}

				var result = Compare(leftItem, rightItem);

				if (result != Result.Indecisive)
				{
					return result;
				}
			}

			if (rightQueue.Any())
			{
				return Result.InOrder;
			}

			return Result.Indecisive;
		}
	}

	internal interface IItem
	{ }

	record IntegerItem(int Entry) : IItem;

	record ListItem(List<IItem> Items) : IItem;

	internal enum Result
	{
		Indecisive,
		InOrder,
		OutOfOrder
	}
}