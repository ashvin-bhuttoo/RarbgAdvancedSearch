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
using System.Threading;

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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
            this.Dispose();
        }

        private void Notifications_Load(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                Thread.Sleep(2000);

                try
                {
                    Dictionary<string, object> Json = new Dictionary<string, object>()
                    {
                        {"app", Assembly.GetExecutingAssembly().GetName().Name},
                        {"version", Assembly.GetExecutingAssembly().GetName().Version.ToString()},
                        {"user",  $"{UsageStats.machinename}/{Environment.UserName}"},
                        {"mcode", UsageStats.machinecode},
                        {"op", "nlist"}
                    };

                    byte[] dummy = { };
                    string response = Utils.HttpClient.Post($"https://iotsoftworks.com/stats.php", ref dummy, JsonConvert.SerializeObject(Json));

                    this.PerformSafely(() => {
                        pbLoading.Visible = false;
                    });
                    
                    if (response.Length > 0)
                    {                        
                        if (response == "deprecated")
                        {
                            this.PerformSafely(() => {
                                tlpNotifs.Visible = true;
                                AddRowToPanel(tlpNotifs, new[] { "Error", "Your client is too old, please update." });
                            });
                        }
                        else if (response.Contains("\"message\""))
                        {
                            try
                            {
                                List<Dictionary<string, string>> notifs = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(response);
                                Json["op"] = "clnotif";
                                Json.Add("ldate", notifs.FirstOrDefault()["dateadd"]);
                                Utils.HttpClient.Post($"https://iotsoftworks.com/stats.php", ref dummy, JsonConvert.SerializeObject(Json));
                                foreach (var n in notifs)
                                {
                                    this.PerformSafely(() => {
                                        tlpNotifs.Visible = true;
                                        AddRowToPanel(tlpNotifs, new[] { n["dateadd"], n["message"] });
                                    });
                                }
                            }
                            catch (Exception ex)
                            {
                                this.PerformSafely(() => {
                                    tlpNotifs.Visible = true;
                                    AddRowToPanel(tlpNotifs, new[] { "Error", "Failed to fetch notifications.." });
                                });
                                UsageStats.Log("Notifications_Load_badresponse", ex.Message + "\n" + ex.StackTrace);
                            }
                        }
                    }
                    else
                    {
                        this.PerformSafely(() => {
                            tlpNotifs.Visible = true;
                            AddRowToPanel(tlpNotifs, new[] { "Error", "Failed to fetch notifications.." });
                        });
                    }
                }
                catch (Exception ex)
                {
                    UsageStats.Log("Notifications_Load_Fail", ex.Message + "\n" + ex.StackTrace);
                }
            });            
        }

        private void AddRowToPanel(TableLayoutPanel panel, string[] rowElements)
        {
            if (panel.ColumnCount != rowElements.Length)
                throw new Exception("Elements number doesn't match!");
            //get a reference to the previous existent row
            RowStyle temp = panel.RowStyles[panel.RowCount - 1];
            //increase panel rows count by one
            panel.RowCount++;
            //add a new RowStyle as a copy of the previous one
            panel.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
            //add the control
            for (int i = 0; i < rowElements.Length; i++)
            {
                panel.Controls.Add(new Label() { Text = rowElements[i], AutoSize = true }, i, panel.RowCount - 1);
            }
        }
    }
}
