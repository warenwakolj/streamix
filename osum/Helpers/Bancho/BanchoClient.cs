using System;
using System.Net;

namespace osum
{
    public class BanchoClient
    {
        private readonly WebClient webClient;
        private bool isConnected;

        public BanchoClient()
        {
            webClient = new WebClient();
            webClient.BaseAddress = "https://c.titanic.sh";
        }

        public bool Connect()
        {
            try
            {
                string response = webClient.DownloadString("/");
                isConnected = true;
                return true;
            }
            catch
            {
                isConnected = false;
                return false;
            }
        }

        public bool IsConnected => isConnected;

    }
}