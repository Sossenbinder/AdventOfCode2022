#undef VISUALIZATION

using BenchmarkDotNet.Attributes;

namespace Day9_1
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
			var headPoint = new Point(0, 0);

			var tailPoint = headPoint;
			var tailVisits = new HashSet<Point>() { headPoint };

			Point MoveHead(char direction)
			{
				return direction switch
				{
					'R' => headPoint with { X = headPoint.X + 1 },
					'L' => headPoint with { X = headPoint.X - 1 },
					'U' => headPoint with { Y = headPoint.Y + 1 },
					'D' => headPoint with { Y = headPoint.Y - 1 },
					_ => throw new ArgumentOutOfRangeException()
				};
			}

			bool OutOfRange(Point newPosition, Point tailPosition)
			{
				var xDiff = Math.Abs(newPosition.X - tailPosition.X);
				var yDiff = Math.Abs(newPosition.Y - tailPosition.Y);

				return xDiff > 1 || yDiff > 1;
			}

			foreach (var input in _input)
			{
				var inputSpan = input.AsSpan();
				var strides = int.Parse(inputSpan[2..]);

				for (var i = 0; i < strides; ++i)
				{
#if VISUALIZATION
					Console.Clear();
					VisualizePosMap(visits);
					Console.WriteLine();
					VisualizePosMap(tailVisits);

					Console.ReadLine();
#endif

					var currentHead = headPoint;

					// Handle head
					headPoint = MoveHead(inputSpan[0]);

					if (!OutOfRange(headPoint, tailPoint))
					{
						continue;
					}

					// Handle tail
					tailPoint = currentHead;
					tailVisits.Add(tailPoint);
				}
			}

			Console.WriteLine(tailVisits.Count);
			Console.ReadLine();
		}

		private static void VisualizePosMap(IEnumerable<Point> points)
		{
			var arr = new char[6][];
			for (var i = 0; i < 6; ++i)
			{
				arr[i] = new char[6];
				for (var j = 0; j < 6; ++j)
				{
					arr[i][j] = '.';
				}
			}

			foreach (var point in points)
			{
				arr[point.Y][point.X] = '#';
			}

			for (var i = arr.Length - 1; i >= 0; --i)
			{
				for (var j = 0; j < arr[0].Length; ++j)
				{
					Console.Write(arr[i][j]);
				}

				Console.WriteLine();
			}
		}

		record struct Point(int X, int Y);
	}
}