using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Disney.Entities;

namespace Disney.Context
{
   
        public class DisneyContext : DbContext
        {
            public DisneyContext(DbContextOptions options) : base(options)
            {

            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)

            {
                base.OnModelCreating(modelBuilder);
            }

            public DbSet<Gender> Genders { get; set; } = null!;

            public DbSet<Character> Characters { get; set; } = null!;

            public DbSet<MovieOrSerie> MovieOrSeries { get; set; } = null!;


        }
    }
