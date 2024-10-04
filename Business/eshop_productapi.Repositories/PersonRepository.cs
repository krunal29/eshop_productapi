using eshop_productapi.Domain;
using eshop_productapi.Interfaces.Repositories;
using System.Linq;

namespace eshop_productapi.Repositories
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(eshop_productapiContext context) : base(context)
        {
        }

        public int GetRoleIdBaseonUserid(string id)
        {
            return Context.Person.FirstOrDefault(x => x.AspNetUserId == id).RoleId;
        }
    }
}