using System.Text;

Console.WriteLine("Welcome to simplex method console!\n");

string[] textLines = File.ReadAllLines("Examples/2x2.csv");

Console.WriteLine("Function to optimize:");

Console.WriteLine(BuildFunctionToOptimizeMessage(textLines[0]));

Console.WriteLine("Conditions:");

for (int i = 1; i < textLines.Length; i++)
{
	Console.WriteLine(textLines[i]);
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