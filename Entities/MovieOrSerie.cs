using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Disney.Entities
{
    public class MovieOrSerie
    {
        public int Id { get; set; }
        public int Title { get; set; }
        public string Image { get; set; }
        public DateTime CreationDate { get; set; }
        public float Score { get; set; }
        public ICollection<Character> Characters { get; set; }

    }
}
