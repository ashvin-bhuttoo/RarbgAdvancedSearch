using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using static RarbgAdvancedSearch.RarbgPageParser;

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
            public string genre;
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

            foreach(HtmlNode html_listing in doc.DocumentNode.SelectNodes("//tr[@class='lista2']"))
            {
                if (html_listing.HasChildNodes)
                    parserListing(html_listing.ChildNodes.Where(n => n.Name != "#text").ToList());
            }
        }

        private void parserListing(List<HtmlNode> listing_nodes)
        {
            if (listing_nodes.Count == 8)
            {
                rarbgEntry entry = new rarbgEntry();

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
                        foreach(var s in name)
                        {
                            if (s.Length == 4 && int.TryParse(s, out entry.year))
                                break;
                        }

                        entry.url = data_nodes[0].Attributes["href"]?.Value ?? string.Empty;

                        if (data_nodes.Count > 1)
                        {
                            entry.imdb_id = data_nodes[1].Attributes["href"]?.Value?.Replace("/torrents.php?imdb=", "") ?? string.Empty;
                        }

                        if(data_nodes.Count > 2)
                        {
                            string[] span_data = data_nodes[2].InnerText.Split(new[] { "IMDB:", "imdb:" }, StringSplitOptions.RemoveEmptyEntries);
                            entry.genre = span_data.Length > 0 ? span_data[0].Trim() : string.Empty;
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
                        throw new Exception($"Error: Listing does not have 3 nodes, found {listing_nodes[1].ChildNodes} nodes!");
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
