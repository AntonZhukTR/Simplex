using System.Text;

Console.WriteLine("Welcome to simplex method console!\n");

string[] textLines = File.ReadAllLines("Examples/2x2.csv");

Console.WriteLine($"Function to optimize:  {BuildFunctionToOptimizeMessage(textLines[0])}");
Console.WriteLine($"Function with zero in right part:  {BuildFunctionToOptimizeWithZeroMessage(textLines[0])}");
Console.WriteLine();
Console.WriteLine("Conditions:");

double[] b = new double[textLines.Length - 1];
b[textLines.Length - 2] = 0;

double[][] matrix = new double[textLines.Length - 1][];

string[] mainFunctionCoefficients = textLines[0].Split([',']);

matrix[textLines.Length - 2] = new double[2 * mainFunctionCoefficients.Length];

for (int i =  0; i < mainFunctionCoefficients.Length; i++)
{
	matrix[textLines.Length - 2][i] =  - Convert.ToDouble(mainFunctionCoefficients[i]);
}

for (int i = 2; i < textLines.Length; i++)
{
	string[] coefficients = textLines[i].Split([',']);
	string leftPart = BuildLeftPartOfRestrictionEquation(coefficients);

	string restriction = coefficients[coefficients.Length - 1];
	b[i - 2] = Convert.ToDouble(restriction);


	matrix[i - 2] = new double[2 * (coefficients.Length - 1)];
	for (int j = 0; j < coefficients.Length - 1; j++)
	{
		matrix[i - 2][j] = Convert.ToDouble(coefficients[j]);
	}

	Console.WriteLine($"{leftPart}+y{i - 1} = {restriction}");
}

// algorythm
// check for optimal plan
double[] lastRowOfMatrix = matrix[textLines.Length - 2];

double minimum = lastRowOfMatrix[0];
int resolvingColumnIndex = 0;

for (int i = 0;i < lastRowOfMatrix.Length; i++)
{
	if (lastRowOfMatrix[i] < minimum)
	{
		minimum = lastRowOfMatrix[i];
		resolvingColumnIndex = i;
	}
}

if (minimum > 0)
{
	// plan is optimal, solution is found
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

string BuildFunctionToOptimizeWithZeroMessage(string functionTextLine)
{
	string[] coefficients = functionTextLine.Split([',']);

	StringBuilder sb = new($"F ");

	for (int i = 0; i < coefficients.Length; i++)
	{
		sb.Append(BuildNegativeCoefficientText(coefficients[i], i));
	}

	sb.Append(" = 0");

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

string BuildNegativeCoefficientText(string coefficient, int index)
{
	string sign = coefficient[0] == '-' ? "+" : "-";
	coefficient = $"{sign}{coefficient}*x{index + 1}";

	return coefficient;
}