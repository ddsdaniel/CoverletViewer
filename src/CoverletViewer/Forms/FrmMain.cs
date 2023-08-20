using System;
using System.Windows.Forms;
using CoverletViewer.Domain.Models;
using CoverletViewer.Domain.Services;
using System.IO;
using System.Diagnostics;
using CoverletViewer.Properties;
using System.Reflection;
using System.Collections.Generic;
using CoverletViewer.Extensions;
using CoverletViewer.Enums;
using System.Text;
using static System.Windows.Forms.ListViewItem;
using System.Linq;

namespace CoverletViewer.Forms
{
    public partial class FrmMain : Form
    {
        private List<Result> _lastResult;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            SetStatus("");
            Icon = Resources.ico_coverlet_viewer;
            tsslVersion.Text = $"v{Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}";
            FormatListView();
            LoadViewMode();
            LoadCoverageLevels();
        }

        private void SetStatus(string status)
        {
            tsslStatus.Text = status;
            Application.DoEvents();
        }

        private void LoadCoverageLevels()
        {
            cboCoverageLevel.Items.AddItem((int)CoverageLevel.All, "All");
            cboCoverageLevel.Items.AddItem((int)CoverageLevel.FullyCovered, "Fully covered");
            cboCoverageLevel.Items.AddItem((int)CoverageLevel.PartiallyCovered, "Partially covered");
            cboCoverageLevel.Items.AddItem((int)CoverageLevel.HalfOrLessCoverage, "50% or less coverage");
            cboCoverageLevel.Items.AddItem((int)CoverageLevel.FullyUncovered, "Fully uncovered");
            cboCoverageLevel.SelectByValue((int)CoverageLevel.All);
        }

        private void LoadViewMode()
        {
            cboView.Items.AddItem((int)ResultView.FolderStructure, "Folder structure");
            cboView.Items.AddItem((int)ResultView.FilePath, "File path");
            cboView.SelectByValue((int)ResultView.FolderStructure);
        }

        private void Import(string fileName)
        {
            SetStatus("importing the file...");
            var importService = new ImportService();
            var projects = importService.Import(fileName);

            SetStatus("Organizing the result...");
            var organizeService = new ResultsOrganizerService(projects);
            _lastResult = organizeService.Organize();

            LoadResults();
        }

        private void LoadResults()
        {
            if (_lastResult == null)
                return;

            SetStatus("Applying the filters...");

            lvwResult.Items.Clear();

            var filteredResults = cboView.GetSelectedValue() == (int)ResultView.FilePath
                ? _lastResult.FindAll(r => r.CodeFile != null)
                : _lastResult;

            switch ((CoverageLevel)cboCoverageLevel.GetSelectedValue())
            {
                case CoverageLevel.FullyCovered:
                    filteredResults = filteredResults.FindAll(r => r.CoveredLines == r.TestableLines);
                    break;
                case CoverageLevel.PartiallyCovered:
                    filteredResults = filteredResults.FindAll(r => r.CoveredLines != r.TestableLines);
                    break;
                case CoverageLevel.HalfOrLessCoverage:
                    filteredResults = filteredResults.FindAll(r => r.PercentageCoverage <= 50);
                    break;
                case CoverageLevel.FullyUncovered:
                    filteredResults = filteredResults.FindAll(r => r.CoveredLines == 0);
                    break;
            }

            filteredResults = filteredResults.FindAll(r => (cboView.GetSelectedValue() == (int)ResultView.FilePath ? r.CodeFile.Path : r.Name).ToLower().Contains(txtSearch.Text.ToLower()));

            SetStatus("Displaying the results...");

            foreach (var result in filteredResults)
            {
                AddLine(result);
            }

            var contFiles = filteredResults.Where(r => r.CodeFile != null).Count();
            SetStatus($"{contFiles} file(s)");
        }

        private void AddLine(Result result)
        {
            var item = new ListViewItem();
            for (var i = 0; i < lvwResult.Columns.Count - 1; i++)
                item.SubItems.Add("");
            item.Tag = result;
            item.SubItems[0].Text = cboView.GetSelectedValue() == (int)ResultView.FilePath
                ? result.CodeFile.Path
                : result.Name;
            item.SubItems[1].Text = $"{result.CoveredLines}/{result.TestableLines}";
            item.SubItems[2].Text = $"{result.PercentageCoverage:0.0}%";
            lvwResult.Items.Add(item);
        }

        private void FormatListView()
        {
            lvwResult.Columns.Clear();
            lvwResult.View = View.Details;
            lvwResult.FullRowSelect = true;
            lvwResult.AllowDrop = false;

            lvwResult.Columns.Add("Item", (int)(lvwResult.Width * 0.75));
            lvwResult.Columns.Add("Lines", (int)(lvwResult.Width * 0.1), HorizontalAlignment.Right);
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
                    var fileCoverage = new FrmFileCoverage(result.CodeFile) { Icon = Icon };
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
            LoadResults();
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

                SetStatus("Deleting old files...");
                var folderSolutionPath = Path.GetDirectoryName(openFileSolution.FileName);
                var coverageJsonFiles = Directory.GetFiles(folderSolutionPath, "coverage.json", SearchOption.AllDirectories);
                foreach (var oldFile in coverageJsonFiles)
                {
                    File.Delete(oldFile);
                }

                SetStatus("Running tests...");
                var msDosService = new MsDosService();

                var commandLine = $"dotnet test \"{openFileSolution.FileName}\" ";
                commandLine += "/p:CollectCoverage=true ";
                commandLine += "/p:CoverletOutput=..\\results\\coverage ";
                commandLine += "/p:MergeWith=..\\results\\coverage.json ";
                commandLine += "/p:CoverletOutputFormat=\"json\" ";
                commandLine += "/p:SkipAutoProps=true";

                var result = msDosService.Run(commandLine);

                coverageJsonFiles = Directory.GetFiles(folderSolutionPath, "coverage.json", SearchOption.AllDirectories);

                Cursor = Cursors.Default;

                if (coverageJsonFiles.Length > 0)
                {
                    Import(coverageJsonFiles[0]);
                }
                else
                {
                    SetStatus("");
                    MessageBox.Show("Failed to run tests or Coverlet not installed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

        }

        private void tsbGitHub_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/ddsdaniel/CoverletViewer");
        }

        private void cboView_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadResults();
        }

        private void cboCoverageLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadResults();
        }

        private void tsbCopySelectedRows_Click(object sender, EventArgs e)
        {
            CopySelectedRows();
        }

        private void CopySelectedRows()
        {
            if (lvwResult.SelectedItems.Count == 0)
                return;

            var sb = new StringBuilder();

            foreach (ColumnHeader column in lvwResult.Columns)
            {
                sb.Append(column.Text + "\t");
            }
            sb.AppendLine("");

            foreach (ListViewItem item in lvwResult.SelectedItems)
            {
                foreach (ListViewSubItem subItem in item.SubItems)
                {
                    sb.Append(subItem.Text + "\t");
                }
                sb.AppendLine("");
            }

            Clipboard.Clear();
            Clipboard.SetText(sb.ToString());
        }

        private void lvwResult_KeyDown(object sender, KeyEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                if (e.KeyCode == Keys.A)
                {
                    foreach (ListViewItem item in lvwResult.Items)
                    {
                        item.Selected = true;
                    }
                }
                else if (e.KeyCode == Keys.C)
                {
                    CopySelectedRows();
                }
            }
        }
    }
}
