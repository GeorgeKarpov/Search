
namespace Search
{
    partial class Alarms
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Alarms));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnIpu = new System.Windows.Forms.Button();
            this.btnLoop = new System.Windows.Forms.Button();
            this.btnCcu = new System.Windows.Forms.Button();
            this.btnOC = new System.Windows.Forms.Button();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // btnIpu
            // 
            resources.ApplyResources(this.btnIpu, "btnIpu");
            this.btnIpu.Name = "btnIpu";
            this.btnIpu.UseVisualStyleBackColor = true;
            this.btnIpu.Click += new System.EventHandler(this.btnIpu_Click);
            // 
            // btnLoop
            // 
            resources.ApplyResources(this.btnLoop, "btnLoop");
            this.btnLoop.Name = "btnLoop";
            this.btnLoop.UseVisualStyleBackColor = true;
            this.btnLoop.Click += new System.EventHandler(this.btnLoop_Click);
            // 
            // btnCcu
            // 
            resources.ApplyResources(this.btnCcu, "btnCcu");
            this.btnCcu.Name = "btnCcu";
            this.btnCcu.UseVisualStyleBackColor = true;
            this.btnCcu.Click += new System.EventHandler(this.btnCcu_Click);
            // 
            // btnOC
            // 
            resources.ApplyResources(this.btnOC, "btnOC");
            this.btnOC.Name = "btnOC";
            this.btnOC.UseVisualStyleBackColor = true;
            this.btnOC.Click += new System.EventHandler(this.btnOC_Click);
            // 
            // btnAll
            // 
            resources.ApplyResources(this.btnAll, "btnAll");
            this.btnAll.Name = "btnAll";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnDelete
            // 
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Alarms
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAll);
            this.Controls.Add(this.btnOC);
            this.Controls.Add(this.btnCcu);
            this.Controls.Add(this.btnLoop);
            this.Controls.Add(this.btnIpu);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Alarms";
            this.Load += new System.EventHandler(this.Alarms_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnIpu;
        private System.Windows.Forms.Button btnLoop;
        private System.Windows.Forms.Button btnCcu;
        private System.Windows.Forms.Button btnOC;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button button1;
    }
}