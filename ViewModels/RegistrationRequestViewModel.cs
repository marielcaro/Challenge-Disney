﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Disney.ViewModels
{
    public class RegistrationRequestViewModel
    {
        [Required]
        [MinLength(6)]

        public string Username { get; set; }

        [Required]
        [EmailAddress]

        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        
        public string Password { get; set; }




    }
}
