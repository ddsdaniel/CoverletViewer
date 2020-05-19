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

            var jsonContent = File.ReadAllText(fileName);

            var jsonObject = JObject.Parse(jsonContent);
            var projects = ImportProjects(jsonObject);

            foreach (var project in projects)
            {
                AddLine(project, project.Name, project.PercentageCoverage, project.CoveredLines, project.TotalLines);

                foreach (var file in project.Files)
                    AddLine(file, $"    {file.Path}", file.PercentageCoverage, file.CoveredLines, file.TotalLines);
            }
        }

        private void AddLine(object indicator, string name, decimal coverlate, int coveredLines, int totalLines)
        {
            var item = new ListViewItem();
            for (var i = 0; i < lvwResult.Columns.Count - 1; i++)
                item.SubItems.Add("");
            item.Tag = indicator;
            item.SubItems[0].Text = name;
            item.SubItems[1].Text = $"{coverlate:0.0}%";
            item.SubItems[2].Text = $"{coveredLines}/{totalLines}";
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
            var file = lvwResult.Items[lvwResult.SelectedItems[0].Index].Tag as CodeFile;
            if (file != null)
            {
                var fileCoverage = new FrmFileCoverage(file);
                fileCoverage.ShowDialog();
            }
        }

        private List<Project> ImportProjects(JObject jObject)
        {
            var projects = new List<Project>();
            foreach (var prj in jObject)
            {
                var project = new Project(prj.Key)
                {
                    Files = ImportFiles(prj.Value)
                };

                projects.Add(project);

            }

            return projects;
        }

        private List<CodeFile> ImportFiles(JToken projectToken)
        {
            var arquivos = new List<CodeFile>();

            foreach (JProperty fileProperty in projectToken)
            {
                var file = new CodeFile { Path = fileProperty.Name };
                file.Classes = ImportClasses(fileProperty.Value);

                arquivos.Add(file);

            }
            return arquivos;
        }

        private List<Class> ImportClasses(JToken fileToken)
        {
            var classes = new List<Class>();

            foreach (JProperty classProperty in fileToken)
            {
                var newClass = new Class { Name = classProperty.Name };
                newClass.Methods = ImportMethods(classProperty.Value);

                classes.Add(newClass);

            }
            return classes;
        }

        private List<Method> ImportMethods(JToken classToken)
        {
            var methods = new List<Method>();

            foreach (JProperty methodProperty in classToken)
            {
                var metodo = new Method { Name = methodProperty.Name };
                ImportLines(metodo, methodProperty.Value);
                methods.Add(metodo);
            }
            return methods;
        }

        private void ImportLines(Method method, JToken methodToken)
        {
            foreach (JProperty linesProperty in methodToken)
            {
                if (linesProperty.Name == "Lines")
                {
                    foreach (JProperty lin in linesProperty.Value)
                    {
                        if (lin.Value.ToString() == "0")
                            method.NotCoveredLines.Add(Convert.ToInt32(lin.Name));
                        else
                            method.CoveredLines.Add(Convert.ToInt32(lin.Name));
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
            var found = false;
            foreach (ListViewItem item in lvwResult.Items)
            {
                if (item.SubItems[0].Text.ToLower().Contains(txtSearch.Text.ToLower()))
                {
                    item.ForeColor = lvwResult.ForeColor;

                    if (!found)
                        lvwResult.EnsureVisible(item.Index);

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
