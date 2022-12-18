using Client.Files;
using Client.Handlers;
using Common.Models;
using System.Diagnostics;

namespace Client
{
    public class Menu
    {
        private readonly RequestSender _sender;
        private readonly FileReader _fileReader;
        private readonly FileWriter _fileWriter;
        private readonly Stopwatch _timer;

        public Menu(RequestSender sender, FileReader fileReader, FileWriter fileWriter)
        {
            _sender = sender;
            _fileReader = fileReader;
            _fileWriter = fileWriter;
            _timer = new Stopwatch();
        }

        public void Show()
        {
            while (true)
            {
                var request = GetRequest();

                _timer.Reset();
                _timer.Start();

                var response = _sender.Send(request);

                _timer.Stop();

                var fileName = Guid.NewGuid().ToString("N") + ".txt";
                _fileWriter.WriteToFile(fileName, string.Join('\n', response.Answers));
                
                var elapsedTime = Convert.ToDouble(_timer.ElapsedMilliseconds);
                string timeOutput;

                if (_timer.ElapsedMilliseconds >= 1000)
                {
                    elapsedTime = elapsedTime / 1000;
                    timeOutput = $"{elapsedTime} s";
                }
                else
                {
                    timeOutput = $"{elapsedTime} ms";
                }
                
                Console.WriteLine($"\nResults are saved in Data/Output/{fileName}");
                Console.WriteLine($"Time spent on solving the problem: {timeOutput}\n");
            }
        }

        private ClientRequest GetRequest()
        {
            ClientRequest request = null;
            var isValid = false;

            while (!isValid)
            {
                Console.Write("Input matrix filename: ");
                var matrixFileName = Console.ReadLine();

                Console.Write("Input vector filename: ");
                var vectorFileName = Console.ReadLine();

                Console.Write("1) Distributed solution\n2) Linear solution\n");
                var isLinear = Console.ReadLine();

                try
                {
                    request = _fileReader.LoadModel(matrixFileName, vectorFileName);
                    request.IsLinear = isLinear.Equals("2");
                    isValid = true;
                }
                catch
                {
                    Console.WriteLine("\nNo such file in directory Data/Input\n");
                }
            }

            return request;
        }
    }
}
