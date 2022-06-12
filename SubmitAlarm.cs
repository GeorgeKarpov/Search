using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Security.Principal;

#pragma warning disable IDE1006

namespace Search
{
    public partial class SubmitAlarm : Form
    {
        private BindingSource bind_alarms;
        private BindingSource bind_found;
        private bool edit;
        private DsSearch.AlarmsRow alarmsRowEdit;
        public Data Data;
        private MessageType message;

        public bool Edit { get => this.edit; set => this.edit = value; }

        public SubmitAlarm(Data data, MessageType message, BindingSource bind_found)
        {
            InitializeComponent();
            this.Icon = Properties.Resources.ALARM;
            bind_alarms = new BindingSource();
            Data = data;
            this.message = message;
            this.bind_found = bind_found;
            textBoxMsg.Validating += TextBox_Validating;
        }

        private void SubmitAlarm_Load(object sender, EventArgs e)
        {
            bind_alarms.DataSource = Data.DsSearch.Alarms;
            dataGridView1.DataSource = bind_alarms;
            HideColumns();
            if (!edit)
            {
                LoadData();
            }
            else
            {
                ShowEdit();
            }
        }

        private void ShowEdit()
        {
            //dataGridView1.Enabled = false;
            textBoxUser.Enabled = false;
            dateTimePicker1.Enabled = false;
            DataRowView alarmRowView = bind_alarms.Current as DataRowView;
            alarmsRowEdit = (DsSearch.AlarmsRow)alarmRowView.Row;
            textBoxMsg.Text = alarmsRowEdit.TEXT;
            textBoxCode.Text = alarmsRowEdit.CODE;
            textBoxUser.Text = alarmsRowEdit.USER;
            dateTimePicker1.Value = alarmsRowEdit.DATE_ADD;
            checkBoxAct.Checked = alarmsRowEdit.ACT;
            this.message = GetEditMsgtype(alarmsRowEdit);
            bind_alarms.Filter = "ID = " + alarmsRowEdit.ID;
            lblObj.Text = this.message.ToString().ToUpper();
            lblIpu.Text = alarmsRowEdit.IPU;
            switch (this.message)
            {
                case MessageType.Ipu:
                    this.Text = "IPU " + alarmsRowEdit.IPU;
                    lblObjName.Visible = false;
                    lblObjCon.Visible = false;                
                    break;
                case MessageType.Loop:
                    this.Text = "LOOP " + alarmsRowEdit.LOOP;
                    lblObjName.Text = this.message.ToString();
                    lblObjCon.Text = alarmsRowEdit.LOOP;
                    break;
                case MessageType.Ccu:
                    this.Text = "CCU " + alarmsRowEdit.CCU;
                    lblObjName.Text = this.message.ToString().ToUpper();
                    lblObjCon.Text = alarmsRowEdit.CCU;
                    break;
                case MessageType.Oc:
                    this.Text = "OC " + alarmsRowEdit.A1A2;
                    lblObjName.Text = this.message.ToString().ToUpper();
                    lblObjCon.Text = alarmsRowEdit.A1A2;
                    break;
                default:
                    break;
            }
        }

        private MessageType GetEditMsgtype(DsSearch.AlarmsRow alarmsRow)
        {
            if (!string.IsNullOrWhiteSpace(alarmsRow.IPU) &&
                alarmsRow.IsLOOPNull() &&
                alarmsRow.IsCCUNull() &&
                alarmsRow.IsA1A2Null())
            {
                return MessageType.Ipu;
            }
            else if (!string.IsNullOrWhiteSpace(alarmsRow.IPU) &&
                !alarmsRow.IsLOOPNull() && !string.IsNullOrWhiteSpace(alarmsRow.LOOP) &&
                alarmsRow.IsCCUNull() &&
                alarmsRow.IsA1A2Null())
            {
                return MessageType.Loop;
            }
            else if (!string.IsNullOrWhiteSpace(alarmsRow.IPU) &&
               alarmsRow.IsLOOPNull() &&
               !alarmsRow.IsCCUNull() && !string.IsNullOrWhiteSpace(alarmsRow.CCU) &&
               alarmsRow.IsA1A2Null())
            {
                return MessageType.Ccu;
            }
            else if (!string.IsNullOrWhiteSpace(alarmsRow.IPU) &&
                alarmsRow.IsLOOPNull() &&
                alarmsRow.IsCCUNull() &&
                !alarmsRow.IsA1A2Null() && !string.IsNullOrWhiteSpace(alarmsRow.A1A2))
            {
                return MessageType.Oc;
            }
            else return new MessageType();
        }

