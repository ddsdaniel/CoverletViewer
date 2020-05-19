using System;

namespace CoverletViewer.Domain.Abstractions
{
    public abstract class Indicator
    {
        public abstract int TotalLines { get; }
        public abstract int NotCoveredLines { get; }
        public abstract int CoveredLines { get; }
        public int TestableLines => CoveredLines + NotCoveredLines;
        public decimal PercentageCoverage => Math.Round((decimal)CoveredLines / TestableLines * 100, 1, MidpointRounding.AwayFromZero);
    }
}
