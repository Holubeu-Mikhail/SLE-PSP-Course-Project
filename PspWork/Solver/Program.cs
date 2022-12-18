using Common.Settings;
using Newtonsoft.Json;
using Solver;

var settingsContent = File.ReadAllText("appsettings.json");
var settings = JsonConvert.DeserializeObject<SingleSettings>(settingsContent);

Console.WriteLine($"Solver ip address: {settings.Address}:{settings.Port}");

var handler = new Handler(settings.Address, settings.Port);
handler.Start();
