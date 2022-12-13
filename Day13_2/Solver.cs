using BenchmarkDotNet.Attributes;

namespace Day13_2
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
			var entries = new List<IItem>
			{
				Parse(_input[0])
			};

			for (var i = 1; i < _input.Length; ++i)
			{
				if ((i + 1) % 3 == 0)
				{
					continue;
				}

				entries.Add(Parse(_input[i]));
			}

			var dividerPacketTwo = Parse("[[2]]");
			var dividerPacketSix = Parse("[[6]]");

			entries.Add(dividerPacketTwo);
			entries.Add(dividerPacketSix);

			entries.Sort((l, r) =>
			{
				var comparisonResult = Compare(l, r);

				return comparisonResult switch
				{
					Result.Indecisive => 0,
					Result.InOrder => -1,
					Result.OutOfOrder => 1,
					_ => throw new ArgumentOutOfRangeException()
				};
			});

			var twoIndex = entries.FindIndex(x => Compare(x, dividerPacketTwo) == Result.Indecisive) + 1;
			var sixIndex = entries.FindIndex(x => Compare(x, dividerPacketSix) == Result.Indecisive) + 1;

			if (_print)
			{
				Console.WriteLine(twoIndex * sixIndex);
				Console.ReadLine();
			}
		}

		private static IItem Parse(string line)
		{
			var item = new ListItem(new List<IItem>());

			var subItemStack = new Stack<IItem>();
			subItemStack.Push(item);

			List<char> currentChar = new();

			void FlushCharBuffer()
			{
				var number = int.Parse(new string(currentChar.ToArray()));
				((ListItem)subItemStack.Peek()).Items.Add(new IntegerItem(number));
				currentChar.Clear();
			}

			foreach (var chr in line[1..^1])
			{
				var currentItem = subItemStack.Peek();

				if (chr == ',')
				{
					if (currentChar.Count > 0)
					{
						FlushCharBuffer();
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
							FlushCharBuffer();
						}
						subItemStack.Pop();
						break;

					default:
						currentChar.Add(chr);
						break;
				}
			}

			if (currentChar.Count > 0)
			{
				FlushCharBuffer();
			}
			return item;
		}

		private static Result Compare(IItem left, IItem right)
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