var input = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt"));

var score = 0;
foreach (var line in input)
{
	var opponentX = line[0] - 'A';
	var selfX = line[2] - 'X';

	var pickScore = selfX switch
	{
		2 => Math.Abs((opponentX + 1) % 3) + 1,
		1 => Math.Abs(opponentX % 3) + 1,
		_ => Math.Abs((opponentX + 2) % 3) + 1
	};

	var roundScoreX = pickScore + selfX * 3;
	score += roundScoreX;
}

Console.WriteLine(score);
Console.ReadLine();