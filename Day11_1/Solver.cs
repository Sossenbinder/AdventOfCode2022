using BenchmarkDotNet.Attributes;

namespace Day11_1
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
			var monkeys = Parser.Parse(_input).ToArray();

			for (var i = 0; i < 20; ++i)
			{
				foreach (var monkey in monkeys)
				{
					monkey.PerformTurn(monkeys);
				}
			}

			var topInspections = monkeys
				.Select(x => x.TotalInspections)
				.OrderDescending()
				.Take(2)
				.ToArray();

			var monkeyBusiness = topInspections[0] * topInspections[1];

			if (_print)
			{
				Console.WriteLine(monkeyBusiness);
				Console.ReadLine();
			}
		}
	}

	internal class Monkey
	{
		public Queue<int> Items { get; set; }

		public Func<int, int> Operation { get; set; }

		public int TestDivisor { get; set; }

		public int SuccessDestination { get; set; }

		public int FailureDestination { get; set; }

		public int TotalInspections { get; private set; }

		public void PerformTurn(Monkey[] monkeys)
		{
			while (Items.TryDequeue(out var item))
			{
				HandleItem(monkeys, item);
			}
		}

		private void HandleItem(Monkey[] monkeys, int item)
		{
			TotalInspections++;
			var postInspectWorryLevel = Operation(item);
			postInspectWorryLevel /= 3;

			var destination = postInspectWorryLevel % TestDivisor == 0 ? SuccessDestination : FailureDestination;

			monkeys[destination].Items.Enqueue(postInspectWorryLevel);
		}
	}

	internal static class Parser
	{
		public static IEnumerable<Monkey> Parse(string[] input)
		{
			var monkeyDataChunks = input.Chunk(7).ToArray();

			foreach (var monkeyData in monkeyDataChunks)
			{
				var monkey = new Monkey();

				for (var j = 0; j < monkeyData.Length; ++j)
				{
					var line = monkeyData[j];
					switch (j)
					{
						case 1:
							monkey.Items = new(line[18..].Split(", ").Select(int.Parse));
							break;

						case 2:
							monkey.Operation = ParseOperation(line);
							break;

						case 3:
							monkey.TestDivisor = ParseDivisor(line);
							break;

						case 4:
							monkey.SuccessDestination = ParseDestination(line);
							break;

						case 5:
							monkey.FailureDestination = ParseDestination(line);
							break;
					}
				}

				yield return monkey;
			}
		}

		private static Func<int, int> ParseOperation(string line)
		{
			var operation = line[20..];

			var operationSplit = operation.Split(" ");

			Func<int, int, int> operand = operationSplit[1][0] switch
			{
				'+' => (@base, val) => @base + val,
				'*' => (@base, val) => @base * val,
			};

			int? factor = null;
			if (int.TryParse(operationSplit[2], out var parsedFactor))
			{
				factor = parsedFactor;
			}

			return old => operand(old, factor ?? old);
		}

		private static int ParseDivisor(string line)
		{
			var divisor = line[21..];
			return int.Parse(divisor);
		}

		private static int ParseDestination(string line)
		{
			return line[^1] - '0';
		}
	}
}