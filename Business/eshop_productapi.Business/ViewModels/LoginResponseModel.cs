using eshop_productapi.Business.ViewModels.Organization;
using eshop_productapi.Domain.Models;
using System.Collections.Generic;

namespace eshop_productapi.Business.ViewModels
{
    public class LoginResponseModel
    {
        public ApplicationUser ApplicationUser { get; set; }

        public string Token { get; set; }

        public List<RoleModuleModel> roleModuleModel { get; set; }
    }
}