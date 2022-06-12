using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#pragma warning disable IDE1006

namespace Search
{
    public partial class Main : Form
    {
        Data Data;
        private Query query = new Query();
        public Main()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icon;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClearSearchControls();
            Data = new Data();

            if (Properties.Settings.Default.MainLocation != null)
            {
                this.Location = Properties.Settings.Default.MainLocation;
            }

            foreach (var btn in this.grpBoxLang.Controls.OfType<Button>())
            {
                string lang = Properties.Settings.Default.CurrentLanguage;
                btn.Enabled = !btn.Name.ToLower().Contains(lang.ToLower());
            }
            ChangeDb(Properties.Settings.Default.RemoteDb);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.MainLocation = this.Location;
            Properties.Settings.Default.Save();
        }

        private void btnEng_Click(object sender, EventArgs e)
        {
            ChangeLang("EN");
        }

        private void btnRu_Click(object sender, EventArgs e)
        {
            ChangeLang("RU");
        }

        private void btnLocalDb_Click(object sender, EventArgs e)
        {
            ChangeDb(false);
        }

        private void ChangeLang(string lang)
        {
            var ci = new System.Globalization.CultureInfo(lang);
            if (!System.Threading.Thread.CurrentThread.CurrentCulture.Equals(ci))
            {
                Properties.Settings.Default.CurrentLanguage = lang;
                Properties.Settings.Default.Save();
                Application.Restart();
            }
        }

