using System.Collections.Generic;

namespace CoverletViewer.Domain.Models
{
    public class Classe
    {
        public string Nome { get; set; }
        public List<Metodo> Metodos { get; set; } = new List<Metodo>();

    }
}
