using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RarbgAdvancedSearch
{
    class Utils
    {
        #region WEB CLIENT
        public static class HttpClient
        {
            private static int READTIMEOUT_CONST = 3000;


            public static string Get(string url)
            {
                // Initialize the WebRequest.
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = WebRequestMethods.Http.Get;
                webRequest.KeepAlive = true;
                webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                webRequest.Headers["accept-encoding"] = "gzip, deflate";
                webRequest.Headers["accept-language"] = "en-US,en;q=0.9";
                webRequest.Headers["cache-control"] = "max-age=0";
                webRequest.Headers["cookie"] = "__cfduid=ddff68ee1ac0b4d0ffed0ec316d624e401588698506; skt=5SDCR79mf0; gaDts48g=q8h5pp9t; skt=5SDCR79mf0; gaDts48g=q8h5pp9t; aby=2";
                webRequest.Headers["sec-fetch-dest"] = "document";
                webRequest.Headers["sec-fetch-mode"] = "navigate";
                webRequest.Headers["sec-fetch-site"] = "none";
                webRequest.Headers["sec-fetch-user"] = "?1";
                webRequest.Headers["upgrade-insecure-requests"] = "1";
                webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36";

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                using (Stream stream = GetStreamForResponse(webResponse, READTIMEOUT_CONST))
                {
                    if (stream.CanRead)
                    {
                        StreamReader reader = new StreamReader(stream);
                        return reader.ReadToEnd();
                    }
                }

                return string.Empty;
            }

            //Method
            private static Stream GetStreamForResponse(HttpWebResponse webResponse, int readTimeOut)
            {
                Stream stream;
                switch (webResponse.ContentEncoding.ToUpperInvariant())
                {
                    case "GZIP":
                        stream = new GZipStream(webResponse.GetResponseStream(), CompressionMode.Decompress);
                        break;
                    case "DEFLATE":
                        stream = new DeflateStream(webResponse.GetResponseStream(), CompressionMode.Decompress);
                        break;

                    default:
                        stream = webResponse.GetResponseStream();
                        if (stream.CanTimeout)
                            stream.ReadTimeout = readTimeOut;
                        break;
                }
                return stream;
            }
        }
        #endregion

    }
}
