using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static RarbgAdvancedSearch.Utils;

namespace RarbgAdvancedSearch
{
    [Serializable]
    public enum RarbgCategory
    {
        XXX = 4,
        Movies_x264_1080p = 44,
        Movies_x265_4k = 51,
        Movies_x265_1080 = 54,
        Music_MP3 = 23,
        Games_PS3 = 40,
        Movies_XVID = 14,
        Movies_x264_720 = 45,
        Movs_x265_4k_HDR = 52,
        TV_Episodes = 18,
        Music_FLAC = 25,
        Games_XBOX_360 = 32,
        Movies_XVID_720 = 48,
        Movies_x264_3D = 47,
        Movies_Full_BD = 42,
        TV_HD_Episodes = 41,
        Games_PC_ISO = 27,
        Software_PC_ISO = 33,
        Movies_x264 = 17,
        Movies_x264_4k = 50,
        Movies_BD_Remux = 46,
        TV_UHD_Episodes = 49,
        Games_PC_RIP = 28,
        Games_PS4 = 53,
        Unknown = 0
    }

    public class RarbgPageParser
    {
        public List<rarbgEntry> listings;
        HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

        public RarbgPageParser()
        {
            listings = new List<rarbgEntry>();
        }        

        [Serializable]
        public struct rarbgEntry
        {
            public RarbgCategory category;
            public string name;
            public string url;
            public DateTime dateAdded;
            public double sizeInGb;
            public int seeders;
            public int leechers;
            public int comments;
            public string uploader;
            public List<string> genre;
            public int year;
            public double imdbRating;
            public string imdb_id;
        }

        public bool getLastPageNum(string html, ref int lastPage)
        {
            listings.Clear();
            doc.LoadHtml(html);

            var pager_link = doc.DocumentNode.SelectSingleNode("//div[@id='pager_links']/b");
            if(pager_link != null)
            {
                return int.TryParse(pager_link.InnerText, out lastPage);
            }

            return false;
        }

        public void parsePage(string html)
        {
            listings.Clear();
            doc.LoadHtml(html);

            var nodes = doc.DocumentNode.SelectNodes("//tr[@class='lista2']");
            if(nodes != null)
                foreach (HtmlNode html_listing in nodes)
                {
                    if (html_listing.HasChildNodes)
                        parserListing(html_listing.ChildNodes.Where(n => n.Name != "#text").ToList());
                }
        }

        private void parserListing(List<HtmlNode> listing_nodes)
        {
            if (listing_nodes.Count == 8)
            {
                rarbgEntry entry = new rarbgEntry() { genre = new List<string>() };

                //extract category
                if(listing_nodes[0].HasChildNodes && listing_nodes[0].FirstChild.Name == "a")
                {
                    RarbgCategory categ = RarbgCategory.Unknown;
                    Enum.TryParse(listing_nodes[0].FirstChild.Attributes["href"]?.Value?.Replace("/torrents.php?category=", ""), out categ);
                    entry.category = categ;
                }

                //extract name, year, url, imdb_id, genre, imdb rating
                if (listing_nodes[1].HasChildNodes && listing_nodes[1].FirstChild.Name == "a")
                {
                    var data_nodes = listing_nodes[1].ChildNodes.Where(n => n.Name != "#text" && n.Name != "br").ToList();
                    if (data_nodes.Count >= 1)
                    {
                        entry.name = data_nodes[0].Attributes["title"]?.Value ?? data_nodes[0].InnerText;

                        string[] name = entry.name.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach(var s in name.Reverse())
                        {
                            if (s.Length == 4 && int.TryParse(s, out entry.year))
                            {
                                if(entry.year > DateTime.Now.Year)
                                {
                                    entry.year = 0;
                                    continue;
                                }
                                break;
                            }                                
                        }

                        entry.url = data_nodes[0].Attributes["href"]?.Value ?? string.Empty;

                        if (data_nodes.Count > 1)
                        {
                            entry.imdb_id = data_nodes[1].Attributes["href"]?.Value?.Replace("/torrents.php?imdb=", "") ?? string.Empty;
                        }

                        if(data_nodes.Count > 2)
                        {
                            string[] span_data = data_nodes[2].InnerText.Split(new[] { "IMDB:", "imdb:" }, StringSplitOptions.RemoveEmptyEntries);
                            entry.genre = span_data.Length > 0 ? span_data[0].Trim().Split( new[] { ',', ' '}, StringSplitOptions.RemoveEmptyEntries ).ToList() : new List<string>();
                            if (span_data.Length > 1)
                            {
                                string imdb_info = span_data[1].Trim();
                                if (imdb_info.Contains("/"))
                                {
                                    double.TryParse(imdb_info.Substring(0, imdb_info.IndexOf("/")), out entry.imdbRating);
                                }
                            }
                        } 
                    }
                    else
                    {
                        throw new Exception($"Error: Listing does not have a nodes!");
                    }
                }

                //extract date added
                DateTime.TryParse(listing_nodes[2].InnerText.Trim(), out entry.dateAdded);

                //extract filesize
                string filesize_str = listing_nodes[3].InnerText.Trim();
                if(filesize_str.Contains("GB"))
                {
                    double.TryParse(filesize_str.Replace("GB", "").Trim(), out entry.sizeInGb);
                }
                else if(filesize_str.Contains("MB"))
                {
                    double.TryParse(filesize_str.Replace("MB", "").Trim(), out entry.sizeInGb);
                    entry.sizeInGb /= 1024;
                }
                else if(filesize_str.Contains("KB"))
                {
                    double.TryParse(filesize_str.Replace("KB", "").Trim(), out entry.sizeInGb);
                    entry.sizeInGb /= (1024*1024);
                }

                //Extract Seeders
                var seedersNode = listing_nodes[4].HasChildNodes ? listing_nodes[4].ChildNodes.FirstOrDefault(n => n.Name == "font") : null;
                if (seedersNode != null)
                {
                    int.TryParse(seedersNode.InnerText, out entry.seeders);
                }

                //Extract Leechers
                int.TryParse(listing_nodes[5].InnerText, out entry.leechers);

                //Extract Comment count
                int.TryParse(listing_nodes[6].InnerText, out entry.comments);

                //Extract Leechers
                entry.uploader = listing_nodes[7].InnerText;

                listings.Add(entry);                
            }
            else
                throw new Exception($"Error: Listing does not have 8 nodes, found {listing_nodes.Count} nodes!");
        }

    }

