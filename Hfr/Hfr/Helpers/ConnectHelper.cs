using Hfr.Model;
using Hfr.Utilities;
using Hfr.ViewModel;
using HtmlAgilityPack;
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
        public static async Task<bool> BeginAuthentication(this Account account, bool firstConnection)
        {
            Debug.WriteLine("Begin connection");
            var tcs = new TaskCompletionSource<bool>();
            var pseudo = account.Pseudo;
            var pseudoEncoded = WebUtility.UrlEncode(pseudo);
            var password = account.Password;
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(new Uri("http://forum.hardware.fr/"), new Cookie("name", "value"));

            var request = WebRequest.CreateHttp(HFRUrl.ConnectUrl);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.Headers["Set-Cookie"] = "name=value";
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
                        switch (cookieContainer.Count)
                        {
                            case 1:
                                tcs.SetResult(false);
                                break;
                            case 4:
                                account.CookieContainer = cookieContainer;
                                Debug.WriteLine("Connection succeed");
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
            return await tcs.Task;
        }

        static async Task GetAvatar(Account account)
        {
            var html = await HttpClientHelper.Get(HFRUrl.ProfilePageUrl, Loc.Main.AccountManager.CurrentAccount.CookieContainer);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            string[] userAvatarFileArray = htmlDoc.DocumentNode.Descendants("img").Where(x => x.GetAttributeValue("src", "").Contains("http://forum-images.hardware.fr/images/mesdiscussions-")).Select(y => y.GetAttributeValue("src", "")).ToArray();

            if (userAvatarFileArray.Length != 0)
            {
                await ThreadUI.Invoke(() => account.AvatarId = userAvatarFileArray[0].Split('/')[4].Replace(".jpg", "").Replace("mesdiscussions-", ""));
            }
        }
    }
}
