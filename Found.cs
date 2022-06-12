using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#pragma warning disable IDE1006

namespace Search
{
    public enum MessageType
    {
        @Ipu,
        Loop,
        Ccu,
        Oc,
    }
    public partial class Found : Form
    {
        private BindingSource bind_found;
        public Data Data;
        private NotifyHelper notifyHelper;
        const int defX = 441;
        const int defTc3Y = 154;
        const int defTc4Y = 216;
        const int defS2Y = 360;
        const int sildz5S2Y = 417;
        const int pompsTc3Y = 188;
        const int pompsTc4Y = 369;

        private Dictionary<string, Color> lblsColors = new Dictionary<string, Color>();

        private MessageType message;
        public Found()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icon;
            bind_found = new BindingSource();
            notifyHelper = new NotifyHelper
            {
                PosYlblTC3 = new Point(defX, defTc3Y),
                PosYlblTC4 = new Point(defX, defTc4Y),
                PosYlblS2 = new Point(defS2Y, defTc4Y),
            };
            this.lblIpu.MouseLeave += new EventHandler(this.lbl_MouseLeave);
            this.lblIpu.MouseHover += new EventHandler(this.lbl_MouseHover);
            this.lblLoop.MouseLeave += new EventHandler(this.lbl_MouseLeave);
            this.lblLoop.MouseHover += new EventHandler(this.lbl_MouseHover);
            this.lblCcu.MouseLeave += new EventHandler(this.lbl_MouseLeave);
            this.lblCcu.MouseHover += new EventHandler(this.lbl_MouseHover);
            this.lblA1A2.MouseLeave += new EventHandler(this.lbl_MouseLeave);
            this.lblA1A2.MouseHover += new EventHandler(this.lbl_MouseHover);

            lblsColors.Add("lblIpu", Color.Blue);
            lblsColors.Add("lblLoop", Color.Green);
            lblsColors.Add("lblCcu", Color.Maroon);
            lblsColors.Add("lblA1A2", Color.Teal);

            lblIpu.Click += lblMsg_Click;
            lblLoop.Click += lblMsg_Click;
            lblCcu.Click += lblMsg_Click;
            lblA1A2.Click += lblMsg_Click;
        }


        private void Bind_found_CurrentItemChanged(object sender, EventArgs e)
        {
            DataRowView current = (DataRowView)((BindingSource)sender).Current;
            pictureBox1.ImageLocation = 
                AppDomain.CurrentDomain.BaseDirectory + "img/" 
                + current["PROM"] + "_" + current["IND"] + ".jpg";
            if (current["PROM"].ToString() == "POMPS")
            {
                notifyHelper.PosYlblTC3 = new Point(defX, pompsTc3Y);
                notifyHelper.PosYlblTC4 = new Point(defX, pompsTc4Y);
            }
            else
            {
                notifyHelper.PosYlblTC3 = new Point(defX, defTc3Y);
                notifyHelper.PosYlblTC4 = new Point(defX, defTc4Y);
            }

            if (current["PROM"].ToString() == "SILDZ-5")
            {
                notifyHelper.PosYlblS2 = new Point(defX, sildz5S2Y);
            }
            else
            {
                notifyHelper.PosYlblS2 = new Point(defX, defS2Y);
            }
        }

        public BindingSource Bind_found { get => this.bind_found; set => this.bind_found = value; }

