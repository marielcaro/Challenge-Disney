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
    public class GenderRepository : BaseRepository<Gender, DisneyContext>, IGenderRepository
    {

        public GenderRepository(DisneyContext dbContext) : base(dbContext)
        {

        }

        public Gender AddGender(Gender gender)
        {
            return Add(gender);

        }

        public List<Gender> GetAllGenders()
        {
            return GetAllEntities();
        }

        public Gender GetGender(int id)
        {
            return Get(id);
        }
    }
}
