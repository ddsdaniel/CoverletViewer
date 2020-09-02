using System.Collections.Generic;

namespace CoverletViewer.Domain.Models
{
    public class Class
    {
        public string Name { get; set; }
        public List<Method> Methods { get; set; } = new List<Method>();

        public override string ToString()
        {
            return Name;
        }

    }
}
