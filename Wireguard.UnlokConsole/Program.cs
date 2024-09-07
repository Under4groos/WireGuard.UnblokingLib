

using Newtonsoft.Json;
using WireGuard.UnblokingLib;
using WireGuard.UnblokingLib.Structures;

string file_json_data = "config.json";
Task WGTask;

StructUDPData structUDPData = new StructUDPData()
{
    Host = string.Empty,
    EndPort = 0,
    IsVisibilityConsole = true,
    ListenPort = 0,
};




using (WireGuardUDPCLient wireGuard = new WireGuardUDPCLient())
{


    while (true)
    {

        if (!File.Exists(file_json_data))
        {
            string json = JsonConvert.SerializeObject(structUDPData, Formatting.Indented);
            File.WriteAllText(file_json_data, json);
        }
        else
        {
            structUDPData = JsonConvert.DeserializeObject<StructUDPData>(File.ReadAllText(file_json_data));

            Console.WriteLine("Read config");
        }
        wireGuard.Init(structUDPData);

        wireGuard.Start();
        Thread.Sleep(1000);
    }

}