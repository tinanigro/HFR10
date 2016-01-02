using Hfr.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Hfr.Utilities;

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

        public static async Task<string> Get(string url)
        {
            if (Loc.Main.AccountManager.CurrentAccount != null)
            {
                return await Get(url, Loc.Main.AccountManager.CurrentAccount.CookieContainer);
            }
            else
            {
                return null;
            }
        }

        public static async Task<string> Get(string url, string cookieContainer)
        {
            string result = "";

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    cleanCookies();
                    Debug.WriteLine("Get Helper url = " + url);

                    var baseAddress = new Uri(HFRUrl.ForumUrl);
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

                        Debug.WriteLine("Get Helper url complete = " + url);
                    }

                }
                catch (Exception exception)
                {
                    Debug.WriteLine("HttpClientHelper.Get Exception : " + exception.ToString());
                }
            }
            return result;
        }

        public static async Task<string> Post(string url, Dictionary<String, String> formData)
        {
            if (Loc.Main.AccountManager.CurrentAccount != null)
            {
                return await Post(url, formData, Loc.Main.AccountManager.CurrentAccount.CookieContainer);
            }
            else
            {
                return null;
            }
        }

        public static async Task<string> Post(string url, Dictionary<String, String> formData, string cookieContainer)
        {
            string result = "";

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                try
                {
                    cleanCookies();
                    Debug.WriteLine("Post Helper url = " + url);

                    var baseAddress = new Uri(HFRUrl.ForumUrl);
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

                        var content = new FormUrlEncodedContent(formData);

                        var resultObj = client.PostAsync(url, content).Result;

#warning "response status code not used"
                        if (resultObj.StatusCode == HttpStatusCode.NotFound)
                            result = "";
                        else
                            result = await resultObj.Content.ReadAsStringAsync();

                        cleanCookies();
                        //Debug.WriteLine("Post Helper result = " + result);
                    }

                }
                catch (Exception exception)
                {
                    Debug.WriteLine("HttpClientHelper.Post Exception : " + exception.ToString());
                }
            }
            return result;
        }
    }
}
