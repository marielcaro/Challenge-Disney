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

namespace Disney.Controllers
{
    [ApiController]
    [Route(template: "api/[controller]")]
    public class GenderController: ControllerBase
    {
        private readonly IGenderRepository _genderRepository;
        private readonly DisneyContext _context;

        public GenderController(IGenderRepository genderRepository, DisneyContext context)
        {
            _context = context;
            _genderRepository = genderRepository;
        }

        //DEVUELVE TODOS LOS GENEROS EN UNA LISTA
        [HttpGet] //Verbo de http GET - le puedo cambiar de nombre y agregarle parámetros de ruta
        [Route(template: "genders")]
        public IActionResult Get() // Puedo agregarle parámetros para que funcionen como filtros
        {
           
            var genders = _genderRepository.GetAllGenders();
            var genderViewModel = new List<GetGenderListViewModel>();
            foreach (var gender in genders)
            {
                genderViewModel.Add(new GetGenderListViewModel
                {
                    Id = gender.Id,
                    Name = gender.Name,
                    Image = gender.Image

                }); ;
            }

            return Ok(genderViewModel);
        }

        // TODO CREATE - POST
        [HttpPost] //Verbo de http POST
        public IActionResult Post(PostGenderViewModel gender)
        {
            Gender dbGender = new Gender
            {
                Id = gender.Id,
                Image = gender.Image,
                Name = gender.Name
            };

            _context.Genders.Add(dbGender);
            _context.SaveChanges();
            return Ok();
        }

        // TODO UPDATE - PUT / PATCH
        [HttpPut] //Verbo de http PUT
        public IActionResult Put(Gender gender)
        {
            var originalGender = _genderRepository.Get(gender.Id);
            if (originalGender == null) return BadRequest(error: $"El género {gender.Id} no existe");

            originalGender.Name = gender.Name;
            originalGender.Image = gender.Image;


            _genderRepository.Update(originalGender);

            return Ok();
        }

        //// TODO DELETE
        [HttpDelete]
        public IActionResult Delete(int id) //Necesito parametros de consulta y parametros de ruta
        {
            if (_context.Genders.FirstOrDefault(x => x.Id == id) == null) return BadRequest(error: "El género enviado no existe.");

            var internalGender = _context.Genders.Find(id);
            _context.Genders.Remove(internalGender);
            _context.SaveChanges();
            return Ok();
        }

    }
}

