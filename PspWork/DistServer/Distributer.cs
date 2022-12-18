using Common;
using Common.Models;
using DistServer.RequestHandlers;
using DistServer.Solving;
using System.Net.Sockets;

namespace DistServer
{
    public class Distributer
    {
        private readonly NetworkStream _stream;
        private readonly List<RequestSender> _senders;
        private readonly TransferClient _transferClient;

        private double[][] _matrix;
        private double[] _vector;

        public Distributer(TcpClient tcpClient, IEnumerable<RequestSender> senders)
        {
            _stream = tcpClient.GetStream();
            _senders = senders.ToList();
            _transferClient = new TransferClient();
        }

        public async Task Distribute()
        {
            var request = _transferClient.Receive<ClientRequest>(_stream);

            _matrix = request.Matrix;
            _vector = request.Vector;

            ClientResponse response;

            if (request.IsLinear)
            {
                response = new ClientResponse
                {
                    Answers = Accord.Math.Matrix.Solve(_matrix, _vector)
                }; 
            }
            else
            {
                response = await DistributeInternal();
            }

            _transferClient.Send(_stream, response);
        }

        private async Task<ClientResponse> DistributeInternal()
        {
            var lineCutter = new LineCutter();
            var tasks = new List<Task>();

            for (int i = 0; i < _matrix.Length - 1; i++)
            {
                var vectorTask = SolveForVector(i);

                var lineModels = lineCutter.GetLines(_matrix, _senders.Count, i);

                await vectorTask;

                for (int j = 0; j < lineModels.Count; j++)
                {
                    tasks.Add(SendTask(_senders[j], lineModels[j]));
                }

                await Task.WhenAll(tasks);
                tasks.Clear();
            }

            var answers = GaussCalculator.Solve(_matrix, _vector);
            
            return new ClientResponse { Answers = answers };
        }

        private async Task SendTask(RequestSender sender, LinesModel model)
        {
            var resultModel = await Task.Run(() => sender.Send(model));

            foreach (var line in resultModel.Lines)
            {
                _matrix[line.LineNumber] = line.Line;
            }
        }

        private Task SolveForVector(int iteration)
        {
            return Task.Run(() =>
            {
                var firstValue = _matrix[iteration][iteration];

                for (int i = iteration + 1; i < _vector.Length; i++)
                {
                    double k = firstValue == 0 ? 0 : _matrix[i][iteration] / firstValue;
                    _vector[i] -= k * _vector[iteration];
                }
            });
        }
    }
}
