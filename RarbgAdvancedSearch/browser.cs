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
using System.Threading;

namespace RarbgAdvancedSearch
{
    public partial class browser : UserControl
    {
        public bool userCancelled = false;

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
           
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            userCancelled = true;
            this.Parent.Controls.Remove(this);
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.AbsolutePath == "/torrents.php")
            {
                //for (int k = 0; k < 1000; k++)
                //{
                //    Thread.Sleep(1);
                //    Application.DoEvents();
                //}

                ////e.Url.

                //CookieCollection cookies = new CookieCollection();
                //if (HttpCookieExtension.GetHttpCookiesFromHeader(webBrowser.Document.Cookie, out cookies))
                //{
                //    List<Cookie> reg_cookies = Reg.cookie;
                //    foreach (Cookie cookie in cookies)
                //    {
                //        if (!reg_cookies.Any(c => c.Name == cookie.Name))
                //        {
                //            reg_cookies.Add(cookie);
                //        }
                //        else
                //        {
                //            reg_cookies.FirstOrDefault(c => c.Name == cookie.Name).Value = cookie.Value;
                //        }
                //    }
                //    Reg.cookie = reg_cookies;
                //}

                //this.Parent.Controls.Remove(this);
                this.webBrowser.Navigate("https://rarbgenter.org/torrent/4ix5319");                 
            }
            else if (e.Url.AbsolutePath == "/torrent/4ix5319")
            {
                for(int k=0;k<1000;k++)
                {
                    Thread.Sleep(1);
                    Application.DoEvents();
                }

                CookieCollection cookies = new CookieCollection();
                if (HttpCookieExtension.GetHttpCookiesFromHeader(webBrowser.Document.Cookie, out cookies))
                {
                    List<Cookie> reg_cookies = Reg.cookie;
                    foreach (Cookie cookie in cookies)
                    {
                        if (!reg_cookies.Any(c => c.Name == cookie.Name))
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
