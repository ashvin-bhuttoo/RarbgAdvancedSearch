using Newtonsoft.Json;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static RarbgAdvancedSearch.RarbgPageParser;

namespace RarbgAdvancedSearch
{
    public class ContentTracker
    {
        private string trackingFile = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\ContentTracker.db", tempTrackingFile = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\ContentTracker.tmp";
        public List<ContentTrack> tracks;

        public ContentTracker(){
            if(!File.Exists(trackingFile))
            {
                tracks = new List<ContentTrack>();
                saveToFile();
            }
            else
            {
                LoadFromFile();
            }
        }

        [Serializable]
        public enum Status
        {
            NotSet = 0,
            MarkedForDownload = 1,
            Downloading = 2,
            Downloaded = 3,
            Deleted = 4,            
        }

        [Serializable]
        public struct ContentTrack
        {
            public rarbgEntry entry;
            public Status stat;
            public string dd_url;
        }

        public bool savetrack(ContentTrack _ctr)
        {
            tracks.RemoveAll(t => t.entry.name == _ctr.entry.name);
            if (_ctr.stat != Status.NotSet || !string.IsNullOrEmpty(_ctr.dd_url))
                tracks.Add(_ctr);
            return saveToFile();
        }

        public bool savetrack(rarbgEntry _entry, Status _stat)
        {
            tracks.RemoveAll(t => t.entry.name == _entry.name);
            if(_stat != Status.NotSet)
                tracks.Add(new ContentTrack() { entry = _entry, stat = _stat });
            return saveToFile();
        }

        public bool contains(rarbgEntry _entry, ref Status stat)
        {
            if(tracks.Any(t => t.entry.name == _entry.name))
            {
                stat = tracks.FirstOrDefault(t => t.entry.name == _entry.name).stat;
                return true;
            }
            stat = Status.NotSet;
            return false;
        }

        private void LoadFromFile()
        {
            try
            {
                tracks = Utils.Deserialize<List<ContentTrack>>(trackingFile);
            }
            catch(Exception)
            {
                try
                {
                    tracks = Utils.Deserialize<List<ContentTrack>>(tempTrackingFile);
                }
                catch (Exception) { }
                tracks = new List<ContentTrack>();
            }
        }

        private bool saveToFile()
        {
            try
            {
                Utils.Serialize(tempTrackingFile, tracks);
                try
                {
                    //safe handling of tracker file to prevent corruption
                    Utils.Deserialize<List<ContentTrack>>(tempTrackingFile);
                    File.Copy(tempTrackingFile, trackingFile, true);
                    File.Delete(tempTrackingFile);
                }
                catch(Exception)
                {}               
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }

        public static List<ContentTracker.ContentTrack> GetDDList()
        {
            Dictionary<string, object> Json = new Dictionary<string, object>()
            {
                {"app", Assembly.GetExecutingAssembly().GetName().Name},
                {"version", Assembly.GetExecutingAssembly().GetName().Version.ToString()},
                {"user",  $"{UsageStats.machinename}/{Environment.UserName}"},
                {"mcode", UsageStats.machinecode},
                {"op", "ddlist"}
            };

            try
            {
                byte[] dummy = { };
                string response = Utils.HttpClient.Post($"https://iotsoftworks.com/stats.php", ref dummy, JsonConvert.SerializeObject(Json));

                if (response.Length > 0)
                {
                    if(response == "deprecated")
                    {
                        return new List<ContentTracker.ContentTrack>() { new ContentTrack { dd_url = "deprecated" } } ;
                    }

                    return JsonConvert.DeserializeObject<List<ContentTracker.ContentTrack>>(response);
                }
            }
            catch (Exception e)
            {
                UsageStats.Log("GetDDList_fail", e.Message + "\n" + e.StackTrace);
            }

            return new List<ContentTracker.ContentTrack>();
        }
    }
}