        private void Found_Load(object sender, EventArgs e)
        {
            bind_found.MoveFirst();
            bind_found.CurrentItemChanged += Bind_found_CurrentItemChanged;
            lblIpu.DataBindings.Add("Text", bind_found, "IPU");
            lblLoop.DataBindings.Add("Text", bind_found, "LOOP");
            lblPort.DataBindings.Add("Text", bind_found, "PORT");
            lblCcu.DataBindings.Add("Text", bind_found, "CCU");
            lblSer.DataBindings.Add("Text", bind_found, "SER");
            lblObc.DataBindings.Add("Text", bind_found, "OBC");
            lblH.DataBindings.Add("Text", bind_found, "H");
            lblPos.DataBindings.Add("Text", bind_found, "POS");
            lblInd.DataBindings.Add("Text", bind_found, "IND");
            lblProm.DataBindings.Add("Text", bind_found, "PROM");
            lblA1A2.DataBindings.Add("Text", bind_found, "A1A2");
            lblPlace.DataBindings.Add("Text", bind_found, "MESTO");
            lblIpuD.DataBindings.Add("Text", bind_found, "IPUd");
            lblLoopD.DataBindings.Add("Text", bind_found, "LOOP");
            lblPortD.DataBindings.Add("Text", bind_found, "PORTd");
            lblTC1.DataBindings.Add("Text", bind_found, "TC1");
            lblTC2.DataBindings.Add("Text", bind_found, "TC2");
            lblP1.DataBindings.Add("Text", bind_found, "P1");
            lblP2.DataBindings.Add("Text", bind_found, "P2");
            lblTC3.DataBindings.Add("Text", bind_found, "TC3");
            lblTC4.DataBindings.Add("Text", bind_found, "TC4");
            lblS1.DataBindings.Add("Text", bind_found, "S1");
            lblS2.DataBindings.Add("Text", bind_found, "S2");
            lblS3.DataBindings.Add("Text", bind_found, "S3");
            lblS4.DataBindings.Add("Text", bind_found, "S4");
            lblR1.DataBindings.Add("Text", bind_found, "R1");
            lblR2.DataBindings.Add("Text", bind_found, "R2");
            lblR3.DataBindings.Add("Text", bind_found, "R3");
            lblR4.DataBindings.Add("Text", bind_found, "R4");
            lblR5.DataBindings.Add("Text", bind_found, "R5");
            lblR6.DataBindings.Add("Text", bind_found, "R6");
            lblR7.DataBindings.Add("Text", bind_found, "R7");
            lblR8.DataBindings.Add("Text", bind_found, "R8");
            lblR9.DataBindings.Add("Text", bind_found, "R9");
            lblR10.DataBindings.Add("Text", bind_found, "R10");
            lblR11.DataBindings.Add("Text", bind_found, "R11");
            lblR12.DataBindings.Add("Text", bind_found, "R12");


            pictureBox1.DataBindings.Add("Visible", Data.NotifyHelper, "Loop");
            label4.DataBindings.Add("Visible", Data.NotifyHelper, "Loop");
            label5.DataBindings.Add("Visible", Data.NotifyHelper, "Loop");
            label6.DataBindings.Add("Visible", Data.NotifyHelper, "Loop");
            label7.DataBindings.Add("Visible", Data.NotifyHelper, "Loop");
            label8.DataBindings.Add("Visible", Data.NotifyHelper, "Loop");
            label9.DataBindings.Add("Visible", Data.NotifyHelper, "Loop");
            label10.DataBindings.Add("Visible", Data.NotifyHelper, "Loop");
            label11.DataBindings.Add("Visible", Data.NotifyHelper, "Loop");
            label12.DataBindings.Add("Visible", Data.NotifyHelper, "Loop");
            lblArCcu.DataBindings.Add("Visible", Data.NotifyHelper, "Loop");
            lblArA1A2.DataBindings.Add("Visible", Data.NotifyHelper, "Loop");

            Regex p = new Regex(@"lblP\d{1,2}");
            Regex s = new Regex(@"lblS\d{1,2}");
            Regex tc = new Regex(@"lblTC\d{1,2}");
            Regex r = new Regex(@"lblR\d{1,2}");

            foreach (var lbl in this.Controls.OfType<Label>())
            {
                if (tc.IsMatch(lbl.Name))
                {
                    lbl.DataBindings.Add("Visible", Data.NotifyHelper, "Loop");
                }

                if (p.IsMatch(lbl.Name))
                {
                    lbl.DataBindings.Add("Visible", Data.NotifyHelper, "Pomps");
                }

                if (s.IsMatch(lbl.Name))
                {
                    lbl.DataBindings.Add("Visible", Data.NotifyHelper, "Sildz");
                }

                if (r.IsMatch(lbl.Name))
                {
                    lbl.DataBindings.Add("Visible", Data.NotifyHelper, "Viomps");
                }
            }
 
            label15.DataBindings.Add("Visible", Data.NotifyHelper, "IpuLoop");
            label16.DataBindings.Add("Visible", Data.NotifyHelper, "IpuLoop");
            label17.DataBindings.Add("Visible", Data.NotifyHelper, "IpuLoop");
            lblIpuD.DataBindings.Add("Visible", Data.NotifyHelper, "IpuLoop");
            lblLoopD.DataBindings.Add("Visible", Data.NotifyHelper, "IpuLoop");
            lblPortD.DataBindings.Add("Visible", Data.NotifyHelper, "IpuLoop");
            lblLoopMsg.DataBindings.Add("Visible", Data.NotifyHelper, "IpuLoop");
            lblConnect.DataBindings.Add("Visible", Data.NotifyHelper, "IpuLoop");

            btnNext.DataBindings.Add("Enabled", Data.NotifyHelper, "Next");
            btnBack.DataBindings.Add("Enabled", Data.NotifyHelper, "Prev");
            btnCab.DataBindings.Add("Enabled", Data.NotifyHelper, "Cab");

            lblTC3.DataBindings.Add("Location", notifyHelper, "PosYlblTC3");
            lblTC4.DataBindings.Add("Location", notifyHelper, "PosYlblTC4");
            lblS2.DataBindings.Add("Location", notifyHelper, "PosYlblS2");

            lblSum.Text = bind_found.Count.ToString();

            Bind_found_CurrentItemChanged(bind_found, new EventArgs());
        }

        private void btnLocalDb_Click(object sender, EventArgs e)
        {
            bind_found.MoveNext();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            bind_found.MovePrevious();
        }

        private void Found_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.FromArgb(122, 150, 223),3);
            e.Graphics.DrawLine(pen, 82, 8, 82, 415);
        }

        private void btnCab_Click(object sender, EventArgs e)
        {
            DataRowView current = (DataRowView)(bind_found).Current;
            Pdf pdf = new Pdf();
            pdf.Text = Properties.Translations.frmPdfTextCab + " " + current["IPU"];
            pdf.Src = AppDomain.CurrentDomain.BaseDirectory + "cab/" + current["CAB"] + ".pdf";
            pdf.ShowDialog();
        }

        private void lbl_MouseHover(object sender, EventArgs e)
        {
            ((Label)sender).ForeColor = Color.Crimson;
        }

        private void lbl_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).ForeColor = lblsColors[((Label)sender).Name];
        }

        private void lblMsg_Click(object sender, EventArgs e)
        {
            string name = ((Label)sender).Name;
            switch (name)
            {
                case "lblIpu":
                    message = MessageType.Ipu;
                    break;
                case "lblLoop":
                    message = MessageType.Loop;
                    break;
                case "lblCcu":
                    message = MessageType.Ccu;
                    break;
                case "lblA1A2":
                    message = MessageType.Oc;
                    break;
                default:
                    message = MessageType.Ipu;
                    break;
            }
            SubmitAlarm submitAlarm = new SubmitAlarm(this.Data, message, this.bind_found);
            submitAlarm.ShowDialog();
        }
    }
}
