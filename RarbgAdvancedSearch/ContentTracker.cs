using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RarbgAdvancedSearch.RarbgPageParser;

namespace RarbgAdvancedSearch
{
    class ContentTracker
    {
        private const string trackingFile = "ContentTracker.db";
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
                tracks = new List<ContentTrack>();
            }
        }

        private bool saveToFile()
        {
            try
            {
                Utils.Serialize(trackingFile, tracks);
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }
    }
}
