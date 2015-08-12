using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Hfr.Helpers
{

    public static class HttpClientHelper
    {
        //Utils
        static void cleanCookies()
        {
            //Remove cookies before anything
            Windows.Web.Http.Filters.HttpBaseProtocolFilter myFilter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
            var cookieManager = myFilter.CookieManager;
            Windows.Web.Http.HttpCookieCollection myCookieJar = cookieManager.GetCookies(new Uri("http://forum.hardware.fr"));
            foreach (Windows.Web.Http.HttpCookie cookie in myCookieJar)
            {
                cookieManager.DeleteCookie(cookie);
            }
            //--
        }

        public static async Task<string> Get(string url, string cookieContainer)
        {
            string result = "";

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    cleanCookies();

                    var baseAddress = new Uri("http://forum.hardware.fr");
                    var cookieContainr = new CookieContainer();
                    var handler = new HttpClientHandler();
                    handler.CookieContainer = cookieContainr;
                    handler.UseCookies = true;
                    
                    using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
                    {
                        List<Cookie> entities = (List<Cookie>)JsonConvert.DeserializeObject(cookieContainer, typeof(List<Cookie>));
                        var c = new CookieCollection();

                        foreach (var entity in entities)
                        {
                            cookieContainr.Add(baseAddress, new Cookie(entity.Name, entity.Value));
                        }

                        var resultObj = client.GetAsync(url).Result;
                        resultObj.EnsureSuccessStatusCode();
                        result = await resultObj.Content.ReadAsStringAsync();
                        cleanCookies();

                        //Debug.WriteLine("result =" + result);
                    }

                }
                catch (Exception exception)
                {
                    Debug.WriteLine("HttpClientHelper.Get Exception : " + exception.ToString());
                }
            }
            return result;
        }
    }
}
