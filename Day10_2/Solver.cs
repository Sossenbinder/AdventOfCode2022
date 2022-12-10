using BenchmarkDotNet.Attributes;

namespace Day10_2
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
			var cpu = new CPU();

			foreach (var input in _input)
			{
				switch (input[..4])
				{
					case "noop":
						cpu.Noop();
						break;
					case "addx":
						cpu.AddX(int.Parse(input[5..]));
						break;
				}
			}
		}
	}

	public struct CPU
	{
		public int SignalStrength { get; set; }

		private int _cycleCounter;
		private int _register;
		
		private const int CounterModulo = 40;

		private Queue<char> _crtRow;

		public CPU()
		{
			_register = 1;
			_cycleCounter = 0;
			SignalStrength = 0;
			_crtRow = new();
		}

		public void Noop()
		{
			PerformCycle();
		}

		public void AddX(int value)
		{
			PerformCycle();
			PerformCycle();
			_register += value;
		}

		private void PerformCycle()
		{
			_cycleCounter++;
			HandleSprite();

			if (_cycleCounter % CounterModulo != 0)
			{
				return;
			}

			PrintSprite();
			_crtRow = new();
		}

		private void HandleSprite()
		{
			var spriteStart = _register - 1;
			var spriteEnd = _register + 1;

			var crtPosition = _crtRow.Count;
			_crtRow.Enqueue(crtPosition <= spriteEnd && crtPosition >= spriteStart ? '#' : '.');
		}

		private void PrintSprite()
		{
			while (_crtRow.TryDequeue(out var result))
			{
				Console.Write(result);
			}

			Console.WriteLine();
		}
	}
}