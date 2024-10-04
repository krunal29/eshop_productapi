using eshop_productapi.Domain;
using eshop_productapi.Domain.Models;
using eshop_productapi.Interfaces.Repositories;

namespace eshop_productapi.Repositories
{
    public class RoleModuleRepository : BaseRepository<RoleModule>, IRoleModuleRepository
    {
        public RoleModuleRepository(eshop_productapiContext context) : base(context)
        {
        }
    }
}