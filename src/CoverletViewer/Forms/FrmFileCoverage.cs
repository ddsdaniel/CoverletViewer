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
        private readonly Arquivo _arquivo;
        private enum StatusLinha
        {
            Normal,
            Coberta,
            NaoCoberta
        }

        public FrmFileCoverage(Arquivo arquivo)
        {
            InitializeComponent();
            _arquivo = arquivo;
        }

        private void FrmFileCoverage_Load(object sender, EventArgs e)
        {
            FormatListView();
            var lines = File.ReadAllLines(_arquivo.Caminho);
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                StatusLinha statusLinha;
                if (_arquivo.Classes.Any(c => c.Metodos.Any(m => m.LinhasNaoCobertas.Contains(i + 1))))
                    statusLinha = StatusLinha.NaoCoberta;
                else if (_arquivo.Classes.Any(c => c.Metodos.Any(m => m.LinhasCobertas.Contains(i + 1))))
                    statusLinha = StatusLinha.Coberta;
                else
                    statusLinha = StatusLinha.Normal;

                AddLine(line, statusLinha, i + 1);
            }
            lvwResult.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void AddLine(string line, StatusLinha statusLinha, int lineNumber)
        {
            var item = new ListViewItem();
            for (var i = 0; i < lvwResult.Columns.Count - 1; i++)
                item.SubItems.Add("");
            //item.Tag = file;
            //item.ImageKey = extension;
            item.SubItems[0].Text = lineNumber.ToString("#,##0");
            item.SubItems[1].Text = line;

            Color cor;
            switch (statusLinha)
            {
                case StatusLinha.Normal:
                    cor = SystemColors.GrayText;// lvwResult.ForeColor;
                    break;
                case StatusLinha.Coberta:
                    cor = Color.Blue;
                    break;
                case StatusLinha.NaoCoberta:
                    cor = Color.Red;
                    break;
                default:
                    throw new NotImplementedException($"StatusLinha: {statusLinha}");
            }
            item.ForeColor = cor;
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
