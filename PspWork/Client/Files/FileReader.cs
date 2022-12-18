using Common.Models;

namespace Client.Files
{
    public class FileReader
    {
        private readonly string _basePath;

        public FileReader(string basePath)
        {
            _basePath = basePath;
        }

        public ClientRequest LoadModel(string matrixFileName, string vectorFileName)
        {
            if (!File.Exists($"{_basePath}{matrixFileName}") || !File.Exists($"{_basePath}{vectorFileName}"))
            {
                throw new FileNotFoundException("No such file in Data/Input");
            }

            return new ClientRequest()
            {
                Matrix = LoadMatrixImpl(matrixFileName),
                Vector = LoadVector(vectorFileName)
            };
        }

        private double[][] LoadMatrixImpl(string matrixFileName)
        {
            var fileStream = new FileStream($"{_basePath}{matrixFileName}", FileMode.Open);
            var streamReader = new StreamReader(fileStream);
            var firstRow = streamReader.ReadLine();
            var str = System.Text.RegularExpressions.Regex.Replace(firstRow, @"\s+", " ");
            var size = str.Split(" ").Count() - 2;
            fileStream.Position = 0;

            var matrix = new double[size][];
            int i = 0;

            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                matrix[i] = ParseLine(line);
                i++;
            }

            var matrixSize = matrix.Length;
            matrix[0] = matrix[0].Skip(matrix[0].Length - matrixSize).ToArray();

            return matrix;
        }

        private double[] LoadVector(string vectorFileName)
        {
            var allLines = File.ReadAllLines($"{_basePath}{vectorFileName}");
            return allLines.Where(x => !x.Trim().Equals(string.Empty)).Select(x => Convert.ToDouble(x.Trim())).ToArray();
        }

        private double[] ParseLine(string line)
        {
            var str = System.Text.RegularExpressions.Regex.Replace(line, @"\s+", " ");
            return str.Split(" ").Where(x => !x.Trim().Equals(string.Empty)).Select(x => Convert.ToDouble(x.Trim())).ToArray();
        }
    }
}
