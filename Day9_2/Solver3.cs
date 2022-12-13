#undef VISUALIZATION

using BenchmarkDotNet.Attributes;

namespace Day9_2
{
	[MemoryDiagnoser]
	public class Solver3
	{
		private readonly string[] _input;

		public Solver3()
		{
			_input = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt"));
		}

		[Benchmark]
		public void Solve()
		{
			var headPoint = new Point(0, 0, true);
			var visits = new HashSet<Point> { headPoint };

			const int tailSize = 9;
			var tailPositions = Enumerable.Range(0, tailSize).Select(_ => headPoint).ToList();
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

			static Point CorrectSpecialDistances(Point newPosition, Point tailPosition, Point headPos)
			{
				var (newX, newY, _) = newPosition;
				var (tailX, tailY, _) = tailPosition;

				var xDiff = Math.Abs(newPosition.X - tailPosition.X);
				var yDiff = Math.Abs(newPosition.Y - tailPosition.Y);

				if (xDiff >= 2)
				{
					var correction = newPosition.X > tailPosition.X ? 1 : -1;
					headPos = headPos with { X = tailPosition.X + correction };

					if (yDiff != 0)
					{
						var yCorrection = newY > tailY ? 1 : -1;
						headPos = headPos with { Y = tailY + yCorrection };
					}
				}

				if (yDiff >= 2)
				{
					var correction = newPosition.Y > tailPosition.Y ? 1 : -1;
					headPos = headPos with { Y = tailPosition.Y + correction };

					if (xDiff != 0)
					{
						var xCorrection = newX > tailX ? 1 : -1;
						headPos = headPos with { X = tailX + xCorrection };
					}
				}

				return headPos;
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
					for (var j = 0; j < tailSize; ++j)
					{
						var tailPosition = tailPositions[j];

						if (OutOfRange(newPosition, tailPosition))
						{
							headPos = CorrectSpecialDistances(newPosition, tailPositions[j], headPos);
							tailPositions[j] = headPos with { Head = false };
						}
						else
						{
							headPos = tailPosition;
						}

						newPosition = headPos;
						headPos = tailPositions[j];
					}
					tailVisits.Add(tailPositions[8]);
				}

#if VISUALIZATION
				Console.Clear();
				//VisualizeHead(new ListItem<Point>() { headPoint });
				//Console.WriteLine();
				var pointList = tailPositions.ToList();
				pointList.Add(headPoint);
				VisualizeTails(pointList);
				Console.WriteLine();
				VisualizeHead(tailVisits);

				//Console.ReadLine();
#endif
			}

			//VisualizeHead(tailVisits);
			Console.WriteLine(tailVisits.Count);
			Console.ReadLine();
		}

		private static void VisualizeHead(IEnumerable<Point> points)
		{
			var anchorPoint = new Point(10, 10);

			var pointsList = points.ToList();
			pointsList.Add(anchorPoint);
			var pointsArray = pointsList.ToArray();
			var xFrom = pointsArray.Min(x => x.X);
			var xTo = pointsArray.Max(x => x.X) + 1;
			var yFrom = pointsArray.Min(x => x.Y);
			var yTo = pointsArray.Max(x => x.Y) + 1;

			pointsList = points.ToList();

			var xSize = Math.Abs(xFrom - xTo);
			var ySize = Math.Abs(yFrom - yTo);

			var arr = new char[ySize][];
			for (var i = 0; i < ySize; ++i)
			{
				arr[i] = new char[xSize];
				for (var j = 0; j < xSize; ++j)
				{
					arr[i][j] = '.';
				}
			}

			var xTransposition = Math.Abs(xFrom);
			var yTransposition = Math.Abs(yFrom);
			foreach (var point in pointsList)
			{
				arr[point.Y + yTransposition][point.X + xTransposition] = '#';
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
			var anchorPoint = new Point(10, 10);

			var pointsList = points.ToList();
			pointsList.Add(anchorPoint);
			var pointsArray = pointsList.ToArray();
			var xFrom = Math.Min(0, pointsArray.Min(x => x.X));
			var xTo = pointsArray.Max(x => x.X) + 1;
			var yFrom = Math.Min(0, pointsArray.Min(x => x.Y));
			var yTo = pointsArray.Max(x => x.Y) + 1;

			var xSize = Math.Abs(xFrom - xTo);
			var ySize = Math.Abs(yFrom - yTo);

			var arr = new char[ySize][];
			for (var i = 0; i < ySize; ++i)
			{
				arr[i] = new char[xSize];
				for (var j = 0; j < xSize; ++j)
				{
					arr[i][j] = '.';
				}
			}

			var xTransposition = Math.Abs(xFrom);
			var yTransposition = Math.Abs(yFrom);
			for (var index = 0; index < pointsArray.Length - 1; index++)
			{
				var point = pointsArray[index];

				arr[point.Y + yTransposition][point.X + xTransposition] = point.Head ? 'H' : (char)(index + 49);
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

		record struct Point(int X, int Y, bool Head = false);
	}
}