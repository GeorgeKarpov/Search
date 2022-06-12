using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#pragma warning disable IDE1006

namespace Search
{
    public partial class Alarms : Form
    {
        private BindingSource bind_alarms;
        private Data data;
        public Alarms(Data data)
        {
            InitializeComponent();
            this.Icon = Properties.Resources.ALARM;
            this.data = data;
            bind_alarms = new BindingSource();
            dataGridView1.DataSource = bind_alarms;
            bind_alarms.ListChanged += Bind_alarms_CurrentItemChanged;
        }

        private void Bind_alarms_CurrentItemChanged(object sender, EventArgs e)
        {
            btnDelete.Enabled = dataGridView1.RowCount > 0;
            btnExport.Enabled = data.DsSearch.Alarms.Count > 0;
        }

        private void Alarms_Load(object sender, EventArgs e)
        {
            bind_alarms.DataSource = data.DsSearch.Alarms;
            bind_alarms.Filter = null;
            bind_alarms.Sort = "DATE_ADD DESC";
            LoadColumns();
            Bind_alarms_CurrentItemChanged(bind_alarms, new EventArgs());
        }

        private void LoadColumns()
        {
            foreach (var col in dataGridView1.Columns)
            {
                ((DataGridViewColumn)col).Visible = false;
            }
            dataGridView1.Columns["ACT_EXCLAM"].Visible = true;
            dataGridView1.Columns["ACT_EXCLAM"].DisplayIndex = 0;
            dataGridView1.Columns["ACT_EXCLAM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns["ACT_EXCLAM"].DefaultCellStyle.Font =
                new Font(dataGridView1.Font, FontStyle.Bold);
            dataGridView1.Columns["ACT_EXCLAM"].HeaderText = Properties.Translations.AlarmColAct;
            dataGridView1.Columns["ACT_EXCLAM"].DefaultCellStyle.ForeColor = Color.Red;
            dataGridView1.Columns["DATE_ADD"].Visible = true;
            dataGridView1.Columns["DATE_ADD"].DisplayIndex = 1;
            dataGridView1.Columns["DATE_ADD"].HeaderText = Properties.Translations.AlarmColDate;
            dataGridView1.Columns["USER"].Visible = true;
            dataGridView1.Columns["USER"].DisplayIndex = 2;
            dataGridView1.Columns["User"].HeaderText = Properties.Translations.AlarmColUser;
            dataGridView1.Columns["OBJECT"].Visible = true;
            dataGridView1.Columns["OBJECT"].DisplayIndex = 3;
            dataGridView1.Columns["OBJECT"].HeaderText = Properties.Translations.AlarmColObject;
            dataGridView1.Columns["TEXT"].Visible = true;
            dataGridView1.Columns["TEXT"].DisplayIndex = 4;
            dataGridView1.Columns["TEXT"].HeaderText = Properties.Translations.AlarmColText;
            dataGridView1.Columns["CODE"].Visible = true;
            dataGridView1.Columns["CODE"].DisplayIndex = 5;
            dataGridView1.Columns["CODE"].HeaderText = Properties.Translations.AlarmColCode;
            dataGridView1.Columns["CODE"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["DATE_ADD"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss";
        }

        private void btnIpu_Click(object sender, EventArgs e)
        {
            bind_alarms.Filter = "IPU Is Not Null AND LOOP Is Null AND CCU Is Null AND A1A2 Is Null";
        }

        private void btnLoop_Click(object sender, EventArgs e)
        {
            bind_alarms.Filter =
                        @"IPU Is Not Null AND " +
                        "LOOP Is Not Null AND " +
                        "CCU Is Null AND " +
                        "A1A2 Is Null";
        }

        private void btnCcu_Click(object sender, EventArgs e)
        {
            bind_alarms.Filter =
                        @"IPU Is Not Null AND " +
                        "LOOP Is Null AND " +
                        "CCU Is Not Null AND " +
                        "A1A2 Is Null";
        }

        private void btnOC_Click(object sender, EventArgs e)
        {
            bind_alarms.Filter =
                       @"IPU Is Not Null AND " +
                       "LOOP Is Null AND " +
                       "CCU Is Null AND " +
                       "A1A2 Is Not Null";
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            bind_alarms.Filter = null;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = 
                MessageBox.Show(Properties.Translations.msgDelAlarmText, Properties.Translations.msgDelAlarmTitle, 
                MessageBoxButtons.YesNo, icon: MessageBoxIcon.Warning, 
                defaultButton: MessageBoxDefaultButton.Button2);
            if (dialogResult != DialogResult.Yes)
            {
                return;
            }
            dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
            if (!data.UpdateAlarms())
            {
                MessageBox.Show(data.ErrMesg, Properties.Translations.DbErrorCaption,
                    buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                data.DsSearch.Alarms.RejectChanges();
            }
            data.DsSearch.Alarms.AcceptChanges();
            bind_alarms.Sort = "DATE_ADD DESC";
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Text Files | *.txt";
            saveFile.DefaultExt = "txt";
            saveFile.FileName = "History.txt";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Join("\t", 
                    Properties.Translations.AlarmColAct,
                    Properties.Translations.AlarmColDate,
                    Properties.Translations.AlarmColUser,
                    Properties.Translations.AlarmColObject,
                    Properties.Translations.AlarmColText,
                    Properties.Translations.AlarmColCode));
                sb.AppendLine();
                foreach (DsSearch.AlarmsRow row in data.DsSearch.Alarms.Rows)
                {
                    string[] fields = row.ItemArray.Select(field => field.ToString()).
                                                    ToArray();
                    sb.AppendLine(string.Join("\t", 
                        row.ACT_EXCLAM,
                        row.DATE_ADD.ToString("dd.MM.yyyy HH:mm:ss"), 
                        row.USER, 
                        row.OBJECT.Trim(), 
                        row.TEXT, 
                        row.CODE ));
                }

                File.WriteAllText(saveFile.FileName, sb.ToString());
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SubmitAlarm submitAlarm = new SubmitAlarm(data, new MessageType(), data.Bind_search);
            submitAlarm.Edit = true;
            submitAlarm.ShowDialog();
        }
    }
}
