using BenchmarkDotNet.Attributes;

namespace Day11_2
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
			var monkeys = Parser.Parse(_input).ToArray()!;

			var leastCommonDenominator = (long)monkeys.Aggregate(1, (current, monkey) => current * monkey.TestDivisor);

			for (var i = 0; i < 10000; ++i)
			{
				foreach (var monkey in monkeys)
				{
					monkey.PerformTurn(monkeys, leastCommonDenominator);
				}
			}

			var topInspections = monkeys
				.Select(x => x.TotalInspections)
				.OrderDescending()
				.Take(2)
				.ToArray();

			var monkeyBusiness = (long)topInspections[0] * (long)topInspections[1];

			if (_print)
			{
				Console.WriteLine(monkeyBusiness);
				Console.ReadLine();
			}
		}
	}

	internal class Monkey
	{
		public Queue<long> Items { get; set; }

		public Func<long, long> Operation { get; set; }

		public int TestDivisor { get; set; }

		public int SuccessDestination { get; set; }

		public int FailureDestination { get; set; }

		public int TotalInspections { get; private set; }

		public void PerformTurn(Monkey[] monkeys, long leastCommonDenominator)
		{
			while (Items.TryDequeue(out var item))
			{
				HandleItem(monkeys, item, leastCommonDenominator);
			}
		}

		private void HandleItem(Monkey[] monkeys, long item, long leastCommonDenominator)
		{
			TotalInspections++;
			var postInspectWorryLevel = Operation(item);

			// Scale down result
			postInspectWorryLevel %= leastCommonDenominator;

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
							monkey.Items = new(line[18..].Split(", ").Select(long.Parse));
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

		private static Func<long, long> ParseOperation(string line)
		{
			var operation = line[20..];

			var operationSplit = operation.Split(" ");

			Func<long, long, long> operand = operationSplit[1][0] switch
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