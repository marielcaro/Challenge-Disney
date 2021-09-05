using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disney.Entities;

namespace Disney.Interfaces
{
    public interface IGenderRepository : IRepository<Gender>
    {
        Gender AddGender(Gender gender);
        List<Gender> GetAllGenders();
        Gender GetGender(int id);
    }
}
