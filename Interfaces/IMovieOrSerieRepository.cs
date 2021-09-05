using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disney.Entities;

namespace Disney.Interfaces
{
    public interface IMovieOrSerieRepository : IRepository<MovieOrSerie>
    {
        MovieOrSerie AddMovieOrSerie(MovieOrSerie movieOrSerie);
        List<MovieOrSerie> GetAllMoviesOrSeries();
        MovieOrSerie GetMovieOrSerie(int id);
    }
}
