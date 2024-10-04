using eshop_productapi.Business.ViewModels.Organization;
using eshop_productapi.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eshop_productapi.Interfaces.Services
{
    public interface IRoleModuleService : IBaseService<RoleModule>
    {
        Task<List<RoleModuleModel>> GetAllAsync();

        Task<bool> AddAsync(RoleModuleDetailsModel roleModule);

        Task<bool> UpdateAsync(RoleModuleDetailsModel roleModule);
    }
}