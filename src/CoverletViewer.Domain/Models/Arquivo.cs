using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoverletViewer.Domain.Abstractions;

namespace CoverletViewer.Domain.Models
{
    public class Arquivo : Indicador
    {
        public string Caminho { get; set; }
        public List<Classe> Classes { get; set; } = new List<Classe>();

        public override int TotalLinhas => File.ReadAllLines(Caminho).Length;

        public override int TotalLinhasNaoCobertas => Classes.Sum(c => c.Metodos.Sum(m => m.LinhasNaoCobertas.Count));

        public override int TotalLinhasCobertas => Classes.Sum(c => c.Metodos.Sum(m => m.LinhasCobertas.Count));
    }
}
