using System;

namespace CoverletViewer.Domain.Abstractions
{
    public abstract class Indicador
    {
        public abstract int TotalLinhas { get; }
        public abstract int TotalLinhasNaoCobertas { get; }
        public abstract int TotalLinhasCobertas { get; }
        public int TotalLinhasTestaveis => TotalLinhasCobertas + TotalLinhasNaoCobertas;
        public decimal PercentualCobertura => Math.Round((decimal)TotalLinhasCobertas / TotalLinhasTestaveis * 100, 1, MidpointRounding.AwayFromZero);
    }
}
