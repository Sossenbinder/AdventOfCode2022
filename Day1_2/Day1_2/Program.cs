var input = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt"));

var calories = new List<int>();

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
		calories.Add(calorieCount);
		calorieCount = 0;
		elfIndex++;
	}
}

Console.WriteLine(calories.OrderByDescending(x => x).Take(3).Sum());
Console.ReadLine();