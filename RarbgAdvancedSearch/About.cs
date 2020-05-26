using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;

namespace RarbgAdvancedSearch
{
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
            lblVersion.Text = $" v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start($"https://github.com/ashvin-bhuttoo/RarbgAdvancedSearch");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start($"https://www.paypal.me/ABhuttoo");
        }
    }
}
