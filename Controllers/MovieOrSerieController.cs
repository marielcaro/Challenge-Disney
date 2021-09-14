using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Disney.Context;
using Disney.Entities;
using Disney.Repositories;
using Disney.Interfaces;
using Disney.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Disney.Controllers
{
    [ApiController]
    [Route(template: "api/[controller]")]
    [Authorize]
    public class MovieOrSerieController : ControllerBase
    {
        private readonly IMovieOrSerieRepository _movieRepository;
        private readonly DisneyContext _context;

        public MovieOrSerieController(IMovieOrSerieRepository movieRepository, DisneyContext context)
        {
            _context = context;
            _movieRepository = movieRepository;
        }

        //LISTADO DE TODAS LAS PELÍCULAS
        [HttpGet] //Verbo de http GET 
        [Route(template: "moviesAndSeriesList")]
        public IActionResult Get(string Order) // Puedo agregarle parámetros para que funcionen como filtros
        {
            //El parámetro ORDER sirve para ordenar el listado de forma Ascendente(ASC) o Descendente(DESC)
     
            var movies = _movieRepository.GetAllMoviesOrSeries();
            var movieViewModel = new List<GetMovieListViewModel>();
            foreach (var movie in movies)
            {
                movieViewModel.Add(new GetMovieListViewModel
                {
                    Id=movie.Id,
                    Title = movie.Title,
                    Image = movie.Image,
                    CreationDate= movie.CreationDate
                }); ;
            }

            switch (Order.ToUpper())
            {
                case "ASC":
                    return Ok(movieViewModel.OrderBy(x => x.Id).ToList()); //ORDEN ASCENDENTE
                    
                case "DESC":
                    return Ok(movieViewModel.OrderByDescending(x => x.Id).ToList()); // ORDEN DESCENDENTE
                    
                default:
                    return Ok(movieViewModel); // SIN PREFERENCIAS DE ORDEN
                    
            }
                      
        }

        // DEVUELVE UNA PELÍCULA O SERIE EN PARTICULAR
        [HttpGet] //Verbo de http GET 
        [Route(template: "DetailedMovieSerie")]
        public IActionResult Get(int id, string title) // Puedo agregarle parámetros para que funcionen como filtros
        {
            //Puedo filtrar por id o título de película, y me devuelve todos los personajes más info detallada de la película

            var movies = _context.MovieOrSeries.Include(x => x.Characters).ToList();
            var movieViewModel = new List<DetailMovieViewModel>();
            if (id != 0)
            {
                movies = movies.Where(x => x.Id == id).ToList(); 

            }
            if (!string.IsNullOrEmpty(title))
            {
                movies = movies.Where(x => x.Title == title).ToList();
              
            }


            foreach (var movie in movies)
            {
                movieViewModel.Add(new DetailMovieViewModel
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Image = movie.Image,
                    CreationDate = movie.CreationDate,
                    Score = movie.Score,
                    Characters = movie.Characters.Any() ? movie.Characters.Select(x => new CharactersViewModel
                    {
                        Name = x.Name,
                        Age = x.Age
                    }).ToList() : null
                });
            }

            if (!movies.Any()) return BadRequest(error: $"Esta película/serie {id} no existe");

            return Ok(movieViewModel);
        }

        //DEVUELVE TODAS LAS PELÍCULAS QUE PERTENECEN A DETERMINADO GÉNERO
        [HttpGet] //Verbo de http GET 
        [Route(template: "GenderMovie")]
        public IActionResult GetMovies(int idGender, string NameGender) // Puedo agregarle parámetros para que funcionen como filtros
        {
           //PUEDO FILTRAR LAS PELÍCULAS POR ID DE GENERO O NOMBRE DE GÉNERO, Y DEVUELVEN TODAS LAS PELICULAS DE DICHO GÉNERO

            var genders = _context.Genders.Include(x => x.MovieOrSeries).ToList();
            var genderViewModel = new List<GenderViewModel>();
            if (idGender > 0)
            {
                genders = genders.Where(x => x.Id == idGender).ToList(); //esto sirve para el filtrado

            }
            if (!string.IsNullOrEmpty(NameGender))
            {
                genders = genders.Where(x => x.Name == NameGender).ToList(); //esto sirve para el filtrado

            }


            foreach (var gender in genders)
            {
                genderViewModel.Add(new GenderViewModel
                {
                    Id = gender.Id,
                    Name = gender.Name,
                    MovieOrSeries = gender.MovieOrSeries.Any() ? gender.MovieOrSeries.Select(x => new MoviesViewModel
                    {
                        Title = x.Title,
                        Image = x.Image
                    }).ToList() : null
                });
            }

            if (!genders.Any()) return BadRequest(error: $"El personaje {idGender} no existe");

            return Ok(genderViewModel);
        }

        // TODO CREATE - POST
        [HttpPost] //Verbo de http POST
        public IActionResult Post(PostMovieViewModel movie)
        {
            MovieOrSerie dbMovie = new MovieOrSerie
            {
                Image = movie.Image,
                Title = movie.Title,
                CreationDate=movie.CreationDate
            };

            _context.MovieOrSeries.Add(dbMovie);
            _context.SaveChanges();
            return Ok();
        }

        // TODO UPDATE - PUT / PATCH
        [HttpPut] //Verbo de http PUT
        public IActionResult Put(MovieOrSerie movie)
        {
            var originalMovie = _movieRepository.Get(movie.Id);
            if (originalMovie == null) return BadRequest(error: $"La película/serie {movie.Id} no existe");

            originalMovie.Title = movie.Title;
            originalMovie.Image = movie.Image;


            _movieRepository.Update(originalMovie);

            return Ok();
        }

        //// TODO DELETE
        [HttpDelete]
        public IActionResult Delete(int id) //Necesito parametros de consulta y parametros de ruta
        {
            if (_context.MovieOrSeries.FirstOrDefault(x => x.Id == id) == null) return BadRequest(error: "La película/serie enviado no existe.");

            var internalMovie = _context.MovieOrSeries.Find(id);
            _context.MovieOrSeries.Remove(internalMovie);
            _context.SaveChanges();
            return Ok();
        }
    }
}
