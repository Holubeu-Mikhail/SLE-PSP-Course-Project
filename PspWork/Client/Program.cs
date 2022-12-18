using Client;
using Client.Files;
using Client.Handlers;
using Client.Settings;
using Newtonsoft.Json; 

var settingsContent = File.ReadAllText("appsettings.json");
var settings = JsonConvert.DeserializeObject<ClientSettings>(settingsContent);

Console.WriteLine($"Requests will be sent to: {settings.Address}:{settings.Port}");

var sender = new RequestSender(settings.Address, settings.Port);
var fileReader = new FileReader(settings.InputPath);
var fileWriter = new FileWriter(settings.OutputPath);
var menu = new Menu(sender, fileReader, fileWriter);

menu.Show();
