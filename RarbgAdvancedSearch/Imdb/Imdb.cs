using HtmlAgilityPack;
using Newtonsoft.Json;
using QuickType;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RarbgAdvancedSearch
{
    public class Imdb
    {
        public struct ImdbInfo
        {
            public string Name, Description, imdbId;
            public Image Image;
            public string imgUrl;
            public DateTimeOffset DatePublished;
            public string[] Keywords;
            public float RatingValue, WorstRating, BestRating;
            public long RatingCount;
            public Genre Genre;
        }

        private Dictionary<string, ImdbInfo> tmpImdbCache = new Dictionary<string, ImdbInfo>();

        Image DownloadImage(string fromUrl)
        {
            using (System.Net.WebClient webClient = new System.Net.WebClient())
            {
                using (Stream stream = webClient.OpenRead(fromUrl))
                {
                    return Image.FromStream(stream);
                }
            }
        }

        public void cacheImage(string imdbId, string imgUrl)
        {
            Task.Run(async () =>
            {
                Thread.Sleep(500);
                if (tmpImdbCache.ContainsKey(imdbId))
                {
                    try
                    {
                        var NewInfoBackup = tmpImdbCache[imdbId];
                        NewInfoBackup.Image = DownloadImage(imgUrl);
                        tmpImdbCache[imdbId] = NewInfoBackup;
                    }
                    catch (Exception) { }
                }
            });            
        }

        private ImdbInfo fetchImdbInfo(string imdbId)
        {
            try
            {
                byte[] response_bytes = { };
                string response = Utils.HttpClient.Get($"https://www.imdb.com/title/{imdbId}/", ref response_bytes, "", "", false, false);
                HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(response);
                var posterImageNode = doc.DocumentNode.SelectSingleNode("//*[@id='title-overview-widget']/div[1]/div[3]/div[1]/a/img") ?? doc.DocumentNode.SelectSingleNode("//*[@id='title-overview-widget']/div[2]/div[1]/a/img");
                var infoJsonNode = doc.DocumentNode.SelectNodes("//script[@type='application/ld+json']");
                if (infoJsonNode != null && infoJsonNode.Count > 0)
                {
                    try
                    {
                        var imdbJsonObj = JsonConvert.DeserializeObject<QuickType.ImdbJson>(infoJsonNode[0].InnerText, Converter.Settings);

                        var info = new ImdbInfo
                        {
                            imdbId = imdbId,
                            Name = imdbJsonObj.Name,
                            Description = imdbJsonObj.Description,
                            imgUrl = posterImageNode?.Attributes["src"]?.Value ?? imdbJsonObj.Image?.OriginalString,
                            Genre = imdbJsonObj.Genre ?? new Genre(),
                            DatePublished = imdbJsonObj.DatePublished ?? DateTimeOffset.Now,
                            Keywords = imdbJsonObj.Keywords?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[] { },
                            RatingValue = imdbJsonObj.AggregateRating?.RatingValue != null ? float.Parse(imdbJsonObj.AggregateRating?.RatingValue) : 0.0f,
                            WorstRating = imdbJsonObj.AggregateRating?.WorstRating != null ? float.Parse(imdbJsonObj.AggregateRating?.WorstRating) : 0.0f,
                            BestRating = imdbJsonObj.AggregateRating?.BestRating != null ? float.Parse(imdbJsonObj.AggregateRating?.BestRating) : 0.0f,
                            RatingCount = imdbJsonObj.AggregateRating?.RatingCount ?? 0
                        };

                        if(info.imgUrl == posterImageNode?.Attributes["src"]?.Value && !string.IsNullOrEmpty(imdbJsonObj.Image?.OriginalString))
                            cacheImage(imdbId, imdbJsonObj.Image?.OriginalString);

                        return info;
                    }
                    catch (Exception e1)
                    {
                        ;
                    }                    
                }
            }
            catch (Exception e2) {
                ;
            }           
            
            return new ImdbInfo();
        }

        public ImdbInfo GetImdbInfo(string imdbId)
        {
            Thread.Sleep(100);
            if (tmpImdbCache.ContainsKey(imdbId))
                return tmpImdbCache[imdbId];
            else
                tmpImdbCache.Add(imdbId, new ImdbInfo());

            var info = fetchImdbInfo(imdbId);

            try
            {
                if(!string.IsNullOrEmpty(info.Name))
                {
                    tmpImdbCache[imdbId] = info;
                }             
                else
                {
                    tmpImdbCache.Remove(imdbId);
                }
            } catch (Exception) { }

            return tmpImdbCache.ContainsKey(imdbId) ? tmpImdbCache[imdbId] : new ImdbInfo();
        }
    }
}
