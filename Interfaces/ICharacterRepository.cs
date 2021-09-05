using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disney.Entities;

namespace Disney.Interfaces
{
    public interface ICharacterRepository : IRepository<Character>
    {
        Character AddCharacter(Character character);
        List<Character> GetAllCharacters();
        Character GetCharacter(int id);
    }
}
