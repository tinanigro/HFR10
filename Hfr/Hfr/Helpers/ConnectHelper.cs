using Hfr.Model;
using Hfr.Utilities;
using Hfr.ViewModel;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hfr.Helpers
{
    public static class ConnectHelper
    {
        public static Task<bool> BeginAuthentication(this Account account, bool firstConnection)
        {
            Debug.WriteLine("Begin connection");
            var tcs = new TaskCompletionSource<bool>();

            //Remove cookies before anything
            Windows.Web.Http.Filters.HttpBaseProtocolFilter myFilter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
            var cookieManager = myFilter.CookieManager;
            Windows.Web.Http.HttpCookieCollection myCookieJar = cookieManager.GetCookies(new Uri("http://forum.hardware.fr"));
            foreach (Windows.Web.Http.HttpCookie cookie in myCookieJar)
            {
                cookieManager.DeleteCookie(cookie);
            }
            //--

            var pseudo = account.Pseudo;
            var pseudoEncoded = WebUtility.UrlEncode(pseudo);
            var password = account.Password;
            var cookieContainer = new CookieContainer();

#warning "this must be rewritten using HttpClientHelper"

            var request = WebRequest.CreateHttp(HFRUrl.ForumUrl + HFRUrl.ConnectUrl);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.CookieContainer = cookieContainer;
            request.BeginGetRequestStream(ar =>
            {
                try
                {
                    var postStream = request.EndGetRequestStream(ar);
                    var postData = "&pseudo=" + pseudoEncoded + "&password=" + password;

                    var byteArray = Encoding.UTF8.GetBytes(postData);

                    postStream.Write(byteArray, 0, postData.Length);
                    postStream.Flush();
                    postStream.Dispose();

                    request.BeginGetResponse(async result =>
                    {
                        var response = (HttpWebResponse)request.EndGetResponse(result);
                        Debug.WriteLine("cookieCount :"+ cookieContainer.Count);

                        switch (cookieContainer.Count)
                        {
                            case 0:
                                #warning "no UI feedback to warn user that something went wrong"
                                tcs.SetResult(false);
                                break;

                            case 3:
                                Debug.WriteLine("Connection succeed");
                                account.CookieContainer = JsonConvert.SerializeObject(cookieContainer.GetCookies(new Uri("http://forum.hardware.fr")).OfType<Cookie>().ToList());
                                
                                if (firstConnection)
                                    await GetAvatar(account);
                                tcs.SetResult(true);
                                break;
                        }
                    }, request);
                }
                catch (WebException exception)
                {
                    Debug.WriteLine("Failed to connect. WebException error");
                    tcs.SetResult(false);
                }
            }, request);
            return tcs.Task;
        }

        public static async Task GetAvatar(Account account)
        {
            var html = await HttpClientHelper.Get(HFRUrl.ProfilePageUrl, Loc.Main.AccountManager.CurrentAccount.CookieContainer);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var avatarSrc = htmlDoc.DocumentNode.Descendants("img").FirstOrDefault(x => x.GetAttributeValue("src", "").Contains("http://forum-images.hardware.fr/images/mesdiscussions-")).GetAttributeValue("src", "");

            if (!string.IsNullOrEmpty(avatarSrc))
            {
                await ThreadUI.Invoke(() => account.AvatarId = avatarSrc.Split('/')[4].Replace("mesdiscussions-", ""));
            }
        }
    }
}
