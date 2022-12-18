using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

namespace Common
{
    public class TransferClient
    {
        public void Send<T>(NetworkStream stream, T model)
            where T : class
        {
            var streamWriter = new StreamWriter(stream, Encoding.ASCII);

            var stringData = JsonConvert.SerializeObject(model) + "&";

            streamWriter.Write(stringData);
            streamWriter.Flush();
        }

        public T Receive<T>(NetworkStream stream)
        {
            var stringBuilder = new StringBuilder(1024);
            var isLastSymbol = false;

            while (!isLastSymbol)
            {
                var buffer = new byte[1024];
                var data = stream.Read(buffer, 0, buffer.Length);
                var stringData = Encoding.ASCII.GetString(buffer, 0, data);

                if (stringData.EndsWith("&"))
                {
                    stringData = stringData.Replace("&", string.Empty);
                    isLastSymbol = true;
                }

                stringBuilder.Append(stringData);
            }

            return JsonConvert.DeserializeObject<T>(stringBuilder.ToString());
        }
    }
}
