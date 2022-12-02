var input = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt"));

var bestCalorieCount = 0;
var elfIndex = 1;
var calorieCount = 0;
foreach (var dataRow in input)
{
	if (int.TryParse(dataRow, out var rowCalorie))
	{
		calorieCount += rowCalorie;
	}
	else
	{
		if (calorieCount > bestCalorieCount)
		{
			bestCalorieCount = calorieCount;
		}

		calorieCount = 0;
		elfIndex++;
	}
}

Console.WriteLine(bestCalorieCount);
Console.ReadLine();