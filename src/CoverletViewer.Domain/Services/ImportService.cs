using CoverletViewer.Domain.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace CoverletViewer.Domain.Services
{
    public class ImportService
    {
        public List<Project> Import(string jsonFileName)
        {
            var jsonContent = File.ReadAllText(jsonFileName);

            var jsonObject = JObject.Parse(jsonContent);
            var projects = ImportProjects(jsonObject);

            return projects;
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
    }
}