    class Utils
    {
        #region Serialization Utils
        private static BinaryFormatter _bin = new BinaryFormatter();

        public static void Serialize(string pathOrFileName, object objToSerialise)
        {
            using (Stream stream = File.Open(pathOrFileName, FileMode.Create))
            {
                try
                {
                    _bin.Serialize(stream, objToSerialise);
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                    throw;
                }
            }
        }

        public static T Deserialize<T>(string pathOrFileName)
        {
            T items;

            using (Stream stream = File.Open(pathOrFileName, FileMode.Open))
            {
                try
                {
                    items = (T)_bin.Deserialize(stream);
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                    throw;
                }
            }

            return items;
        }
        #endregion


        #region WEB CLIENT
        public static class HttpClient
        {
            private const int READTIMEOUT_CONST = 3000;

            public static string Post(string url, string content = "", int RXTIMEO = READTIMEOUT_CONST)
            {
                //Ignore ssl errors
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                // Initialize the WebRequest.
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = WebRequestMethods.Http.Post;
                webRequest.KeepAlive = true;
                webRequest.Headers["Accept-Encoding"] = "gzip, deflate";
                webRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                webRequest.Timeout = READTIMEOUT_CONST;
                if (!string.IsNullOrEmpty(content))
                {
                    using (Stream postStream = webRequest.GetRequestStream())
                    {
                        byte[] data = Encoding.UTF8.GetBytes(content);
                        postStream.Write(data, 0, data.Length);
                    }
                }

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

            public static string Get(string url, ref byte[] responseBytes, string referer = "", string host = "rarbgenter.org", bool addRarbgHeaders = true)
            {
                //Ignore ssl errors
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                // Initialize the WebRequest.
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = WebRequestMethods.Http.Get;
                webRequest.KeepAlive = true;
                bool hasreferer = !string.IsNullOrEmpty(referer);
                if (addRarbgHeaders)
                {
                    if (hasreferer)
                    {
                        webRequest.Host = host;
                        webRequest.KeepAlive = true;
                    }                       

                    webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                    webRequest.Headers["accept-encoding"] = "gzip, deflate";
                    webRequest.Headers["accept-language"] = "en-US,en;q=0.9";
                    webRequest.Headers["cache-control"] = "max-age=0";
                    
                    webRequest.Headers["cookie"] = Reg.cookie.Cast<Cookie>().Aggregate("", (current, next) => current + $"{next.Name}={next.Value}; ").TrimEnd(new[] { ';', ' ' });
                    if(hasreferer)
                        webRequest.Referer = referer;
                    webRequest.Headers["sec-fetch-dest"] = "document";
                    webRequest.Headers["sec-fetch-mode"] = "navigate";
                    webRequest.Headers["sec-fetch-site"] = hasreferer ? "same-origin" : "none";
                    webRequest.Headers["sec-fetch-user"] = "?1";
                    webRequest.Headers["upgrade-insecure-requests"] = "1";
                    webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.61 Safari/537.36";
                    webRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                }
                else
                {
                    webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                    webRequest.Headers["accept-encoding"] = "gzip, deflate";
                    webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36";
                }

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                using (Stream stream = GetStreamForResponse(webResponse, READTIMEOUT_CONST))
                {
                    if (stream.CanRead)
                    {
                        if (webResponse.ResponseUri.AbsoluteUri.Contains("threat_defence"))
                            return webResponse.ResponseUri.AbsoluteUri;

                        MemoryStream ms = new MemoryStream();
                        stream.CopyTo(ms);
                        responseBytes = ms.ToArray();

                        if (webResponse.Cookies.Count > 0 || webResponse.Headers["Set-Cookie"] != null)
                        {
                            List<Cookie> reg_cookies = Reg.cookie;
                            foreach (Cookie cookie in webResponse.Cookies)
                            {
                                if (!reg_cookies.Any(c => c.Name == cookie.Name))
                                {
                                    reg_cookies.Add(cookie);
                                }
                                else
                                {
                                    reg_cookies.FirstOrDefault(c => c.Name == cookie.Name).Value = cookie.Value;
                                }
                            }

                            if(webResponse.Headers["Set-Cookie"] != null)
                            {
                                CookieCollection newCookies = new CookieCollection();
                                if (HttpCookieExtension.GetHttpCookiesFromHeader(webResponse.Headers["Set-Cookie"], out newCookies))
                                {
                                    foreach (Cookie cookie in newCookies)
                                    {
                                        if (cookie.Name != "__cfduid")
                                            continue;

                                        if (!reg_cookies.Any(c => c.Name == cookie.Name))
                                        {
                                            reg_cookies.Add(cookie);
                                        }
                                        else
                                        {
                                            reg_cookies.FirstOrDefault(c => c.Name == cookie.Name).Value = cookie.Value;
                                        }
                                    }
                                }
                            }

                            Reg.cookie = reg_cookies;
                        }                            

                        return Encoding.ASCII.GetString(responseBytes);
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

        /// <summary>
        /// Registry Wrapper
        /// </summary>
        /// 
        #region Registry Wrapper
        public static class Reg
        {
            static Microsoft.Win32.RegistryKey rootKey;

            static Reg()
            {
                rootKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey($@"Software\{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}");
            }

            //expiry date
            public static List<Cookie> cookie
            {
                get
                {
                    var defaultValue = string.Empty;
                    var info = System.Reflection.MethodBase.GetCurrentMethod() as System.Reflection.MethodInfo;
                    var name = info.Name.Substring(4);
                    List<Cookie> cookies = new List<Cookie>();
                    try
                    {
                        cookies = JsonConvert.DeserializeObject<List<Cookie>>((string)rootKey.GetValue(name, defaultValue));
                    }
                    catch (Exception) {
                        Reg.cookie = cookies;
                    }
                    return (dynamic)Convert.ChangeType(cookies, info.ReturnType);
                }
                set
                {
                    var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Substring(4);
                    rootKey.SetValue(name, JsonConvert.SerializeObject(value));
                }
            }
        }
        #endregion

    }

    public static class UsageStats
    {
        static string machinename = GetInfo.GetMachineName();
        static string machinecode = GetInfo.GetMachineCode();

        public static void Log(string stat, string msg = "", bool blocking = false)
        {
            Dictionary<string, object> Json = new Dictionary<string, object>()
            {
                {"app", Assembly.GetExecutingAssembly().GetName().Name},
                {"version", Assembly.GetExecutingAssembly().GetName().Version.ToString()}
            };
            Json.Add("operation", stat);
            if (msg.Length > 0)
                Json.Add("msg", msg);
            Json.Add("user", $"{machinename}/{Environment.UserName}");
            Json.Add("mcode", machinecode);
           
            if(blocking)
            {
                SendLog(JsonConvert.SerializeObject(Json));
            }
            else
            {
                Task.Run(() =>
                {
                    SendLog(JsonConvert.SerializeObject(Json));
                });
            }            
        }

        /// <summary>
        /// A generic update checker for github projects
        /// The tag text is used as version field from github releases, the description text should contain an installer link pointed at raw.githubusercontent.com
        /// </summary>
        /// <returns></returns>
        private static void SendLog(string jsonmessage)
        {
            HttpClient.Post($"https://iotsoftworks.com/stats.php", jsonmessage);
        }
    }

    public static class HttpCookieExtension
    {
        static Regex rxCookieParts = new Regex(@"(?<name>.*?)\=(?<value>.*?)\;|(?<name>\bsecure\b|\bhttponly\b)", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        static Regex rxRemoveCommaFromDate = new Regex(@"\bexpires\b\=.*?(\;|$)", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline);

        public static bool GetHttpCookiesFromHeader(this string cookieHeader, out CookieCollection cookies)
        {
            cookies = new CookieCollection();


            try
            {

                string rawcookieString = rxRemoveCommaFromDate.Replace(cookieHeader, new MatchEvaluator(RemoveComma));

                string[] rawCookies = rawcookieString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                if (rawCookies.Length == 0)
                {
                    cookies.Add(rawcookieString.ToCookie());
                }
                else
                {
                    foreach (var rawCookie in rawCookies)
                    {
                        cookies.Add(rawCookie.ToCookie());
                    }
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static Cookie ToCookie(this string rawCookie)
        {

            if (!rawCookie.EndsWith(";")) rawCookie += ";";

            MatchCollection maches = rxCookieParts.Matches(rawCookie);

            Cookie cookie = new Cookie(maches[0].Groups["name"].Value.Trim(), maches[0].Groups["value"].Value.Trim());

            for (int i = 1; i < maches.Count; i++)
            {
                switch (maches[i].Groups["name"].Value.ToLower().Trim())
                {
                    case "domain":
                        cookie.Domain = maches[i].Groups["value"].Value;
                        break;
                    case "expires":

                        DateTime dt;

                        if (DateTime.TryParse(maches[i].Groups["value"].Value, out dt))
                        {
                            cookie.Expires = dt;
                        }
                        else
                        {
                            cookie.Expires = DateTime.Now.AddDays(2);
                        }
                        break;
                    case "path":
                        cookie.Path = maches[i].Groups["value"].Value;
                        break;
                    case "secure":
                        cookie.Secure = true;
                        break;
                    case "httponly":
                        cookie.HttpOnly = true;
                        break;
                }
            }
            return cookie;


        }

        private static KeyValuePair<string, string> SplitToPair(this string input)
        {
            string[] parts = input.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
            return new KeyValuePair<string, string>(parts[0], parts[1]);
        }

        private static string RemoveComma(Match match)
        {
            return match.Value.Replace(',', ' ');
        }
    }

    public static class GetInfo
    {
        /// <summary>
        /// return Volume Serial Number from hard drive
        /// </summary>
        /// <param name="strDriveLetter">[optional] Drive letter</param>
        /// <returns>[string] VolumeSerialNumber</returns>
        public static string GetVolumeSerial(string strDriveLetter)
        {
            if (strDriveLetter == "" || strDriveLetter == null) strDriveLetter = Path.GetPathRoot(Environment.SystemDirectory)[0].ToString();/*"C"*/;
            ManagementObject disk =
                new ManagementObject("win32_logicaldisk.deviceid=\"" + strDriveLetter + ":\"");
            disk.Get();
            return disk["VolumeSerialNumber"].ToString();
        }

        /// <summary>
        /// Returns MAC Address from first Network Card in Computer
        /// </summary>
        /// <returns>[string] MAC Address</returns>
        public static string GetMACAddress()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            string MACAddress = String.Empty;
            foreach (ManagementObject mo in moc)
            {
                if (MACAddress == String.Empty)  // only return MAC Address from first card
                {
                    if ((bool)mo["IPEnabled"] == true) MACAddress = mo["MacAddress"].ToString();
                }
                mo.Dispose();
            }
            MACAddress = MACAddress.Replace(":", "");
            return MACAddress;
        }
        /// <summary>
        /// Return processorId from first CPU in machine
        /// </summary>
        /// <returns>[string] ProcessorId</returns>
        public static string GetCPUId()
        {
            string cpuInfo = String.Empty;
            string temp = String.Empty;
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (cpuInfo == String.Empty)
                {// only return cpuInfo from first CPU
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
            }
            return cpuInfo;
        }

        private static string ComputeHash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        internal static string GetMachineCode()
        {
            return ComputeHash(GetVolumeSerial("") + /*GetMACAddress() +*/ GetCPUId());
        }

        internal static string GetMachineName()
        {
            return Environment.MachineName;
        }
    }

}
