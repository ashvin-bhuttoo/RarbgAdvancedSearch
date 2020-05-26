using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RarbgAdvancedSearch.Utils;

namespace RarbgAdvancedSearch
{
    public partial class browser : UserControl
    {
        public browser()
        {
            InitializeComponent();
        }

        public void navigateTo(string url)
        {
            webBrowser.Navigate(url);
        }

        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if(e.Url.AbsolutePath == "/torrents.php")
            {
                Reg.cookie = webBrowser.Document.Cookie;
                this.Parent.Controls.Remove(this);                
            }            
        }
    }
}
