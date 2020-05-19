using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

            var jsonContent = System.IO.File.ReadAllText(fileName);

            var jsonObject = JObject.Parse(jsonContent); // parse as array  
            var projetos = ImportarProjetos(jsonObject);

            foreach (var projeto in projetos)
            {
                AddLine(projeto, projeto.Nome, projeto.PercentualCobertura, projeto.TotalLinhasCobertas, projeto.TotalLinhas);

                foreach (var arquivo in projeto.Arquivos)
                    AddLine(arquivo, $"    {arquivo.Caminho}", arquivo.PercentualCobertura, arquivo.TotalLinhasCobertas, arquivo.TotalLinhas);
            }
        }

        private void AddLine(object indicador, string name, decimal coverlate, int linhasCobertas, int totalLinhas)
        {
            var item = new ListViewItem();
            for (var i = 0; i < lvwResult.Columns.Count - 1; i++)
                item.SubItems.Add("");
            item.Tag = indicador;
            //item.ImageKey = extension;
            item.SubItems[0].Text = name;
            item.SubItems[1].Text = $"{coverlate:0.0}%";
            item.SubItems[2].Text = $"{linhasCobertas}/{totalLinhas}";
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
            lvwResult.Columns.Add("Coverage", (int)(lvwResult.Width * 0.1));
            lvwResult.Columns.Add("Lines", (int)(lvwResult.Width * 0.2));
            lvwResult.Scrollable = true;

            lvwResult.MouseDoubleClick += lvwResult_MouseDoubleClick;
        }

        private void lvwResult_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var file = lvwResult.Items[lvwResult.SelectedItems[0].Index].Tag as Arquivo;
            if (file != null)
            {
                var fileCoverage = new FrmFileCoverage(file);
                fileCoverage.ShowDialog();
            }
        }

        private List<Projeto> ImportarProjetos(JObject objeto)
        {
            var projetos = new List<Projeto>();
            foreach (var prj in objeto)
            {
                var projeto = new Projeto(prj.Key);
                projeto.Arquivos = ImportarArquivos(prj.Value);

                projetos.Add(projeto);

            }

            return projetos;
        }

        private List<Arquivo> ImportarArquivos(JToken projeto)
        {
            var arquivos = new List<Arquivo>();

            foreach (JProperty arq in (JToken)projeto)
            {
                var arquivo = new Arquivo { Caminho = arq.Name };
                arquivo.Classes = ImportarClasses(arq.Value);

                arquivos.Add(arquivo);

            }
            return arquivos;
        }

        private List<Classe> ImportarClasses(JToken arquivo)
        {
            var classes = new List<Classe>();

            foreach (JProperty cls in arquivo)
            {
                var classe = new Classe { Nome = cls.Name };
                classe.Metodos = ImportarMetodos(cls.Value);

                classes.Add(classe);

            }
            return classes;
        }

        private List<Metodo> ImportarMetodos(JToken classe)
        {
            var metodos = new List<Metodo>();

            foreach (JProperty met in classe)
            {
                var metodo = new Metodo { Nome = met.Name };
                ImportarLinhas(metodo, met.Value);
                metodos.Add(metodo);
            }
            return metodos;
        }

        private void ImportarLinhas(Metodo metodo1, JToken metodo)
        {
            foreach (JProperty linhasProperty in metodo)
            {
                if (linhasProperty.Name == "Lines")
                {
                    foreach (JProperty lin in linhasProperty.Value)
                    {
                        if (lin.Value.ToString() == "0")
                            metodo1.LinhasNaoCobertas.Add(Convert.ToInt32(lin.Name));
                        else
                            metodo1.LinhasCobertas.Add(Convert.ToInt32(lin.Name));
                    }
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
            var achou = false;
            foreach (ListViewItem item in lvwResult.Items)
            {
                if (item.SubItems[0].Text.ToLower().Contains(txtSearch.Text.ToLower()))
                {
                    item.ForeColor = lvwResult.ForeColor;

                    if (!achou)
                        lvwResult.EnsureVisible(item.Index);

                    achou = true;
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
