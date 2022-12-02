var input = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt"));

var score = 0;
foreach (var line in input)
{
	var opponentX = line[0] - 'A';
	var selfX = line[2] - 'X';

	var strategyBit = Math.Abs(selfX + 2) % 3;
	var roundScoreX = Math.Abs((opponentX + strategyBit) % 3) + 1 + selfX * 3;
	score += roundScoreX;
}

Console.WriteLine(score);
Console.ReadLine();