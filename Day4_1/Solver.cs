using BenchmarkDotNet.Attributes;

namespace Day4_1
{
	[MemoryDiagnoser]
	public class Solver
	{
		private readonly string[] _input;

		public Solver()
		{
			_input = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt"));
		}

		[Benchmark]
		public void Solve()
		{
			var fullMatchCounter = 0;
			foreach (var line in _input)
			{
				var span = line.AsSpan();
				var separatorIndex = span.IndexOf(',');

				var firstRange = ParseRange(span[..separatorIndex]);
				var secondRange = ParseRange(span[(separatorIndex + 1)..]);

				if (RangeIntersects(firstRange, secondRange) || RangeIntersects(secondRange, firstRange))
				{
					fullMatchCounter++;
				}
			}

			static Range ParseRange(ReadOnlySpan<char> input)
			{
				var dashIndex = input.IndexOf('-');

				var start = int.Parse(input[..dashIndex]);
				var end = int.Parse(input[(dashIndex + 1)..]);

				return start..end;
			}

			static bool RangeIntersects(Range range1, Range range2)
			{
				return range1.Start.Value >= range2.Start.Value && range1.End.Value <= range2.End.Value;
			}
		}
	}
}
