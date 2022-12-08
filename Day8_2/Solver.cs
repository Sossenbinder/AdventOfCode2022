using BenchmarkDotNet.Attributes;

namespace Day8_2
{
	[MemoryDiagnoser]
	public class Solver
	{
		private readonly string[] _input;

		public Solver()
		{
			_input = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt"));
		}

		record struct Point(int X, int Y);

		[Benchmark]
		public void Solve()
		{
			var spottedTrees = new HashSet<Point>();

			// Horizontal
			for (var i = 1; i < _input.Length - 1; ++i)
			{
				var slider = 0;
				for (var j = 0; j < _input[i].Length; ++j)
				{
					var value = _input[i][j] - '0';
					if (value > slider && j > 0 && j < _input[i].Length - 1)
					{
						spottedTrees.Add(new(j, i));
					}

					if (value > slider)
					{
						slider = value;
					}
				}

				slider = 0;
				for (var j = _input[i].Length - 1; j >= 0; --j)
				{
					var value = _input[i][j] - '0';
					if (value > slider && j > 0 && j < _input[i].Length - 1)
					{
						spottedTrees.Add(new(j, i));
					}

					if (value > slider)
					{
						slider = value;
					}
				}
			}

			// Vertical
			for (var i = 1; i < _input[0].Length - 1; ++i)
			{
				var slider = 0;
				for (var j = 0; j < _input.Length; ++j)
				{
					var value = _input[j][i] - '0';
					if (value > slider && j > 0 && j < _input.Length - 1)
					{
						spottedTrees.Add(new(i, j));
					}

					if (value > slider)
					{
						slider = value;
					}
				}

				slider = 0;
				for (var j = _input.Length - 1; j >= 0; --j)
				{
					var value = _input[j][i] - '0';
					if (value > slider && j > 0 && j < _input.Length - 1)
					{
						spottedTrees.Add(new(i, j));
					}

					if (value > slider)
					{
						slider = value;
					}
				}
			}
			// Sliding window algorithm

			var max = spottedTrees.Select(CalculateScenicScore).Max();

			int CalculateScenicScore(Point point)
			{
				var (x, y) = point;
				var originHeight = _input[y][x] - '0';

				var up = 1;
				for (var yPos = y - 1; yPos > 0; yPos--)
				{
					if (originHeight <= _input[yPos][x] - '0')
					{
						break;
					}

					up++;
				}

				var down = 1;
				for (var yPos = y + 1; yPos < _input.Length - 1; yPos++)
				{
					if (originHeight <= _input[yPos][x] - '0')
					{
						break;
					}

					down++;
				}

				var left = 1;
				for (var xPos = x - 1; xPos > 0; xPos--)
				{
					if (originHeight <= _input[y][xPos] - '0')
					{
						break;
					}

					left++;
				}

				var right = 1;
				for (var xPos = x + 1; xPos < _input.Length - 1; xPos++)
				{
					if (originHeight <= _input[y][xPos] - '0')
					{
						break;
					}

					right++;
				}

				return up * down * left * right;
			}
		}
	}
}