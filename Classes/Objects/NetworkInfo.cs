using System.Net;

namespace ServerManager.Classes.Objects
{
    public class NetworkInfo
    {
        private static string _publicAddress;    

        public static string PublicIpAddress()
        {
            using (WebClient submissionAddressClient = new WebClient())
            {
                submissionAddressClient.BaseAddress = "https://www.myexternalip.com/";
                _publicAddress = submissionAddressClient.DownloadString("raw").Trim();
            }
            return _publicAddress;
        }
    }
}