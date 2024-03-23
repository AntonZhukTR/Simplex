using System.Text;

Console.WriteLine("Welcome to simplex method console!\n");

string[] textLines = File.ReadAllLines("Examples/2x2.csv");

Console.WriteLine("Function to optimize:");
Console.WriteLine(BuildFunctionToOptimizeMessage(textLines[0]));
Console.WriteLine();
Console.WriteLine("Conditions:");

for (int i = 2; i < textLines.Length; i++)
{
	string[] coefficients = textLines[i].Split([',']);
	string leftPart = BuildLeftPartOfRestrictionEquation(coefficients);
	
	Console.WriteLine($"{leftPart}+y{i - 1} = {coefficients[coefficients.Length - 1]}");
}

Console.ReadLine();

string BuildFunctionToOptimizeMessage(string functionTextLine)
{
	string[] coefficients = functionTextLine.Split([',']);

	StringBuilder sb = new($"F = {coefficients[0]}*x1");

	for (int i = 1; i < coefficients.Length; i++)
	{
		sb.Append(BuildCoefficientText(coefficients[i], i));
	}

	sb.Append(" -> Max");

	return sb.ToString();
}

string BuildLeftPartOfRestrictionEquation(string[] coefficients)
{
	StringBuilder sb = new();

	for (int i = 0; i < coefficients.Length - 1; i++)
	{
		sb.Append(BuildCoefficientText(coefficients[i], i));
	}

	return sb.ToString();
}

string BuildCoefficientText(string coefficient, int index)
{
	string sign = coefficient[0] == '-' ? "" : "+";
	coefficient = $"{sign}{coefficient}*x{index + 1}";

	return coefficient;
}