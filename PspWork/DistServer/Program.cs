using DistServer.RequestHandlers;
using DistServer.Settings;
using Newtonsoft.Json;

var settingsContent = File.ReadAllText("appsettings.json");
var settings = JsonConvert.DeserializeObject<DistSettings>(settingsContent);

Console.WriteLine($"Dist server ip address: {settings.ServerSettings.Address}:{settings.ServerSettings.Port}");

var requestSenders = new List<RequestSender>();

foreach (var setting in settings.SolversSettings)
{
    requestSenders.Add(new RequestSender(setting.Address, setting.Port));
}

var requestReceiver = new RequestReceiver(settings.ServerSettings.Address, settings.ServerSettings.Port, requestSenders);

ThreadPool.QueueUserWorkItem(requestReceiver.Start);
Console.ReadLine();
