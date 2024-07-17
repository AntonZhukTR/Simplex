namespace Simplex
{
	public static class FileUtility
	{
		public static (string[], double[][], double[]) ReadMatrixAndPrepareCoefficients(string[] matrixFileContent)
		{
			double[] b = new double[matrixFileContent.Length - 1];
			b[matrixFileContent.Length - 2] = 0;

			double[][] matrix = new double[matrixFileContent.Length - 1][];

			string[] mainFunctionCoefficients = matrixFileContent[0].Split([',']);

			matrix[matrixFileContent.Length - 2] = new double[2 * mainFunctionCoefficients.Length];

			for (int i = 0; i < mainFunctionCoefficients.Length; i++)
			{
				matrix[matrixFileContent.Length - 2][i] = -Convert.ToDouble(mainFunctionCoefficients[i]);
			}

			for (int i = 2; i < matrixFileContent.Length; i++)
			{
				string[] coefficients = matrixFileContent[i].Split([',']);

				string restriction = coefficients[coefficients.Length - 1];
				b[i - 2] = Convert.ToDouble(restriction);


				matrix[i - 2] = new double[2 * (coefficients.Length - 1)];

				// populate 1 for initial basis
				matrix[i - 2][(coefficients.Length - 1) + (i - 2)] = 1;

				for (int j = 0; j < coefficients.Length - 1; j++)
				{
					matrix[i - 2][j] = Convert.ToDouble(coefficients[j]);
				}
			}

			return (mainFunctionCoefficients, matrix, b);
		}
	}
}
