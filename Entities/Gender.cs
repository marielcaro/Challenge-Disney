using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Disney.Entities
{
    public class Gender
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public ICollection<MovieOrSerie> MovieOrSeries { get; set; }


    }
}
