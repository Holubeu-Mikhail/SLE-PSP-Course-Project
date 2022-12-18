using Common;
using Common.Models;
using System.Net;
using System.Net.Sockets;

namespace DistServer.RequestHandlers
{
    public class RequestSender
    {
        private readonly string _address;
        private readonly int _port;
        private readonly TransferClient _transferClient;

        public RequestSender(string address, int port)
        {
            _address = address;
            _port = port;
            _transferClient = new TransferClient();
        }

        public LinesModel Send(LinesModel model)
        {
            var ipPoint = new IPEndPoint(IPAddress.Parse(_address), _port);

            var tcpClient = new TcpClient();
            tcpClient.Connect(ipPoint);

            var stream = tcpClient.GetStream();

            _transferClient.Send(stream, model);
            var response = _transferClient.Receive<LinesModel>(stream);

            stream.Close();
            tcpClient.Close();

            return response;
        }
    }
}