        private void ChangeDb(bool remote)
        {
            ClearSearchControls();
            if (remote)
            {
                if (!File.Exists(Properties.Settings.Default.DbRemotePath))
                {
                    DialogResult result = 
                    MessageBox.Show(Properties.Translations.msgRemoteDbError, Properties.Translations.DbErrorCaption,
                                buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        OpenFileDialog openFile = new OpenFileDialog();
                        openFile.Filter = "MS Access database | *.mdb";
                        openFile.DefaultExt = "mdb";
                        openFile.Multiselect = false;
                        if (openFile.ShowDialog() == DialogResult.OK)
                        {
                            Properties.Settings.Default.DbRemotePath = openFile.FileName;
                            Data.RemoteDbPath = openFile.FileName;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            Data.Remote = remote;
            if (!Data.GetData())
            {
                MessageBox.Show(Data.ErrMesg, Properties.Translations.DbErrorCaption,
                                buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                return;
            }
            Properties.Settings.Default.RemoteDb = remote;
            btnRemoteDb.Enabled = !Properties.Settings.Default.RemoteDb;
            btnLocalDb.Enabled = Properties.Settings.Default.RemoteDb;
            if (Properties.Settings.Default.RemoteDb)
            {
                lblDb.Text = Properties.Translations.LabelDbRemote;
            }
            else
            {
                lblDb.Text = Properties.Translations.LabelDbLocal;
            }
        }

        private void btnRemoteDb_Click(object sender, EventArgs e)
        {
            ChangeDb(true);
        }

        private void ClearSearchControls()
        {
            foreach (var cmb in this.panelSearch.Controls.OfType<ComboBox>())
            {
                cmb.Visible = false;
                cmb.DataSource = null;
            }

            foreach (var lbl in this.panelSearch.Controls.OfType<Label>())
            {
                lbl.Visible = false;
            }

            foreach (var btn in this.panelSearch.Controls.OfType<Button>())
            {
                btn.Visible = false;
                btn.Enabled = false;
            }
        }

        private void btnIPU_Click(object sender, EventArgs e)
        {
            query = Query.Ipu;
            QueryData();
        }

        private void QueryData()
        {
            ClearSearchControls();
            if (!Data.ConstructSources(query))
            {
                MessageBox.Show(Data.ErrMesg, Properties.Translations.DbErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                return;
            };
            switch (query)
            {
                case Query.Ipu:
                    comboBox1.DataSource = Data.Bind_1;
                    comboBox1.DisplayMember = "IPU";
                    comboBox1.ValueMember = "IPU";
                    break;
                case Query.IpuLoop:
                    comboBox1.DataSource = Data.Bind_1;
                    comboBox1.DisplayMember = "IPU";
                    comboBox1.ValueMember = "IPU";
                    comboBox2.DataSource = Data.Bind_2;
                    comboBox2.DisplayMember = "LOOP";
                    comboBox2.ValueMember = "LOOP";
                    break;
                case Query.IpuPort:
                    comboBox1.DataSource = Data.Bind_1;
                    comboBox1.DisplayMember = "IPU";
                    comboBox1.ValueMember = "IPU";
                    comboBox2.DataSource = Data.Bind_2;
                    comboBox2.DisplayMember = "PORT";
                    comboBox2.ValueMember = "PORT";
                    break;
                case Query.IpuCCU:
                    comboBox1.DataSource = Data.Bind_1;
                    comboBox1.DisplayMember = "IPU";
                    comboBox1.ValueMember = "IPU";
                    comboBox2.DataSource = Data.Bind_2;
                    comboBox2.DisplayMember = "CCU";
                    comboBox2.ValueMember = "CCU";
                    break;
                case Query.IpuOC:
                    comboBox1.DataSource = Data.Bind_1;
                    comboBox1.DisplayMember = "IPU";
                    comboBox1.ValueMember = "IPU";
                    comboBox2.DataSource = Data.Bind_2;
                    comboBox2.DisplayMember = "A1A2";
                    comboBox2.ValueMember = "A1A2";
                    break;
                case Query.Ser:
                    comboBox1.DataSource = Data.Bind_1;
                    comboBox1.DisplayMember = "SER";
                    comboBox1.ValueMember = "SER";
                    break;
                case Query.SerObc:
                    comboBox1.DataSource = Data.Bind_1;
                    comboBox1.DisplayMember = "SER";
                    comboBox1.ValueMember = "SER";
                    comboBox2.DataSource = Data.Bind_2;
                    comboBox2.DisplayMember = "OBC";
                    comboBox2.ValueMember = "OBC";
                    break;
                case Query.SerObcRack:
                    comboBox1.DataSource = Data.Bind_1;
                    comboBox1.DisplayMember = "SER";
                    comboBox1.ValueMember = "SER";
                    comboBox2.DataSource = Data.Bind_2;
                    comboBox2.DisplayMember = "OBC";
                    comboBox2.ValueMember = "OBC";
                    comboBox3.DataSource = Data.Bind_3;
                    comboBox3.DisplayMember = "H";
                    comboBox3.ValueMember = "H";
                    break;
                case Query.SerObcRackPos:
                    comboBox1.DataSource = Data.Bind_1;
                    comboBox1.DisplayMember = "SER";
                    comboBox1.ValueMember = "SER";
                    comboBox2.DataSource = Data.Bind_2;
                    comboBox2.DisplayMember = "OBC";
                    comboBox2.ValueMember = "OBC";
                    comboBox3.DataSource = Data.Bind_3;
                    comboBox3.DisplayMember = "H";
                    comboBox3.ValueMember = "H";
                    comboBox4.DataSource = Data.Bind_4;
                    comboBox4.DisplayMember = "POS";
                    comboBox4.ValueMember = "POS";
                    break;
                case Query.Place:
                    comboBox1.DataSource = Data.Bind_1;
                    comboBox1.DisplayMember = "PLACE";
                    comboBox1.ValueMember = "PLACE";
                    break;
                case Query.PlaceObj:
                case Query.CabLay:
                    comboBox1.DataSource = Data.Bind_1;
                    comboBox1.DisplayMember = "PLACE";
                    comboBox1.ValueMember = "PLACE";
                    comboBox2.DataSource = Data.Bind_2;
                    comboBox2.DisplayMember = "OBJ";
                    comboBox2.ValueMember = "OBJ";
                    break;
                default:
                    break;
            }          
            //ClearSearchControls();
            LoadSearchControls();
        }

        private void LoadSearchControls()
        {
            btnSearch.Visible = true;
            btnSearch.Enabled = true;
            switch (query)
            {
                case Query.Ipu:
                    label1.Text = "IPU";
                    comboBox1.Visible = true;
                    label1.Visible = true;
                    break;
                case Query.IpuLoop:
                    label1.Text = "IPU";
                    label2.Text = "Loop";
                    comboBox1.Visible = true;
                    label1.Visible = true;
                    comboBox2.Visible = true;
                    label2.Visible = true;
                    btnLoopPort.Visible = true;
                    btnLoopPort.Enabled = true;
                    break;
                case Query.IpuPort:
                    label1.Text = "IPU";
                    label2.Text = "Port";
                    comboBox1.Visible = true;
                    label1.Visible = true;
                    comboBox2.Visible = true;
                    label2.Visible = true;
                    break;
                case Query.IpuCCU:
                    label1.Text = "IPU";
                    label2.Text = "CCU";
                    comboBox1.Visible = true;
                    label1.Visible = true;
                    comboBox2.Visible = true;
                    label2.Visible = true;
                    break;
                case Query.IpuOC:
                    label1.Text = "IPU";
                    label2.Text = "A1A2";
                    comboBox1.Visible = true;
                    label1.Visible = true;
                    comboBox2.Visible = true;
                    label2.Visible = true;
                    break;
                case Query.Ser:
                    label1.Text = "SER";
                    comboBox1.Visible = true;
                    label1.Visible = true;
                    break;
                case Query.SerObc:
                    label1.Text = "SER";
                    label2.Text = "OBC";
                    comboBox1.Visible = true;
                    label1.Visible = true;
                    comboBox2.Visible = true;
                    label2.Visible = true;
                    break;
                case Query.SerObcRack:
                    label1.Text = "SER";
                    label2.Text = "OBC";
                    label3.Text = "Rack";
                    comboBox1.Visible = true;
                    label1.Visible = true;
                    comboBox2.Visible = true;
                    label2.Visible = true;
                    comboBox3.Visible = true;
                    label3.Visible = true;
                    break;
                case Query.SerObcRackPos:
                    label1.Text = "SER";
                    label2.Text = "OBC";
                    label3.Text = "Rack";
                    label4.Text = "Pos";
                    comboBox1.Visible = true;
                    label1.Visible = true;
                    comboBox2.Visible = true;
                    label2.Visible = true;
                    comboBox3.Visible = true;
                    label3.Visible = true;
                    comboBox4.Visible = true;
                    label4.Visible = true;
                    break;
                case Query.Place:
                    label1.Text = Properties.Translations.LabelPlace;
                    label1.Visible = true;
                    comboBox1.Visible = true;
                    break;
                case Query.PlaceObj:
                case Query.CabLay:
                    label1.Text = Properties.Translations.LabelPlace;
                    label1.Visible = true;
                    comboBox1.Visible = true;
                    label2.Text = Properties.Translations.LabelObject;
                    label2.Visible = true;
                    comboBox2.Visible = true;
                    break;
                default:
                    break;
            }
            
            //label1.DataBindings.Add("Text", Data.Bind_search, "PORT");
        }

        private void btnLoop_Click(object sender, EventArgs e)
        {
            query = Query.IpuLoop;
            QueryData();
        }

        private void btnPort_Click(object sender, EventArgs e)
        {
            query = Query.IpuPort;
            QueryData();
        }

        private void btnCCU_Click(object sender, EventArgs e)
        {
            query = Query.IpuCCU;
            QueryData();
        }

        private void btnOC_Click(object sender, EventArgs e)
        {
            query = Query.IpuOC;
            QueryData();
        }

        private void btnSer_Click(object sender, EventArgs e)
        {
            query = Query.Ser;
            QueryData();
        }

        private void btnOBC_Click(object sender, EventArgs e)
        {
            query = Query.SerObc;
            QueryData();
        }


        private void btnRack_Click(object sender, EventArgs e)
        {
            query = Query.SerObcRack;
            QueryData();
        }

        private void btnPos_Click(object sender, EventArgs e)
        {
            query = Query.SerObcRackPos;
            QueryData();
        }

        private void btnPlace_Click(object sender, EventArgs e)
        {
            query = Query.Place;
            QueryData();
        }

        private void btnObj_Click(object sender, EventArgs e)
        {
            query = Query.PlaceObj;
            QueryData();
        }

        private void btnCabLayout_Click(object sender, EventArgs e)
        {
            query = Query.CabLay;
            QueryData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Found found = new Found();
            found.Data = this.Data;
            found.Bind_found = Data.Bind_search;
            found.ShowDialog();
        }

        private void btnAlarms_Click(object sender, EventArgs e)
        {
            Alarms alarms = new Alarms(this.Data);
            alarms.ShowDialog();
        }

        private void btnLoopPort_Click(object sender, EventArgs e)
        {
            Pdf pdf = new Pdf();
            pdf.Src = AppDomain.CurrentDomain.BaseDirectory + "loop/" + comboBox1.SelectedValue + ".pdf";
            pdf.Text = Properties.Translations.frmPdfTextLoop + " " + comboBox1.SelectedValue;
            pdf.ShowDialog();
        }

        private void btnCodes_Click(object sender, EventArgs e)
        {
            FeuCodes feuCodes = new FeuCodes(this.Data);
            feuCodes.ShowDialog();
        }
    }
}
