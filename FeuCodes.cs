using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#pragma warning disable IDE1006

namespace Search
{
    public partial class FeuCodes : Form
    {
        private Data data;
        BindingSource bind_codes;
        BindingSource bind_descr;
        BindingSource bind_class;
        public FeuCodes(Data data)
        {
            InitializeComponent();
            this.Icon = Properties.Resources.Feu_Code;
            this.data = data;
            bind_codes = new BindingSource();
            bind_descr = new BindingSource();
            bind_class = new BindingSource();
            bind_codes.ListChanged += Bind_codes_ListChanged;
        }

        private void Bind_codes_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (bind_codes.Count == 0)
            {
                lblOverview.Visible = true;
            }
            else
            {
                lblOverview.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            
        }

        private void FeuCodes_Load(object sender, EventArgs e)
        {
            string[] postSource = data.DsSearch.feu_code
                    .AsEnumerable()
                    .Select<System.Data.DataRow, String>(x => x.Field<String>("code"))
                    .ToArray();

            var source = new AutoCompleteStringCollection();
            source.AddRange(postSource);
            textBoxCode.AutoCompleteCustomSource = source;
            textBoxCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBoxCode.AutoCompleteSource = AutoCompleteSource.CustomSource;


            bind_codes.DataSource = data.DsSearch;
            bind_codes.DataMember = data.DsSearch.feu_code.TableName;

            bind_class.DataSource = bind_codes; // data.DsSearch.Relations["feu_code_Opisanie"];
            bind_class.DataMember = "feu_code_Class";

            bind_descr.DataSource = bind_codes; // data.DsSearch.Relations["feu_code_Opisanie"];
            bind_descr.DataMember = "feu_code_Opisanie";

            bind_codes.Filter = "code Is Null";


            lblDescHead.DataBindings.Add("Text", bind_descr, data.DsSearch.Opisanie.zaglavieColumn.ColumnName);
            lblDescr.DataBindings.Add("Text", bind_descr, data.DsSearch.Opisanie.OpisanieColumn.ColumnName);
            lblCodeVal.DataBindings.Add("Text", bind_codes, data.DsSearch.feu_code.codeColumn.ColumnName);
            lblCodeDescrVal.DataBindings.Add("Text", bind_codes, data.DsSearch.feu_code.textColumn.ColumnName);
            lblCodeClsVal.DataBindings.Add("Text", bind_codes, data.DsSearch.feu_code.clasColumn.ColumnName);
            lblCodeActVal.DataBindings.Add("Text", bind_codes, data.DsSearch.feu_code.descriptColumn.ColumnName);

            pictureBoxSig.DataBindings.Add("Visible", bind_codes, data.DsSearch.feu_code.socColumn.ColumnName);
            pictureBoxPt.DataBindings.Add("Visible", bind_codes, data.DsSearch.feu_code.pocColumn.ColumnName);
            pictureBoxRel.DataBindings.Add("Visible", bind_codes, data.DsSearch.feu_code.rocColumn.ColumnName);

            labelSigOC.DataBindings.Add("Visible", bind_codes, data.DsSearch.feu_code.socColumn.ColumnName);
            labelPtOC.DataBindings.Add("Visible", bind_codes, data.DsSearch.feu_code.pocColumn.ColumnName);
            labelRelOC.DataBindings.Add("Visible", bind_codes, data.DsSearch.feu_code.rocColumn.ColumnName);

            lblCls.DataBindings.Add("Text", bind_class, data.DsSearch.Class.ClassColumn.ColumnName);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            bind_codes.Filter = "code = '" + textBoxCode.Text +"'";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Pdf pdf = new Pdf();
            pdf.Text = Properties.Translations.frmPdfTextFeu;
            pdf.Src = AppDomain.CurrentDomain.BaseDirectory + "docs/FEU_code.pdf";
            pdf.ShowDialog();
        }
    }
}
