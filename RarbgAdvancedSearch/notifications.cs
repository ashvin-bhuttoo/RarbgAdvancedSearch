using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Newtonsoft.Json;

namespace RarbgAdvancedSearch
{
    public partial class Notifications : UserControl
    {
        public Notifications()
        {
            InitializeComponent();
        }


        public static int GetNotifCount()
        {
            Dictionary<string, object> Json = new Dictionary<string, object>()
            {
                {"app", Assembly.GetExecutingAssembly().GetName().Name},
                {"version", Assembly.GetExecutingAssembly().GetName().Version.ToString()},
                {"user",  $"{UsageStats.machinename}/{Environment.UserName}"},
                {"mcode", UsageStats.machinecode},
                {"op", "ncount"}
            };

            try
            {
                byte[] dummy = { };
                string response = Utils.HttpClient.Post($"https://iotsoftworks.com/stats.php", ref dummy, JsonConvert.SerializeObject(Json));

                if (response.Length > 0)
                {
                    if (response == "deprecated")
                    {
                        return -1;
                    }
                    else if (response.Contains("\"ncount\""))
                    {
                        try
                        {
                            Dictionary<string, int> notif_count = JsonConvert.DeserializeObject<Dictionary<string, int>>(response);
                            return notif_count["ncount"];                            
                        }
                        catch (Exception ex)
                        {
                            UsageStats.Log("GetNotifCount_badresponse", ex.Message + "\n" + ex.StackTrace);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                UsageStats.Log("GetNotifCount_Fail", e.Message + "\n" + e.StackTrace);
            }

            return 0;
        }
    }
}
