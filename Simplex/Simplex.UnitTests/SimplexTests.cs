namespace Simplex.UnitTests
{
	[TestClass]
	public class SimplexTests
	{
		[TestMethod]
		public void SimplexMethod_Valid2x2Matrix_ReturnsOptimalPlan()
		{
			// Arrange
			string[] textLines = File.ReadAllLines("TestData/2x2.csv");
			(string[] mainFunctionCoefficients, double[][] matrix, double[] b) = FileUtility.ReadMatrixAndPrepareCoefficients(textLines);

			// Act
			(double[] results, double maxFunctionValue) = Algorythm.Simplex(mainFunctionCoefficients, matrix, b);

			Assert.AreEqual(19, maxFunctionValue);
			CollectionAssert.AreEqual(new double[] { 1, 4 }, results);
		}
	}
}