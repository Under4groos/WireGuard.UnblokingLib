using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using WireGuard.UnblokingLib.Structures;

namespace WireGuard.UnblokingLib
{
    public class WireGuardUDPCLient : IDisposable
    {
        #region Disposable
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты)
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить метод завершения
                // TODO: установить значение NULL для больших полей
                disposedValue = true;
            }
        }

        // // TODO: переопределить метод завершения, только если "Dispose(bool disposing)" содержит код для освобождения неуправляемых ресурсов
        // ~WireGuardUDPCLient()
        // {
        //     // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion


        IPAddress? __IPAdres = null;
        System.Net.IPEndPoint? endPoint;
        private bool IsOK = false;
        public Action<string> LOGENVENT;
        private StructUDPData _structUDPData = new StructUDPData();




        public string SendData
        {
            get; set;
        } = "ad";
        public void Init(StructUDPData structUDPData)
        {
            try
            {
                if (IPAddress.TryParse(structUDPData.Host, out __IPAdres) && structUDPData.ListenPort > 0 && structUDPData.EndPort > 0)
                {
                    if (__IPAdres == null)
                        return;

                    endPoint = new System.Net.IPEndPoint(__IPAdres.Address, structUDPData.EndPort);

                    _structUDPData = structUDPData;
                    IsOK = true;
                }
            }
            catch (Exception e)
            {

                __log_err(e.Message);
                IsOK = false;

            }
        }
        private void __log_err(string str)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(str);
                Console.ForegroundColor = ConsoleColor.White;
            }



            LOGENVENT?.Invoke(str);
        }
        public void Start()
        {
            if (!IsOK)
            {
                __log_err("Config Error!");
                return;
            }

            try
            {
                using (System.Net.Sockets.UdpClient udp = new System.Net.Sockets.UdpClient(_structUDPData.ListenPort))
                {

                    byte[] buffer = Encoding.ASCII.GetBytes(SendData);

                    try
                    {
                        udp.Send(buffer, buffer.Length, endPoint);
                        Console.WriteLine($"Send: {buffer.Length}bytes");
                    }
                    catch (Exception e)
                    {
                        __log_err(e.Message);
                    }

                }
            }
            catch (Exception e)
            {
                __log_err(e.Message);

            }

        }
    }
}
