using Common.Models;

namespace DistServer.Solving
{
    public class LineCutter
    {
        public List<LinesModel> GetLines(double[][] matrix, int serversCount, int iteration)
        {
            var lines = new List<LinesModel>();

            for (int i = 0; i < serversCount; i++)
            {
                lines.Add(new LinesModel 
                { 
                    Iteration = iteration, 
                    SolveRow = matrix[iteration],
                    Lines = new List<LinesModel.LineItem>()
                });
            }

            for (int i = iteration + 1; i < matrix.Length;)
            {
                for (int j = 0; j < serversCount; j++, i++)
                {
                    lines[j].Lines.Add(new LinesModel.LineItem { Line = matrix[i], LineNumber = i });

                    if (i == matrix.Length - 1)
                    {
                        i = matrix.Length;
                        break;
                    }
                }
            }

            return lines;
        }
    }
}
