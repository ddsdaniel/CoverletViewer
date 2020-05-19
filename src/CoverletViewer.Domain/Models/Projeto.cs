using System.Collections.Generic;
using System.Linq;
using CoverletViewer.Domain.Abstractions;

namespace CoverletViewer.Domain.Models
{
    public class Projeto : Indicador
    {
        public string Nome { get; private set; }
        public List<Arquivo> Arquivos { get; set; } = new List<Arquivo>();

        public override int TotalLinhas => Arquivos.Sum(a => a.TotalLinhas);

        public override int TotalLinhasNaoCobertas => Arquivos.Sum(a => a.TotalLinhasNaoCobertas);

        public override int TotalLinhasCobertas => Arquivos.Sum(a => a.TotalLinhasCobertas);

        public Projeto(string nome)
        {
            Nome = nome;            
        }
    }
}
