using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoverletViewer.Domain.Abstractions;

namespace CoverletViewer.Domain.Models
{
    public class CodeFile : Indicator
    {
        public string Path { get; set; }
        public List<Class> Classes { get; set; } = new List<Class>();

        public override int TotalLines => File.ReadAllLines(Path).Length;

        public override int NotCoveredLines => Classes.Sum(c => c.Methods.Sum(m => m.NotCoveredLines.Count));

        public override int CoveredLines => Classes.Sum(c => c.Methods.Sum(m => m.CoveredLines.Count));
    }
}
