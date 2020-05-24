using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RarbgAdvancedSearch
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(btnSearch.Text == "STOP")
            {
                btnSearch.Text = "SEARCH";
                btnSearch.ForeColor = Color.DarkGreen;
                return;
            }

            btnSearch.Text = "STOP";
            btnSearch.ForeColor = Color.DarkRed;

            if (true || categ1.CheckedItems.Count > 0 || categ2.CheckedItems.Count > 0 || categ3.CheckedItems.Count > 0 || categ4.CheckedItems.Count > 0)
            {
                string category = string.Empty;

                foreach(int index in categ1.SelectedIndices)
                {
                    category += "&category[]=" + (new[] { "4", "44", "51", "54", "23", "40" }[index]);
                }
                foreach (int index in categ2.SelectedIndices)
                {
                    category += "&category[]=" + (new[] { "14", "45", "52", "18", "25", "32" }[index]);
                }
                foreach (int index in categ3.SelectedIndices)
                {
                    category += "&category[]=" + (new[] { "48", "47", "42", "41", "27", "33" }[index]);
                }
                foreach (int index in categ3.SelectedIndices)
                {
                    category += "&category[]=" + (new[] { "17", "50", "46", "49", "28", "53" }[index]);
                }


                //string response = Utils.HttpClient.Get($"https://rarbgenter.org/torrents.php?search={category}");

                string response = System.IO.File.ReadAllText(@"html_response.txt");

                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(response);



                Console.WriteLine(response);

            }

            else
            {
                btnSearch.Text = "SEARCH";
                btnSearch.ForeColor = Color.DarkGreen;
                showMessage("Error", "Please select a category first.", MessageBoxIcon.Error);
            }         
        }

        private void showMessage(string caption, string text, MessageBoxIcon icon = MessageBoxIcon.Information, MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            MessageBox.Show(text, caption, buttons, icon);
        }
    }
}
