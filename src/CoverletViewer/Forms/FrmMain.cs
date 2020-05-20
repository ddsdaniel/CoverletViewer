using System;
using System.Drawing;
using System.Windows.Forms;
using CoverletViewer.Domain.Models;
using CoverletViewer.Domain.Services;
using System.IO;

namespace CoverletViewer.Forms
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            FormatListView();
        }

        private void Import(string fileName)
        {
            lvwResult.Items.Clear();

            var importService = new ImportService();
            var projects = importService.Import(fileName);

            var organizeService = new ResultsOrganizerService(projects);
            var results = organizeService.Organize();

            foreach (var result in results)
            {
                AddLine(result);
            }
        }

        private void AddLine(Result result)
        {
            var item = new ListViewItem();
            for (var i = 0; i < lvwResult.Columns.Count - 1; i++)
                item.SubItems.Add("");
            item.Tag = result;
            item.SubItems[0].Text = result.Name;
            item.SubItems[1].Text = $"{result.CoveredLines}/{result.TestableLines}";
            item.SubItems[2].Text = $"{result.PercentageCoverage:0.0}%";
            lvwResult.Items.Add(item);
        }

        private void FormatListView()
        {
            lvwResult.Columns.Clear();
            lvwResult.View = View.Details;
            lvwResult.MultiSelect = false;
            lvwResult.FullRowSelect = true;
            lvwResult.AllowDrop = false;

            lvwResult.Columns.Add("Item", (int)(lvwResult.Width * 0.65));
            lvwResult.Columns.Add("Lines", (int)(lvwResult.Width * 0.2), HorizontalAlignment.Right);
            lvwResult.Columns.Add("Coverage", (int)(lvwResult.Width * 0.1), HorizontalAlignment.Right);
            lvwResult.Scrollable = true;

            lvwResult.MouseDoubleClick += lvwResult_MouseDoubleClick;
        }

        private void lvwResult_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var result = lvwResult.Items[lvwResult.SelectedItems[0].Index].Tag as Result;
            if (result != null)
            {
                if (result.CodeFile != null)
                {
                    var fileCoverage = new FrmFileCoverage(result.CodeFile);
                    fileCoverage.ShowDialog();
                }
            }
        }

        private void tsbOpen_Click(object sender, EventArgs e)
        {
            var openFile = new OpenFileDialog
            {
                Filter = "*Coverlet JSON Files (*.json)|*.json"
            };
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                Import(openFile.FileName);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchInResults();
        }

        private void SearchInResults()
        {
            var found = false;
            foreach (ListViewItem item in lvwResult.Items)
            {
                if (item.SubItems[0].Text.ToLower().Contains(txtSearch.Text.ToLower()))
                {
                    item.ForeColor = lvwResult.ForeColor;

                    if (!found)
                    {
                        lvwResult.EnsureVisible(item.Index);
                        item.Selected = true;
                        lvwResult.HideSelection = false;
                    }
                    found = true;
                }
                else
                {
                    item.ForeColor = SystemColors.GrayText;
                }
            }
        }

        private void tsbRunDotnetTest_Click(object sender, EventArgs e)
        {
            var openFileSolution = new OpenFileDialog
            {
                Filter = "*Solution Files (*.sln)|*.sln"
            };
            if (openFileSolution.ShowDialog() == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;

                var msDosService = new MsDosService();

                var commandLine = $"dotnet test \"{openFileSolution.FileName}\" /p:CollectCoverage=true ";
                commandLine += "/p:CoverletOutput=..\\results\\coverage ";
                commandLine += "/p:MergeWith=..\\results\\coverage.json ";
                commandLine += "/p:CoverletOutputFormat=\"json\"";

                var result = msDosService.Run(commandLine);

                var folderSolutionPath = Path.GetDirectoryName(openFileSolution.FileName);
                var coverageJsonFiles = Directory.GetFiles(folderSolutionPath, "coverage.json", SearchOption.AllDirectories);

                Cursor = Cursors.Default;

                if (coverageJsonFiles.Length > 0)
                    Import(coverageJsonFiles[0]);
                else
                    MessageBox.Show(result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }
    }
}
