using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disney.Interfaces;
using Disney.Entities;
using Disney.Context;
using Disney.Repositories;

namespace Disney.Repositories
{
    public class MovieOrSerieRepository : BaseRepository<MovieOrSerie, DisneyContext>, IMovieOrSerieRepository
    {


        public MovieOrSerieRepository(DisneyContext dbContext) : base(dbContext)
        {

        }

        public MovieOrSerie AddMovieOrSerie(MovieOrSerie movieOrSerie)
        {
            return Add(movieOrSerie);
        }

        public List<MovieOrSerie> GetAllMoviesOrSeries()
        {
            return GetAllEntities();

        }

        public MovieOrSerie GetMovieOrSerie(int id)
        {
            return Get(id);

        }
    }
}
