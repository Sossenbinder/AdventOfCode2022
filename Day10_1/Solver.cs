using BenchmarkDotNet.Attributes;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsTCPIP;

namespace Day10_1
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

		private const int MaxCycle = 220;
		private const int CounterModulo = 40;

		public CPU()
		{
			_register = 1;
			_cycleCounter = 0;
			SignalStrength = 0;
		}

		public void Noop()
		{
			IncrementCycle();
		}

		public void AddX(int value)
		{
			IncrementCycle();
			IncrementCycle();
			_register += value;
		}

		private void IncrementCycle()
		{
			_cycleCounter++;

			if (_cycleCounter <= MaxCycle && _cycleCounter % CounterModulo == 20)
			{
				SignalStrength += _cycleCounter * _register;
			}
		}
	}
}