using System.Text;

Console.WriteLine("Welcome to simplex method console!\n");

string[] textLines = File.ReadAllLines("Examples/2x2.csv");

Console.WriteLine("Function to optimize:");

Console.WriteLine(BuildFunctionToOptimizeMessage(textLines[0]));

Console.WriteLine();

Console.WriteLine("Conditions:");

for (int i = 2; i < textLines.Length; i++)
{
	Console.WriteLine(BuildRestrictionMessage(textLines[i]));
}

Console.ReadLine();

string BuildFunctionToOptimizeMessage(string functionTextLine)
{
	string[] coefficients = functionTextLine.Split([',']);

	StringBuilder sb = new($"F = {coefficients[0]}*x1");

	for (int i = 1; i < coefficients.Length; i++)
	{
		string sign = coefficients[i][0] == '-' ? "" : "+";
		sb.Append($"{sign}{coefficients[i]}*x{i + 1}");
	}

	sb.Append(" -> Max");

	return sb.ToString();
}

string BuildRestrictionMessage(string restrictionTextLine)
{
	string[] coefficients = restrictionTextLine.Split([',']);

	StringBuilder sb = new();

	for (int i = 0;  i < coefficients.Length - 1; i++)
	{
		string sign = coefficients[i][0] == '-' ? "" : "+";
		sb.Append($"{sign}{coefficients[i]}*x{i + 1}");
	}

	sb.Append($" <= {coefficients[coefficients.Length - 1]}");

	return sb.ToString();
}