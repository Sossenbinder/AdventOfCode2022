using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace Day15_1
{
	[MemoryDiagnoser]
	public class Solver
	{
		private readonly string[] _input;

		private readonly bool _print;

		private int _searchedRow = 2000000;

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
			var inputs = Parse().ToList();
			var (field, yNormalize, xNormalize) = CreateField(inputs);

			for (var index = 0; index < inputs.Count; index++)
			{
				var sensor = inputs[index].Sensor;
				var beacon = inputs[index].Beacon;
				inputs[index] = (
					new Point(X: sensor.X + xNormalize, Y: sensor.Y + yNormalize),
					new Point(X: beacon.X + xNormalize, Y: beacon.Y + yNormalize)
				);
			}

			_searchedRow += yNormalize;

			var points = Run(field, inputs);

			if (_print)
			{
				Console.WriteLine(points.Count - 1);
				Console.Read();
			}
		}

		private (char[] Field, int yNormalize, int xNormalize) CreateField(List<(Point Sensor, Point Beacon)> sensorBeaconSet)
		{
			var allPoints = sensorBeaconSet.SelectMany(x => new List<Point>() { x.Sensor, x.Beacon }).ToList();

			var yNormalize = Math.Abs(allPoints.Min(x => x.Y));
			var xMax = allPoints.Max(x => x.X);
			const int xNormalize = 10000000;
			xMax += xNormalize;

			var field = new char[xMax * 3];

			for (var i = 0; i < field.Length; ++i)
			{
				field[i] = '.';
			}

			foreach (var pointSet in sensorBeaconSet)
			{
				if (pointSet.Beacon.Y + yNormalize == _searchedRow)
				{
					field[pointSet.Beacon.X + xNormalize] = 'B';
				}

				if (pointSet.Sensor.Y + yNormalize == _searchedRow)
				{
					field[pointSet.Sensor.X + xNormalize] = 'S';
				}
			}

			return (field, yNormalize, xNormalize);
		}

		private IEnumerable<(Point Sensor, Point Beacon)> Parse()
		{
			var regex = new Regex("Sensor at x=(-?[0-9]*\\.?[0-9]+.*), y=(-?[0-9]*\\.?[0-9]+.*): closest beacon is at x=(-?[0-9]*\\.?[0-9]+.*), y=(-?[0-9]*\\.?[0-9]+.*)", RegexOptions.Compiled);
			foreach (var line in _input)
			{
				var match = regex.Match(line);
				var sensor = new Point(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
				var beacon = new Point(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));

				yield return (sensor, beacon);
			}
		}

		private HashSet<Point> Run(char[] field, List<(Point Sensor, Point Beacon)> sensorBeaconSet)
		{
			var blockList = new HashSet<Point>();
			foreach (var (sensor, beacon) in sensorBeaconSet)
			{
				var (sensorX, sensorY) = sensor;
				var (beaconX, beaconY) = beacon;

				var manhattanDistance = Math.Abs(sensorX - beaconX) + Math.Abs(sensorY - beaconY);

				var searchedRowDiff = Math.Abs(_searchedRow - sensorY);

				var yDistance = 0;
				for (var x = sensorX - manhattanDistance; x <= sensorX + manhattanDistance; ++x)
				{
					var blocks = false;
					if (yDistance >= searchedRowDiff)
					{
						blocks = field[x] != 'B';
					}

					if (blocks)
					{
						blockList.Add(new Point(x, _searchedRow));
					}

					if (x >= sensorX)
					{
						yDistance--;
					}
					else
					{
						yDistance++;
					}
				}
			}

			return blockList;
		}

		private record struct Point(int X, int Y);
	}
}