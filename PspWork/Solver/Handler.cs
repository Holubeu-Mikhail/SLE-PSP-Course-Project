using System.Net.Sockets;
using System.Net;
using Common;
using Common.Models;

namespace Solver
{
    internal class Handler
    {
        private readonly string _address;
        private readonly int _port;
        private readonly TransferClient _transferClient;

        public Handler(string address, int port)
        {
            _address = address;
            _port = port;
            _transferClient = new TransferClient();
        }

        public void Start()
        {
            int i = 0;
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(_address), _port);
            var tcpListener = new TcpListener(ipPoint);

            tcpListener.Start();

            while (true)
            {
                var tcpClient = tcpListener.AcceptTcpClient();

                var stream = tcpClient.GetStream();

                var request = _transferClient.Receive<LinesModel>(stream);
                var solveResult = GaussSolver.Solve(request);
                _transferClient.Send(stream, solveResult);
            }

            tcpListener.Stop();
        }
    }
}
