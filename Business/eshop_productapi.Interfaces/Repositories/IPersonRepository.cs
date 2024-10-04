using eshop_productapi.Domain;
using eshop_productapi.Interfaces.Repository;

namespace eshop_productapi.Interfaces.Repositories
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        int GetRoleIdBaseonUserid(string id);
    }
}