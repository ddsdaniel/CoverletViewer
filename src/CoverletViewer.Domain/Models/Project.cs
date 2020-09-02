using System.Collections.Generic;
using System.Linq;
using CoverletViewer.Domain.Abstractions;

namespace CoverletViewer.Domain.Models
{
    public class Project : Indicator
    {
        public string Name { get; private set; }
        public List<CodeFile> Files { get; set; } = new List<CodeFile>();

        public override int TotalLines => Files.Sum(a => a.TotalLines);

        public override int NotCoveredLines => Files.Sum(a => a.NotCoveredLines);

        public override int CoveredLines => Files.Sum(a => a.CoveredLines);

        public Project(string name)
        {
            Name = name;            
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
