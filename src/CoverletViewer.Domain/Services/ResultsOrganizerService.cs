using CoverletViewer.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CoverletViewer.Domain.Services
{
    public class ResultsOrganizerService
    {
        private readonly List<Project> _projects;

        public ResultsOrganizerService(List<Project> projects)
        {
            _projects = projects;
        }

        public List<Result> Organize()
        {
            var results = new List<Result>();

            //TODO: acho que nao precisa desta conversao, isso facilitaria encontrar o arquivo no projeto, seria o proprio
            var files = _projects.SelectMany(p => p.Files.Select(f => f.Path)).ToList();

            foreach (var file in files)
            {
                var folders = file.Split('\\');
                var fullFolderPath = "";

                //TODO: revisar aqui
                var codeFile = _projects.Select(p => p.Files.FirstOrDefault(f => f.Path.ToLower() == file.ToLower())).FirstOrDefault();
                //if (codeFile != null)
                //{
                for (int i = 0; i < folders.Length - 1; i++)
                {
                    var folder = folders[i];
                    fullFolderPath += $"{folder}\\";

                    var result = results.FirstOrDefault(r => r.Name == fullFolderPath);

                    if (result == null)
                    {
                        result = new Result(fullFolderPath);
                        results.Add(result);
                    }
                    result.Increment(codeFile);
                }
                //}
            }

            return results;
        }
    }
}
