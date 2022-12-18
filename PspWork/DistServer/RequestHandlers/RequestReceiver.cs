using System.Net.Sockets;
using System.Net;
using System.Collections.Concurrent;

namespace DistServer.RequestHandlers
{
    public class RequestReceiver
    {
        private readonly string _address;
        private readonly int _port;
        private readonly IEnumerable<RequestSender> _senders;

        private readonly ConcurrentQueue<TcpClient> _clients;

        public RequestReceiver(string address, int port, IEnumerable<RequestSender> senders)
        {
            _address = address;
            _port = port;
            _senders = senders;
            _clients = new ConcurrentQueue<TcpClient>();
        }

        public void Start(object state)
        {
            ThreadPool.QueueUserWorkItem(HandleRequests);

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(_address), _port);
            var tcpListener = new TcpListener(ipPoint);

            tcpListener.Start();
            Console.WriteLine("Waiting for connections\n");

            try
            {
                while (true)
                {
                    var tcpClient = tcpListener.AcceptTcpClient();
                    _clients.Enqueue(tcpClient);

                    Console.WriteLine("Task received");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                tcpListener.Stop();
            }
        }

        private async void HandleRequests(object state)
        {
            while (true)
            {
                var isExistsClient = _clients.TryDequeue(out TcpClient tcpClient);

                if (isExistsClient)
                {
                    var distributer = new Distributer(tcpClient, _senders);
                    await distributer.Distribute();
                }
            }
        }
    }
}
