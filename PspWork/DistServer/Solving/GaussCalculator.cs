namespace DistServer.Solving
{
    public static class GaussCalculator
    {
        public static double[] Solve(double[][] matrix, double[] vector)
        {
            var x = new double[matrix.Length];
            var lastIndex = matrix.Length - 1;

            var lastX = vector[lastIndex] / matrix[lastIndex][lastIndex];
            x[lastIndex] = lastX;

            for (int i = lastIndex - 1; i >= 0; i--)
            {
                var localY = vector[i];
                int localJ = lastIndex;

                while (localJ >= i)
                {
                    localY -= matrix[i][localJ] * x[localJ];
                    localJ--;
                }

                if (Math.Round(matrix[i][i], 5) != 0)
                {
                    x[i] = localY / matrix[i][i];
                }
            }

            return x.Select(x => x).ToArray();
        }
    }
}
