using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutoFire
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            
            InitializeComponent();
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url;
            if (e.Link.LinkData != null)
                url = e.Link.LinkData.ToString();
            else
                url = linkLabel1.Text.Substring(e.Link.Start, e.Link.Length);

            if (!url.Contains("://"))
                url = "http://" + url;

            var si = new ProcessStartInfo(url);
            Process.Start(si);
            linkLabel1.LinkVisited = true;
        }
    }
}
