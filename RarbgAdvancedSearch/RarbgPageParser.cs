using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
            if (pager_link != null)
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
            if (nodes != null)
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
                if (listing_nodes[0].HasChildNodes && listing_nodes[0].FirstChild.Name == "a")
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
                        foreach (var s in name.Reverse())
                        {
                            if (s.Length == 4 && int.TryParse(s, out entry.year))
                            {
                                if (entry.year > DateTime.Now.Year)
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

                        if (data_nodes.Count > 2)
                        {
                            string[] span_data = data_nodes[2].InnerText.Split(new[] { "IMDB:", "imdb:" }, StringSplitOptions.RemoveEmptyEntries);
                            entry.genre = span_data.Length > 0 ? span_data[0].Trim().Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
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
                if (filesize_str.Contains("GB"))
                {
                    double.TryParse(filesize_str.Replace("GB", "").Trim(), out entry.sizeInGb);
                }
                else if (filesize_str.Contains("MB"))
                {
                    double.TryParse(filesize_str.Replace("MB", "").Trim(), out entry.sizeInGb);
                    entry.sizeInGb /= 1024;
                }
                else if (filesize_str.Contains("KB"))
                {
                    double.TryParse(filesize_str.Replace("KB", "").Trim(), out entry.sizeInGb);
                    entry.sizeInGb /= (1024 * 1024);
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

}
