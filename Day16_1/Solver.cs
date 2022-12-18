using BenchmarkDotNet.Attributes;

namespace Day16_1
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
			var (normalizer, yMax, array) = Parse();
			var sourcePoint = new Point(500 - normalizer, 0);

			var result = DropSand(yMax, array, sourcePoint);
			var sandDrop = new Point(500 - normalizer, 1);
			while (result != sandDrop)
			{
				if (_print)
				{
					Visualize(array);
					Console.ReadLine();
					Console.Clear();
				}
				result = DropSand(yMax, array, sourcePoint);
			}

			var sandCount = GetSandCount(array);

			if (_print)
			{
				Console.WriteLine(sandCount);
				Console.ReadLine();
			}
		}

		private Point? DropSand(int yMax, char[][] array, Point pt)
		{
			var nextPoint = pt with { Y = pt.Y + 1 };

			if (array[nextPoint.Y][nextPoint.X] == '#' || array[nextPoint.Y][nextPoint.X] == 'o')
			{
				if (array[nextPoint.Y][nextPoint.X - 1] == '#' || array[nextPoint.Y][nextPoint.X - 1] == 'o')
				{
					if (array[nextPoint.Y][nextPoint.X + 1] == '#' || array[nextPoint.Y][nextPoint.X + 1] == 'o')
					{
						array[nextPoint.Y - 1][nextPoint.X] = 'o';
						return nextPoint;
					}

					nextPoint = nextPoint with { X = nextPoint.X + 1 };
				}
				else
				{
					nextPoint = nextPoint with { X = nextPoint.X - 1 };
				}
			}

			return DropSand(yMax, array, nextPoint);
		}

		private (int Normalize, int YMax, char[][] Array) Parse()
		{
			var rockPoints = new HashSet<Point>();
			foreach (var input in _input)
			{
				var points = new Queue<Point>(input
					.Split("->")
					.Select(x => x.Trim().Split(','))
					.Select(point => new Point(int.Parse(point[0]), int.Parse(point[1]))));

				while (points.TryDequeue(out var point))
				{
					if (!points.TryPeek(out var nextPoint))
					{
						continue;
					}

					if (point.X == nextPoint.X)
					{
						for (var y = point.Y; y != nextPoint.Y; y += (point.Y > nextPoint.Y ? -1 : 1))
						{
							rockPoints.Add(point with { Y = y });
						}
						rockPoints.Add(point with { Y = nextPoint.Y });
					}
					else if (point.Y == nextPoint.Y)
					{
						for (var x = point.X; x != nextPoint.X; x += (point.X > nextPoint.X ? -1 : 1))
						{
							rockPoints.Add(point with { X = x });
						}
						rockPoints.Add(point with { X = nextPoint.X });
					}
				}
			}

			var minX = rockPoints.Min(x => x.X);
			var maxX = rockPoints.MaxBy(x => x.X).X - minX;
			var maxY = rockPoints.MaxBy(x => x.Y).Y + 2;

			var arr = new char[maxY + 1][];

			for (var y = 0; y <= maxY; ++y)
			{
				arr[y] = new char[maxX + 1];
				for (var x = 0; x <= maxX; ++x)
				{
					arr[y][x] = ((rockPoints.Contains(new Point(x + minX, y))) || (maxY == y)) ? '#' : '.';
				}
			}

			return (minX, maxY, arr);
		}

		private int GetSandCount(char[][] array)
		{
			return array.SelectMany(x => x).Count(x => x == 'o');
		}

		private static void Visualize(IEnumerable<char[]> array)
		{
			foreach (var y in array)
			{
				foreach (var x in y)
				{
					Console.Write(x);
				}

				Console.WriteLine();
			}

			Console.WriteLine();
			Console.WriteLine("----------------");
			Console.WriteLine();
		}

		private record struct Point(int X, int Y);
	}
}