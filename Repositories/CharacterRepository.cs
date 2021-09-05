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
    public class CharacterRepository : BaseRepository<Character, DisneyContext>, ICharacterRepository
    {

        public CharacterRepository(DisneyContext dbContext) : base(dbContext)
        {

        }

        public Character AddCharacter(Character character)
        {
            return Add(character);
        }

        public List<Character> GetAllCharacters()
        {
            return GetAllEntities();
        }

        public Character GetCharacter(int id)
        {
            return Get(id);
        }
    }
}
