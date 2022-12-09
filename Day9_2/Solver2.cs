#define VISUALIZATION

using BenchmarkDotNet.Attributes;

namespace Day9_2
{
	[MemoryDiagnoser]
	public class Solver2
	{
		private readonly string[] _input;

		public Solver2()
		{
			_input = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt"));
		}

		[Benchmark]
		public void Solve()
		{
			var headPoint = new Point(0, 0);
			var visits = new HashSet<Point> { headPoint };

			var tailPositions = Enumerable.Range(0, 10).Select(_ => headPoint).ToList();
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

			static bool OutOfRange(Point newPosition, Point tailPosition)
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
					var headPos = headPoint;
					// Handle head
					headPoint = MoveHead(inputSpan[0]);
					visits.Add(headPoint);

					var newPosition = headPoint;

					// Handle tail
					for (var j = 0; j < 10; ++j)
					{
						var tailPosition = tailPositions[j];

						if (OutOfRange(newPosition, tailPosition))
						{
							tailPositions[j] = headPos;
						}
						else
						{
							headPos = tailPosition;
						}

						newPosition = headPos;
						headPos = tailPosition;
#if VISUALIZATION
						Console.Clear();
						VisualizeHead(visits);
						Console.WriteLine();
						VisualizeTails(tailPositions);
						Console.WriteLine();
						VisualizeHead(tailVisits);
#endif
					}
					//Console.ReadLine();
					tailVisits.Add(tailPositions[9]);
				}
			}

			Console.WriteLine(tailVisits.Count);
			Console.ReadLine();
		}

		private static void VisualizeHead(IEnumerable<Point> points)
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

		private void VisualizeTails(IEnumerable<Point> points)
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

			var pointsArray = points.ToArray();
			for (var index = 0; index < pointsArray.Length; index++)
			{
				var point = pointsArray[index];

				arr[point.Y][point.X] = (char)(index + 48);
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