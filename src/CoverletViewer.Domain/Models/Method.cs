using System.Collections.Generic;

namespace CoverletViewer.Domain.Models
{
    public class Method
    {
        public string Name { get; set; }
        public List<int> NotCoveredLines { get; set; } = new List<int>();
        public List<int> CoveredLines { get; set; } = new List<int>();
        public decimal NotCoveredLinesPercentage => (decimal)NotCoveredLines.Count / (NotCoveredLines.Count + CoveredLines.Count) * 100;

        public override string ToString()
        {
            return Name;
        }
    }
}
