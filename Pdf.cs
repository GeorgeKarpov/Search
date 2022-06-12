using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Search
{
    public partial class Pdf : Form
    {
        private string src;
        public Pdf()
        {
            InitializeComponent();
        }

        public string Src { get => this.src; set => this.src = value; }

        private void Pdf_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.PDF;
            webBrowser1.Navigate(src);
        }
    }
}
