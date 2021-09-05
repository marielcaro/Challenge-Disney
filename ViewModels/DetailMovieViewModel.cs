using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Disney.ViewModels
{
    public class DetailMovieViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public DateTime CreationDate { get; set; }
        public float Score { get; set; }
        public List<CharactersViewModel> Characters { get; set; }
    }
}
