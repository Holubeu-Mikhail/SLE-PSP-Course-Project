using Common.Models;

namespace Solver
{
    public static class GaussSolver
    {
        public static LinesModel Solve(LinesModel model)
        {
            var iteration = model.Iteration;

            for (int i = 0; i < model.Lines.Count; i++)
            {
                double k = model.SolveRow[iteration] != 0
                    ? model.Lines[i].Line[iteration] / model.SolveRow[iteration]
                    : 0;

                for (int j = iteration; j < model.SolveRow.Length; j++)
                {
                    model.Lines[i].Line[j] -= k * model.SolveRow[j];
                }
            }

            return model;
        }
    }
}
