using AutoMapper;
using eshop_productapi.Business.ViewModels.Organization;
using eshop_productapi.Domain.Models;
using eshop_productapi.Interfaces.Services;
using eshop_productapi.UoW;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_productapi.Services
{
    public class RoleModuleService : ServiceBase, IRoleModuleService
    {
        public RoleModuleService(IUnitOfWork unitOfWork, IMapper _mapper) : base(unitOfWork, _mapper)
        {
        }

        public async Task<List<RoleModuleModel>> GetAllAsync()
        {
            var getRoleModule = await unitOfWork.RoleModuleRepository.GetAllAsync();
            List<RoleModuleModel> model = new List<RoleModuleModel>();
            model = getRoleModule.GroupBy(x => x.RoleId, (key, g) => new { RoleId = key, ModuleDetails = g.ToList() }).Select(x => new RoleModuleModel
            {
                RoleId = x.RoleId,
                RoleModuleDetails = mapper.Map<List<RoleModuleDetailsModel>>(x.ModuleDetails.ToList())
            }).ToList();
            return model;
        }

        public async Task<bool> AddAsync(RoleModuleDetailsModel roleModuleModel)
        {
            var roleModule = mapper.Map<RoleModule>(roleModuleModel);
            await unitOfWork.RoleModuleRepository.AddAsync(roleModule);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync(RoleModuleDetailsModel roleModuleModel)
        {
            var roleModule = await unitOfWork.RoleModuleRepository.GetAsync(roleModuleModel.Id);
            if (roleModule != null)
            {
                roleModule = mapper.Map(roleModuleModel, roleModule);
                roleModule.Id = roleModuleModel.Id;
                await unitOfWork.RoleModuleRepository.UpdateAsync(roleModule);
            }
            return await Task.FromResult(true);
        }
    }
}