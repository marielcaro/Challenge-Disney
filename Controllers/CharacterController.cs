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
    public class CharacterController: ControllerBase
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly DisneyContext _context;

        public CharacterController(ICharacterRepository characterRepository, DisneyContext context)
        {
            _context = context;
            _characterRepository = characterRepository;
        }

        //DEVUELVE TODOS LOS PERSONAJES EN UN LISTADO
        [HttpGet] //Verbo de http GET 
        [Route(template: "characters")]
        public IActionResult Get() // Puedo agregarle parámetros para que funcionen como filtros
        {
         
            var characters = _characterRepository.GetAllCharacters();
            var characterViewModel = new List<GetCharactersListViewModel>();
            foreach (var character in characters)
            {
                characterViewModel.Add(new GetCharactersListViewModel
                {
                    Name = character.Name,
                    Image = character.Image
                  
                }); ;
            }

            return Ok(characterViewModel);
        }

        // DEVUELVE INFO DETALLADA DE UN PERSONAJE EN PARTICULAR
        [HttpGet] //Verbo de http GET 
        [Route(template: "DetailedCharacters")]
        public IActionResult Get(int id, string name, int age) // Puedo agregarle parámetros para que funcionen como filtros
        {
            //filtra el personaje por nombre, id o edad. - ´También devuelve todas las películas en las que participó ese personaje
          
            var characters = _context.Characters.Include(x => x.MovieOrSeries).ToList();
            var characterViewModel = new List<DetailCharacterViewModel>();
            if (id != 0) // FILTRO POR ID
            {
                characters = characters.Where(x => x.Id == id).ToList(); //esto sirve para el filtrado

                foreach (var character in characters)
                {
                    characterViewModel.Add(new DetailCharacterViewModel
                    { 
                        Id= character.Id,
                        Name = character.Name,
                        Image = character.Image,
                        Age= character.Age,
                        Weight=character.Weight,
                        History=character.History,
                        MoviesSeries=character.MovieOrSeries.Any() ? character.MovieOrSeries.Select(x => new MoviesViewModel
                        {
                            Title= x.Title,
                            Image=x.Image
                        }).ToList() : null
                    }); 
                }

            }
            if (!string.IsNullOrEmpty(name)) // FILTRO POR NOMBRE
            {
                characters = characters.Where(x => x.Name == name).ToList();
                foreach (var character in characters)
                {
                    characterViewModel.Add(new DetailCharacterViewModel
                    {
                        Id = character.Id,
                        Name = character.Name,
                        Image = character.Image,
                        Age = character.Age,
                        Weight = character.Weight,
                        History = character.History,
                        MoviesSeries = character.MovieOrSeries.Any() ? character.MovieOrSeries.Select(x => new MoviesViewModel
                        {
                            Title = x.Title,
                            Image = x.Image
                        }).ToList() : null
                    });
                }
            }
            if (age>0) //FILTRO POR EDAD
            {
                characters = characters.Where(x => x.Age == age).ToList();
                foreach (var character in characters)
                {
                    characterViewModel.Add(new DetailCharacterViewModel
                    {
                        Id = character.Id,
                        Name = character.Name,
                        Image = character.Image,
                        Age = character.Age,
                        Weight = character.Weight,
                        History = character.History,
                        MoviesSeries = character.MovieOrSeries.Any() ? character.MovieOrSeries.Select(x => new MoviesViewModel
                        {
                            Title = x.Title,
                            Image = x.Image
                        }).ToList() : null
                    });
                }
            }

         

            if (!characters.Any()) return BadRequest(error: $"El personaje {id} no existe");

            return Ok(characterViewModel);
        }

        //DEVUELVE TODOS LOS PERSONAJES DE UNA PELÍCULA MÁS INFO DETALLADA DE ESA PELÍCULA
        [HttpGet] //Verbo de http GET 
        [Route(template: "MovieCharacters")]
        public IActionResult GetCharactersByMovie(int idMovie, string TitleMovie) // Puedo agregarle parámetros para que funcionen como filtros
        {
            //Puede buscar por id de película o título de peícula

            var videos = _context.MovieOrSeries.Include(x => x.Characters).ToList();
            var videoViewModel = new List<DetailMovieViewModel>();
            if (idMovie > 0) // ID DE PELÍCULA
            {
                videos = videos.Where(x=>x.Id==idMovie).ToList(); //esto sirve para el filtrado



                foreach (var video in videos)
                {
                    videoViewModel.Add(new DetailMovieViewModel
                    {
                        Id = video.Id,
                        Title = video.Title,
                        Characters = video.Characters.Any() ? video.Characters.Select(x => new CharactersViewModel
                        {
                            Name = x.Name,
                            Age = x.Age
                        }).ToList() : null
                    });
                }

            }

       
            if (!string.IsNullOrEmpty(TitleMovie)) // TÍTULO DE PELÍCULA
            {
                videos = videos.Where(x => x.Title == TitleMovie).ToList(); //esto sirve para el filtrado



                foreach (var video in videos)
                {
                    videoViewModel.Add(new DetailMovieViewModel
                    {
                        Id = video.Id,
                        Title = video.Title,
                        Characters = video.Characters.Any() ? video.Characters.Select(x => new CharactersViewModel
                        {
                            Name = x.Name,
                            Age = x.Age
                        }).ToList() : null
                    });
                }

            }



            if (!videos.Any()) return BadRequest(error: $"El personaje {idMovie} no existe");

            return Ok(videoViewModel);
        }


        // TODO CREATE - POST
        [HttpPost] //Verbo de http POST
        public IActionResult Post(PostCharacterViewModel character)
        {
            Character dbCharacter = new Character
            {
                Image = character.Image,
                Name = character.Name
            };

            _context.Characters.Add(dbCharacter);
            _context.SaveChanges();
            return Ok();
        }

        // TODO UPDATE - PUT / PATCH
        [HttpPut] //Verbo de http PUT
        public IActionResult Put(Character character)
        {
            var originalCharacter = _characterRepository.Get(character.Id);
            if (originalCharacter == null) return BadRequest(error: $"El personaje {character.Id} no existe");

            originalCharacter.Name = character.Name;
            originalCharacter.Image = character.Image;


            _characterRepository.Update(originalCharacter);

            return Ok();
        }

        //// TODO DELETE
        [HttpDelete]
        public IActionResult Delete(int id) //Necesito parametros de consulta y parametros de ruta
        {
            if (_context.Characters.FirstOrDefault(x => x.Id == id) == null) return BadRequest(error: "El personaje enviado no existe.");

            var internalCharacter = _context.Characters.Find(id);
            _context.Characters.Remove(internalCharacter);
            _context.SaveChanges();
            return Ok();
        }

    }
}
