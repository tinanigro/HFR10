using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

namespace Hfr.Helpers
{
    public static class HfrRehostHelper
    {
        public static async Task<string> Upload(StorageFile file)
        {
            if (file == null) return string.Empty;
            var tcs = new TaskCompletionSource<string>();

            var stream = await file.OpenStreamForReadAsync();
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

            var br = new BinaryReader(stream);
            var imageBytes = br.ReadBytes((int) stream.Length);

            var request = (HttpWebRequest)WebRequest.Create("http://reho.st/upload");
            request.ContentType = "multipart/form-data;boundary=" + boundary;

            request.Method = "POST";
            
            request.BeginGetRequestStream((ar =>
            {
                try
                {
                    var getRequest = (HttpWebRequest)ar.AsyncState;

                    // Arrêt de l'opération
                    var postStream = request.EndGetRequestStream(ar);

                    // Enregistrement de la requête vers le string
                    var payload = new StringBuilder();

                    payload.AppendFormat("--" + boundary + "{0}", Environment.NewLine);
                    payload.AppendFormat("Content-Disposition: form-data; name=\"fichier\"; filename=\"{1}\"{0}", Environment.NewLine, "fichier.jpg");
                    payload.AppendFormat("Content-Type: {0}{1}{1}", "image/jpeg", Environment.NewLine);
                    var postdata = Encoding.GetEncoding("iso-8859-1").GetString(imageBytes, 0, imageBytes.Length);
                    payload.AppendLine(postdata);
                    payload.AppendFormat("--" + boundary + "--");
                    var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(payload.ToString());

                    postStream.Write(bytes, 0, bytes.Length);
                    postStream.Dispose();
                    // Démarrage de l'opération asynchrone pour avoir la réponse
                    getRequest.BeginGetResponse((result =>
                    {
                        var getResponse = (HttpWebRequest)result.AsyncState;

                        // Arrêt de l'opération
                        try
                        {
                            var response = (HttpWebResponse) getResponse.EndGetResponse(result);
                            var responseStream = response.GetResponseStream();
                            var reader = new StreamReader(responseStream);
                            var codeImage = reader.ReadToEnd();
                            response.Dispose();

                            var codeImgNode = WebUtility.HtmlDecode(codeImage);
                            var firstUrl = codeImgNode.IndexOf("[url=http://reho.st/view/self/") +
                                           ("[url=http://reho.st/view/self/").Length;
                            var lastUrl = codeImgNode.IndexOf("][img]", firstUrl);
                            var urlImage = codeImgNode.Substring(firstUrl, lastUrl - firstUrl);
                            tcs.TrySetResult(urlImage);
                        }
                        catch
                        {
                            tcs.TrySetResult(string.Empty);
                        }
                    }), getRequest);
                }
                catch
                {
                    tcs.TrySetResult(string.Empty);
                }
            }), request);
            await tcs.Task;
            return tcs.Task.Result;
        }
    }
}
