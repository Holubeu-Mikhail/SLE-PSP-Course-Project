namespace Common.Models
{
    public class LinesModel
    {
        public int Iteration { get; set; }

        public double[] SolveRow { get; set; }

        public List<LineItem> Lines { get; set; }

        public class LineItem
        {
            public double[] Line { get; set; }

            public int LineNumber { get; set; }
        }
    }
}
