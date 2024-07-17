namespace Simplex
{
	public static class Algorythm
	{
		public static (double[], double) Simplex(string[] mainFunctionCoefficients, double[][] matrix, double[] b)
		{
			int[] basis = InitializeBasis(mainFunctionCoefficients.Length);

			double[] functionRow = matrix[mainFunctionCoefficients.Length];

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
					if (basis[i] == basisIndexToExclude)
					{
						basis[i] = resolvingColumnIndex;
					}
				}

				// replace matrix with recalculated values
				matrix = newMatrix;

				double[] newFunctionRow = matrix[mainFunctionCoefficients.Length];

				(minimum, resolvingColumnIndex) = IsOptimalPlan(newFunctionRow);
			}

			double[] results = new double[basis.Length];

			// form the result
			for (int i = 0; i < matrix.Length - 1; i++)
			{
				for (int j = 0; j < basis.Length; j++)
				{
					if (matrix[i][basis[j]] > 0)
					{
						results[i] = b[i] / matrix[i][basis[j]];
					}
				}
			}

			double maximumFunctionValue = b[b.Length - 1];

			return (results, maximumFunctionValue);
		}

		private static (double, int) IsOptimalPlan(double[] function)
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

		private static int[] InitializeBasis(int length)
		{
			int[] basis = new int[length];
			for (int i = 0; i < length; i++)
			{
				basis[i] = length + i;
			}

			return basis;
		}

		private static double[][] UpdateMainMatrixByResolvingRowAndColumn(double[][] matrix, double[] b, int resolvingRowIndex, int resolvingColumnIndex)
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
				double coefficientToApplyToRow = -(matrix[i][resolvingColumnIndex] / resolvingElementValue);

				for (int j = 0; j < newMatrix[i].Length; j++)
				{
					newMatrix[i][j] = matrix[i][j] + coefficientToApplyToRow * matrix[resolvingRowIndex][j];
				}

				b[i] = b[resolvingRowIndex] * coefficientToApplyToRow + b[i];
			}

			return newMatrix;
		}

		private static int GetBasisIndexToExclude(int[] basis, double[] resolvingRow)
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
	}
}
