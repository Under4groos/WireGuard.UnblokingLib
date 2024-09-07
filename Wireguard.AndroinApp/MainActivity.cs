using Newtonsoft.Json;
using WireGuard.UnblokingLib;
using WireGuard.UnblokingLib.Structures;
using Formatting = Newtonsoft.Json.Formatting;

namespace Wireguard.AndroinApp
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        TextView debug_, host, ListenPort, EndPORT;
        WireGuardUDPCLient wireGuard = new WireGuardUDPCLient();

        StructUDPData structUDPData = new StructUDPData()
        {
            Host = string.Empty,
            EndPort = 0,
            IsVisibilityConsole = true,
            ListenPort = 0,
        };
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            host = FindViewById<TextView>(Resource.Id.ipcfg);
            host.TextChanged += (o, e) =>
            {
                structUDPData.Host = host.Text;
            };


            ListenPort = FindViewById<TextView>(Resource.Id.wgListenPort);
            ListenPort.TextChanged += (o, e) =>
            {
                if (int.TryParse(ListenPort.Text, out int port))
                {
                    structUDPData.ListenPort = port;
                }

            };

            EndPORT = FindViewById<TextView>(Resource.Id.wgPORT);
            EndPORT.TextChanged += (o, e) =>
            {
                if (int.TryParse(EndPORT.Text, out int port))
                {
                    structUDPData.EndPort = port;
                }

            };
            Button b_send = FindViewById<Button>(Resource.Id.b_send);
            debug_ = FindViewById<TextView>(Resource.Id.debugstatus);

            b_send.Click += B_send_Click;
            wireGuard.LOGENVENT += (string err) =>
            {
                debug_.Text += err;
            };
        }
        string filePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "config.json");
        private void B_send_Click(object? sender, EventArgs e)
        {
            Directory.CreateDirectory(new FileInfo(filePath).DirectoryName);

            if (!File.Exists(filePath))
            {
                string json = JsonConvert.SerializeObject(structUDPData, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            else
            {

                structUDPData = JsonConvert.DeserializeObject<StructUDPData>(File.ReadAllText(filePath));
                debug_.Text += "Read config";
            }






            wireGuard.Init(structUDPData);

            wireGuard.Start();


        }
    }
}