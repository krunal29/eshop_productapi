using eshop_productapi.Domain;
using eshop_productapi.Domain.Models;
using eshop_productapi.Interfaces.Repositories;

namespace eshop_productapi.Repositories
{
    public class AccessModuleRepository : BaseRepository<AccessModule>, IAccessModuleRepository
    {
        public AccessModuleRepository(eshop_productapiContext context) : base(context)
        {
        }
    }
}