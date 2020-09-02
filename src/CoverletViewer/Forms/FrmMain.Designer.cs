namespace CoverletViewer.Forms
{
    partial class FrmMain
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.lvwResult = new System.Windows.Forms.ListView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbOpen = new System.Windows.Forms.ToolStripButton();
            this.tsbRunDotnetTest = new System.Windows.Forms.ToolStripButton();
            this.tsbCopySelectedRows = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbGitHub = new System.Windows.Forms.ToolStripButton();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.cboView = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboCoverageLevel = new System.Windows.Forms.ComboBox();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvwResult
            // 
            this.lvwResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwResult.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvwResult.HideSelection = false;
            this.lvwResult.Location = new System.Drawing.Point(12, 78);
            this.lvwResult.Name = "lvwResult";
            this.lvwResult.Size = new System.Drawing.Size(1081, 546);
            this.lvwResult.TabIndex = 2;
            this.lvwResult.UseCompatibleStateImageBehavior = false;
            this.lvwResult.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvwResult_KeyDown);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbOpen,
            this.tsbRunDotnetTest,
            this.tsbCopySelectedRows,
            this.toolStripSeparator1,
            this.tsbGitHub});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1105, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbOpen
            // 
            this.tsbOpen.Image = global::CoverletViewer.Properties.Resources.png_open_folder_16x16;
            this.tsbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpen.Name = "tsbOpen";
            this.tsbOpen.Size = new System.Drawing.Size(100, 22);
            this.tsbOpen.Text = "Open json file";
            this.tsbOpen.Click += new System.EventHandler(this.tsbOpen_Click);
            // 
            // tsbRunDotnetTest
            // 
            this.tsbRunDotnetTest.Image = global::CoverletViewer.Properties.Resources.png_play_32_32;
            this.tsbRunDotnetTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRunDotnetTest.Name = "tsbRunDotnetTest";
            this.tsbRunDotnetTest.Size = new System.Drawing.Size(108, 22);
            this.tsbRunDotnetTest.Text = "Run dotnet test";
            this.tsbRunDotnetTest.Click += new System.EventHandler(this.tsbRunDotnetTest_Click);
            // 
            // tsbCopySelectedRows
            // 
            this.tsbCopySelectedRows.Image = global::CoverletViewer.Properties.Resources.png_copy_16x16;
            this.tsbCopySelectedRows.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCopySelectedRows.Name = "tsbCopySelectedRows";
            this.tsbCopySelectedRows.Size = new System.Drawing.Size(129, 22);
            this.tsbCopySelectedRows.Text = "Copy selected rows";
            this.tsbCopySelectedRows.Click += new System.EventHandler(this.tsbCopySelectedRows_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbGitHub
            // 
            this.tsbGitHub.Image = global::CoverletViewer.Properties.Resources.png_github_32_32;
            this.tsbGitHub.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbGitHub.Name = "tsbGitHub";
            this.tsbGitHub.Size = new System.Drawing.Size(122, 22);
            this.tsbGitHub.Text = "Project on GitHub";
            this.tsbGitHub.Click += new System.EventHandler(this.tsbGitHub_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(12, 52);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(727, 20);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Search in files result";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslVersion});
            this.statusStrip1.Location = new System.Drawing.Point(0, 634);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1105, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslVersion
            // 
            this.tsslVersion.Name = "tsslVersion";
            this.tsslVersion.Size = new System.Drawing.Size(1090, 17);
            this.tsslVersion.Spring = true;
            this.tsslVersion.Text = "{version}";
            this.tsslVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboView
            // 
            this.cboView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboView.FormattingEnabled = true;
            this.cboView.Location = new System.Drawing.Point(922, 52);
            this.cboView.Name = "cboView";
            this.cboView.Size = new System.Drawing.Size(171, 21);
            this.cboView.TabIndex = 1;
            this.cboView.SelectedIndexChanged += new System.EventHandler(this.cboView_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(919, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Result view mode";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(742, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Coverage level";
            // 
            // cboCoverageLevel
            // 
            this.cboCoverageLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboCoverageLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCoverageLevel.FormattingEnabled = true;
            this.cboCoverageLevel.Location = new System.Drawing.Point(745, 52);
            this.cboCoverageLevel.Name = "cboCoverageLevel";
            this.cboCoverageLevel.Size = new System.Drawing.Size(171, 21);
            this.cboCoverageLevel.TabIndex = 7;
            this.cboCoverageLevel.SelectedIndexChanged += new System.EventHandler(this.cboCoverageLevel_SelectedIndexChanged);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1105, 656);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboCoverageLevel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboView);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.lvwResult);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Coverlet Viewer - DDS Sistemas";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvwResult;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbOpen;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripButton tsbRunDotnetTest;
        private System.Windows.Forms.ToolStripButton tsbGitHub;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslVersion;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ComboBox cboView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboCoverageLevel;
        private System.Windows.Forms.ToolStripButton tsbCopySelectedRows;
    }
}

