using BenchmarkDotNet.Attributes;

namespace Day12_2
{
	[MemoryDiagnoser]
	public class Solver
	{
		private readonly char[][] _input;

		private readonly bool _print;

		public Solver()
		{
			_input = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt")).Select(x => x.ToCharArray()).ToArray();
		}

		public Solver(bool doPrint)
			: this()
		{
			_print = doPrint;
		}

		[Benchmark]
		public void Solve()
		{
			var startPoint = FindChar('S');
			var endPoint = FindChar('E');
			_input[startPoint.Y][startPoint.X] = (char)('a' - 1);
			_input[endPoint.Y][endPoint.X] = (char)('z' + 1);

			var aPositions = FindAPositions().ToList();
			aPositions.Add(startPoint);

			var minResult = aPositions
				.Select(Traverse)
				.Where(x => x > 0)
				.Min();

			if (_print)
			{
				Console.WriteLine(minResult);
				Console.ReadLine();
			}
		}

		private int Traverse(Point point)
		{
			var openPoints = new List<Point>() { point };

			var closedPoints = new List<Point>() { point };

			var printIndex = 0;
			var mod = 999999;
			while (openPoints.Any())
			{
				printIndex++;
				if (_print && printIndex % mod == 0)
				{
					Console.Read();
					Console.Clear();
					PrintOpenPoints(openPoints);
				}
				var currentPoint = openPoints.MinBy(pt => pt.Cost);
				openPoints.Remove(currentPoint);

				var (x, y) = currentPoint;

				var value = _input[y][x];

				if (value == 'z' + 1)
				{
					return currentPoint.Cost;
				}

				var subNodes = new List<Point>();

				// Up
				if (y > 0 && (_input[y - 1][x] <= value + 1))
				{
					subNodes.Add(new Point(x, y - 1));
				}

				// Down
				if (y < _input.Length - 1 && (_input[y + 1][x] <= value + 1))
				{
					subNodes.Add(new Point(x, y + 1));
				}

				// Left
				if (x > 0 && (_input[y][x - 1] <= value + 1))
				{
					subNodes.Add(new Point(x - 1, y));
				}

				// Right
				if (x < _input[0].Length - 1 && (_input[y][x + 1] <= value + 1))
				{
					subNodes.Add(new Point(x + 1, y));
				}

				// Remove self since I'm too lazy to write conditions
				subNodes.Remove(point);

				foreach (var subNode in subNodes)
				{
					var costedSubNode = subNode with { Cost = currentPoint.Cost + 1 };

					if (openPoints.Any(pt => pt.X == costedSubNode.X && pt.Y == costedSubNode.Y && pt.Cost <= costedSubNode.Cost))
					{
						continue;
					}

					if (closedPoints.Any(pt => pt.X == costedSubNode.X && pt.Y == costedSubNode.Y && pt.Cost <= costedSubNode.Cost))
					{
						continue;
					}

					openPoints.Add(costedSubNode);
				}

				if (openPoints.Count == 1)
				{
				}

				closedPoints.Add(currentPoint);
			}

			return 0;
		}

		private Point FindChar(char find)
		{
			for (var y = 0; y < _input.Length; y++)
			{
				for (var x = 0; x < _input[y].Length; x++)
				{
					if (_input[y][x] == find)
					{
						return new Point(x, y)
						{
							Cost = 0
						};
					}
				}
			}

			throw new ArgumentException("Couldn't find start point");
		}

		private void PrintOpenPoints(List<Point> openPoints)
		{
			for (var y = 0; y < _input.Length; y++)
			{
				for (var x = 0; x < _input[y].Length; x++)
				{
					Console.Write(openPoints.Any(p => p.X == x && p.Y == y) ? '#' : '.');
				}

				Console.WriteLine();
			}
		}

		private IEnumerable<Point> FindAPositions()
		{
			for (var y = 0; y < _input.Length; y++)
			{
				for (var x = 0; x < _input[y].Length; x++)
				{
					if (_input[y][x] == 'a')
					{
						yield return new Point(x, y)
						{
							Cost = 0
						};
					}
				}
			}
		}
	}

	record struct Point(int X, int Y)
	{
		public int Cost { get; set; }
	};
}