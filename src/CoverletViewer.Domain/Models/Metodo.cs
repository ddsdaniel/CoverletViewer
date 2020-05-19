using System.Collections.Generic;

namespace CoverletViewer.Domain.Models
{
    public class Metodo
    {
        public string Nome { get; set; }
        public List<int> LinhasNaoCobertas { get; set; } = new List<int>();
        public List<int> LinhasCobertas { get; set; } = new List<int>();
    }
}
