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

            foreach (var project in _projects)
            {
                foreach (var file in project.Files)
                {
                    var parts = file.Path.Split('\\');
                    var tabs = "";

                    for (int i = 0; i < parts.Length; i++)
                    {
                        var part = parts[i];
                        tabs += "  ";
                        var resultName = $"{tabs}{part}";

                        var result = results.FirstOrDefault(r => r.Name == resultName);

                        if (result == null)
                        {
                            if (i == parts.Length -1)
                                result = new Result(resultName, file);
                            else
                                result = new Result(resultName);
                            results.Add(result);
                        }
                        result.Increment(file);
                    }
                }


            }

            return results;
        }
    }
}
