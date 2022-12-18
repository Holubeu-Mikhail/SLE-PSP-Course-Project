using Common;
using Common.Models;
using System.Net;
using System.Net.Sockets;

namespace Client.Handlers
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

        public ClientResponse Send(ClientRequest request)
        {
            var ipPoint = new IPEndPoint(IPAddress.Parse(_address), _port);

            var tcpClient = new TcpClient();
            tcpClient.Connect(ipPoint);

            var stream = tcpClient.GetStream();

            _transferClient.Send(stream, request);
            var response = _transferClient.Receive<ClientResponse>(stream);

            tcpClient.Close();

            return response;
        }
    }
}
