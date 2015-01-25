using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HFR4WinRT.Model;

namespace HFR4WinRT.Helpers
{
    public static class ConnectHelper
    {
        public static Task BeginAuthentication(this Account account)
        {
            Debug.WriteLine("Begin connection");
            var pseudo = account.Pseudo;
            var pseudoEncoded = WebUtility.UrlEncode(pseudo);
            var password = account.Password;
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(new Uri("http://forum.hardware.fr/"), new Cookie("name", "value"));
            
            var request = WebRequest.CreateHttp("http://forum.hardware.fr/login_validation.php?config=hfr.inc");
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.Headers["Set-Cookie"] = "name=value";
            request.CookieContainer = cookieContainer;
            request.BeginGetRequestStream(ar =>
            {
                var postStream = request.EndGetRequestStream(ar);
                var postData = "&pseudo=" + pseudoEncoded + "&password=" + password;

                var byteArray = Encoding.UTF8.GetBytes(postData);

                postStream.Write(byteArray, 0, postData.Length);
                postStream.Flush();
                postStream.Dispose();

                request.BeginGetResponse(result =>
                {
                    var response = (HttpWebResponse)request.EndGetResponse(result);
                    switch (cookieContainer.Count)
                    {
                        case 1:
                            break;
                        case 4:
                            account.CookieContainer = cookieContainer;
                            Debug.WriteLine("Connection succeed");
                            break;
                    }
                }, request);
            }, request);
        }
    }
}
