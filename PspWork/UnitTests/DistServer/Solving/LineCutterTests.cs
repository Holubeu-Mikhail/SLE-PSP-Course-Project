using DistServer.Solving;
using NUnit.Framework;

namespace UnitTests.DistServer.Solving
{
    public class LineCutterTests
    {
        private double[][] _matrix;

        [SetUp]
        public void Setup()
        {
            _matrix = new double[5][];
            _matrix[0] = new double[] { 1, 2, 0, 4, 5 };
            _matrix[1] = new double[] { 1, 5, 5, 4, 5 };
            _matrix[2] = new double[] { 1, 2, 3, 4, 5 };
            _matrix[3] = new double[] { 1, 2, 3, 4, 5 };
            _matrix[4] = new double[] { 1, 2, 3, 4, 5 };
        }

        [Test]
        public void GetLines_WhenExistSomeServers_ShouldReturnCorrectValue()
        {
            var serversCount = 2;
            var lineCutter = new LineCutter();

            var result = lineCutter.GetLines(_matrix, serversCount, 0);

            Assert.AreEqual(serversCount, result.Count);
            CollectionAssert.AreEqual(new double[] { 1, 2, 0, 4, 5 }, result.First().SolveRow);
        }

        [Test]
        public void Test()
        {
            var serversCount = 3;
            var lineCutter = new LineCutter();

            var result = lineCutter.GetLines(_matrix, serversCount, 0);

            Assert.AreEqual(serversCount, result.Count);
            CollectionAssert.AreEqual(new double[] { 1, 2, 0, 4, 5 }, result.First().SolveRow);
        }
    }
}