        private void HideColumns()
        {
            foreach (var col in dataGridView1.Columns)
            {
                ((DataGridViewColumn)col).Visible = false;
            }
            dataGridView1.Columns["ACT_EXCLAM"].Visible = true;
            dataGridView1.Columns["ACT_EXCLAM"].DisplayIndex = 0;
            dataGridView1.Columns["ACT_EXCLAM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["ACT_EXCLAM"].DefaultCellStyle.Font =
                new Font(dataGridView1.Font, FontStyle.Bold);
            dataGridView1.Columns["ACT_EXCLAM"].DefaultCellStyle.ForeColor = Color.Red;
            dataGridView1.Columns["DATE_ADD"].Visible = true;
            dataGridView1.Columns["DATE_ADD"].DisplayIndex = 1;
            dataGridView1.Columns["USER"].Visible = true;
            dataGridView1.Columns["USER"].DisplayIndex = 2;
            dataGridView1.Columns["TEXT"].Visible = true;
            dataGridView1.Columns["TEXT"].DisplayIndex = 3;
            dataGridView1.Columns["CODE"].Visible = true;
            dataGridView1.Columns["CODE"].DisplayIndex = 4;
            dataGridView1.Columns["CODE"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["DATE_ADD"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss";
        }

        private void TextBox_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(textBox,Properties.Translations.errorProvider1msg1);
                return;
            }
            errorProvider1.Clear();
        }

        private void ClearControls()
        {
            foreach (var txt in this.panelMessage.Controls.OfType<TextBox>())
            {
                txt.Text = null;
            }
            checkBoxAct.Checked = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        private void LoadData()
        {
            foreach (var lbl in this.panelObjLabels.Controls.OfType<Label>())
            {
                lbl.Visible = true;
            }
            DataRowView foundRowView = bind_found.Current as DataRowView;


            switch (this.message)
            {
                case MessageType.Ipu:
                    this.Text = "IPU " + foundRowView["IPU"].ToString();
                    lblObjName.Visible = false;
                    lblObjCon.Visible = false;
                    lblObj.Text = this.message.ToString().ToUpper();
                    lblIpu.Text = foundRowView["IPU"].ToString();
                    bind_alarms.Filter = "IPU = '" + foundRowView["IPU"] + "' AND LOOP Is Null AND CCU Is Null AND A1A2 Is Null";
                    break;
                case MessageType.Loop:
                    this.Text = "LOOP " + foundRowView["LOOP"].ToString();
                    lblObj.Text = this.message.ToString();
                    lblIpu.Text = foundRowView["IPU"].ToString();
                    lblObjName.Text = this.message.ToString();
                    lblObjCon.Text = foundRowView["LOOP"].ToString();
                    bind_alarms.Filter = 
                        @"IPU = '" + foundRowView["IPU"] + "' AND " +
                        "LOOP = '" + foundRowView["LOOP"] + "' AND " +
                        "CCU Is Null AND " +
                        "A1A2 Is Null";
                    break;
                case MessageType.Ccu:
                    this.Text = "CCU " + foundRowView["CCU"].ToString();
                    lblObj.Text = this.message.ToString().ToUpper();
                    lblIpu.Text = foundRowView["IPU"].ToString();
                    lblObjName.Text = this.message.ToString().ToUpper();
                    lblObjCon.Text = foundRowView["CCU"].ToString();
                    bind_alarms.Filter =
                        @"IPU = '" + foundRowView["IPU"] + "' AND " +
                        "LOOP Is Null AND " +
                        "CCU = '" + foundRowView["CCU"] + "' AND " +
                        "A1A2 Is Null";
                    break;
                case MessageType.Oc:
                    this.Text = "OC " + foundRowView["A1A2"].ToString();
                    lblObj.Text = this.message.ToString().ToUpper();
                    lblIpu.Text = foundRowView["IPU"].ToString();
                    lblObjName.Text = this.message.ToString().ToUpper();
                    lblObjCon.Text = foundRowView["A1A2"].ToString();
                    bind_alarms.Filter =
                        @"IPU = '" + foundRowView["IPU"] + "' AND " +
                        "LOOP Is Null AND " +
                        "CCU Is Null AND " +
                        "A1A2 = '" + foundRowView["A1A2"] + "'";
                    break;
                default:
                    break;
            }
        }

        private bool SaveData()
        {
            if (!edit)
            {
                DataRowView foundRowView = bind_found.Current as DataRowView;
                string userName = WindowsIdentity.GetCurrent().Name.Split('\\').Last();
                if (textBoxUser.TextLength > 0)
                {
                    userName = textBoxUser.Text;
                }
                DsSearch.AlarmsRow alarmsRow = Data.DsSearch.Alarms.NewAlarmsRow();
                alarmsRow.ACT = checkBoxAct.Checked;
                alarmsRow.ACT_EXCLAM = checkBoxAct.Checked ? "!" : null;
                alarmsRow.DATE_ADD = dateTimePicker1.Value;
                alarmsRow.USER = string.Format("[{0}:]", userName);
                alarmsRow.IPU = foundRowView["IPU"].ToString();
                switch (message)
                {
                    case MessageType.Loop:
                        alarmsRow.LOOP = foundRowView["LOOP"].ToString();
                        break;
                    case MessageType.Ccu:
                        alarmsRow.CCU = foundRowView["CCU"].ToString();
                        break;
                    case MessageType.Oc:
                        alarmsRow.A1A2 = foundRowView["A1A2"].ToString();
                        break;
                    default:
                        break;
                }
                alarmsRow.CODE = textBoxCode.Text;
                alarmsRow.TEXT = textBoxMsg.Text;
                Data.DsSearch.Alarms.AddAlarmsRow(alarmsRow);
            }         

            if (!Data.UpdateAlarms())
            {
                MessageBox.Show(Data.ErrMesg, Properties.Translations.DbErrorCaption, 
                    buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                Data.DsSearch.Alarms.RejectChanges();
                return false;
            }
            Data.DsSearch.Alarms.AcceptChanges();
            bind_alarms.Sort = "DATE_ADD DESC";
            return true;
        }

        private void SubmitAlarm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                if (!ValidateChildren(ValidationConstraints.Enabled))
                {
                    e.Cancel = true;
                    return;
                }
                if (!SaveData())
                {
                    e.Cancel = true;
                    return;
                }
            }
            else
            {
                errorProvider1.Clear();
                Data.DsSearch.Alarms.RejectChanges();
                bind_alarms.DataSource = Data.DsSearch.Alarms;
            }
            
            //ClearControls();

        }

        private void textBoxMsg_TextChanged(object sender, EventArgs e)
        {
            if (edit)
            {
                alarmsRowEdit.TEXT = textBoxMsg.Text;
            }
        }

        private void textBoxCode_TextChanged(object sender, EventArgs e)
        {
            if (edit)
            {
                alarmsRowEdit.CODE = textBoxCode.Text;
            }
        }

        private void checkBoxAct_CheckedChanged(object sender, EventArgs e)
        {
            if (edit)
            {
                alarmsRowEdit.ACT = checkBoxAct.Checked;
                //alarmsRowEdit.ACT_EXCLAM = checkBoxAct.Checked ? "!" : null;
            }
        }
    }
}
