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
using System.Net;

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
                CookieCollection cookies = new CookieCollection();
                if(HttpCookieExtension.GetHttpCookiesFromHeader(webBrowser.Document.Cookie, out cookies))
                {
                    List<Cookie> reg_cookies = Reg.cookie;
                    foreach(Cookie cookie in cookies)
                    {
                        if(!reg_cookies.Any(c => c.Name == cookie.Name))
                        {
                            reg_cookies.Add(cookie);
                        }
                        else
                        {
                            reg_cookies.FirstOrDefault(c => c.Name == cookie.Name).Value = cookie.Value;
                        }
                    }
                    Reg.cookie = reg_cookies;
                }

                this.Parent.Controls.Remove(this);                
            }            
        }
    }
}
