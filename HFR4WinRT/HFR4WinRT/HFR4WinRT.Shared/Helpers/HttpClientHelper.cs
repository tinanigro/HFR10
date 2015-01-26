using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace HFR4WinRT.Helpers
{
    public static class HttpClientHelper
    {
        public static async Task<string> Get(string url, CookieContainer cookieContainer) 
        { 
            string result = "";
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    HttpClientHandler handler = new HttpClientHandler();
                    handler.UseDefaultCredentials = true;
                    handler.AllowAutoRedirect = true;
                    handler.UseCookies = true;
                    handler.CookieContainer = cookieContainer;
                    HttpClient client = new HttpClient(handler);
                    result = await client.GetStringAsync(new Uri(url));
                }
                catch(Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                }
            }
            return result;
        }
    }
}
