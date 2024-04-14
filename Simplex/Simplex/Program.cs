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

	// populate 1 for initial basis
	matrix[i - 2][(coefficients.Length - 1) + (i - 2)] = 1;

	for (int j = 0; j < coefficients.Length - 1; j++)
	{
		matrix[i - 2][j] = Convert.ToDouble(coefficients[j]);
	}

	Console.WriteLine($"{leftPart}+y{i - 1} = {restriction}");
}

// algorythm beginning
int[] basis = InitializeBasis(mainFunctionCoefficients.Length);

double[] functionRow = matrix[textLines.Length - 2];

(double minimum, int resolvingColumnIndex) = IsOptimalPlan(functionRow);

int iteration = 0;

while (minimum < 0)
{
	iteration++;

	// recalculation of main matrix cycle
	int resolvingRowIndex = 0;
	double bMin = b[0];

	for (int i = 0; i < b.Length - 1; i++)
	{
		double minTemp = b[i] / matrix[i][resolvingColumnIndex];
		if (minTemp < bMin && minTemp > 0)
		{
			bMin = b[i];
			resolvingRowIndex = i;
		}
	}

	double[][] newMatrix = UpdateMainMatrixByResolvingRowAndColumn(matrix, b, resolvingRowIndex, resolvingColumnIndex);

	double[] resolvingRow = matrix[resolvingRowIndex];

	int basisIndexToExclude = GetBasisIndexToExclude(basis, resolvingRow);

	// replace basis indices
	for (int i = 0; i < basis.Length; i++)
	{
		if (basis[i] ==  basisIndexToExclude)
		{
			basis[i] = resolvingColumnIndex;
		}
	}

	// replace matrix with recalculated values
	matrix = newMatrix;

	double[] newFunctionRow = matrix[textLines.Length - 2];

	(minimum, resolvingColumnIndex) = IsOptimalPlan(newFunctionRow);
}

Console.ReadLine();





int[] InitializeBasis(int length)
{
	int[] basis = new int[length];
	for (int i = 0; i < length; i++)
	{
		basis[i] = length + i;
	}

	return basis;
}

int GetBasisIndexToExclude(int[] basis, double[] resolvingRow)
{
	int basisItemToExclude = 0;

	for (int i = basis.Length; i < resolvingRow.Length; i++)
	{
		if (resolvingRow[i] > 0)
		{
			basisItemToExclude = i;
			break;
		}
	}

	return basisItemToExclude;
}

double[][] UpdateMainMatrixByResolvingRowAndColumn(double[][] matrix, double[] b, int resolvingRowIndex, int resolvingColumnIndex)
{
	double[][] newMatrix = new double[matrix.Length][];

	// resolving row stays the same
	newMatrix[resolvingRowIndex] = new double[matrix[resolvingRowIndex].Length];
	matrix[resolvingRowIndex].CopyTo(newMatrix[resolvingRowIndex], 0);

	//
	double resolvingElementValue = matrix[resolvingRowIndex][resolvingColumnIndex];

	// make 0 for all resolving column items except resolving element at resolving row index

	for (int i = 0; i < newMatrix.Length; i++)
	{
		if (i == resolvingRowIndex) { continue; }
		newMatrix[i] = new double[matrix[i].Length];

		// calculate coefficient to make all column items 0 except resolving element
		double coefficientToApplyToRow = - (matrix[i][resolvingColumnIndex] / resolvingElementValue);

		for (int j = 0; j < newMatrix[i].Length; j++)
		{
			//if (j == resolvingColumnIndex) { continue; };
			newMatrix[i][j] = matrix[i][j] + coefficientToApplyToRow * matrix[resolvingRowIndex][j];
		}

		b[i] = b[resolvingRowIndex] * coefficientToApplyToRow + b[i];
	}

	return newMatrix;
}

(double, int) IsOptimalPlan(double[] function)
{
	double minimum = function[0];
	int resolvingColumnIndex = 0;

	for (int i = 0; i < function.Length; i++)
	{
		if (function[i] < minimum)
		{
			minimum = function[i];
			resolvingColumnIndex = i;
		}
	}

	return (minimum, resolvingColumnIndex);
}

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