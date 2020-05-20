using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CoverletViewer.Domain.Models;

namespace CoverletViewer.Forms
{
    public partial class FrmFileCoverage : Form
    {
        private readonly CodeFile _codeFile;
        private enum LineStatus
        {
            Ignored,
            Covered,
            NotCovered
        }

        public FrmFileCoverage(CodeFile codeFile)
        {
            InitializeComponent();
            _codeFile = codeFile;
        }

        private void FrmFileCoverage_Load(object sender, EventArgs e)
        {
            FormatListView();
            var lines = File.ReadAllLines(_codeFile.Path);
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                LineStatus lineStatus;
                if (_codeFile.Classes.Any(c => c.Methods.Any(m => m.NotCoveredLines.Contains(i + 1))))
                    lineStatus = LineStatus.NotCovered;
                else if (_codeFile.Classes.Any(c => c.Methods.Any(m => m.CoveredLines.Contains(i + 1))))
                    lineStatus = LineStatus.Covered;
                else
                    lineStatus = LineStatus.Ignored;

                AddLine(line, lineStatus, i + 1);
            }
            lvwResult.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void AddLine(string line, LineStatus lineStatus, int lineNumber)
        {
            var item = new ListViewItem();
            for (var i = 0; i < lvwResult.Columns.Count - 1; i++)
                item.SubItems.Add("");
            item.SubItems[0].Text = lineNumber.ToString("#,##0");
            item.SubItems[1].Text = line;

            Color color;
            switch (lineStatus)
            {
                case LineStatus.Ignored:
                    color = item.BackColor;
                    break;
                case LineStatus.Covered:
                    color = Color.LightGreen;
                    break;
                case LineStatus.NotCovered:
                    color = Color.LightCoral;
                    break;
                default:
                    throw new NotImplementedException($"LineStatus: {lineStatus}");
            }
            item.BackColor = color;
            lvwResult.Items.Add(item);
        }

        private void FormatListView()
        {
            lvwResult.Columns.Clear();
            lvwResult.View = View.Details;
            lvwResult.MultiSelect = false;
            lvwResult.FullRowSelect = true;
            lvwResult.AllowDrop = false;

            lvwResult.Columns.Add("#", (int)(lvwResult.Width * 0.1));
            lvwResult.Columns.Add("Line", (int)(lvwResult.Width * 0.85));
            lvwResult.Scrollable = true;
        }
    }
}
