var input = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt"));

var score = 0;
foreach (var line in input)
{
	var opponent = line[0];
	var self = line[2];

	var roundScore = GetOutComeResult(opponent, self) + GetScoreForShape(self);
	score += roundScore;
}


static int GetScoreForShape(char shape)
{
	return shape switch
	{
		// Rock
		'X' or 'A' => 1,
		// Paper
		'Y' or 'B' => 2,
		// Scissors
		'Z' or 'C' => 3
	};
}

static int GetOutComeResult(char opponentShape, char selfShape)
{
	return opponentShape switch
	{
		'A' when selfShape == 'Y' => 6,
		'B' when selfShape == 'Z' => 6,
		'C' when selfShape == 'X' => 6,
		'A' when selfShape == 'X' => 3,
		'B' when selfShape == 'Y' => 3,
		'C' when selfShape == 'Z' => 3,
		_ => 0
	};
}

Console.WriteLine(score);
Console.ReadLine();