using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Disney.ViewModels
{
    public class GenderViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public List<MoviesViewModel> MovieOrSeries { get; set; }
    }
}
