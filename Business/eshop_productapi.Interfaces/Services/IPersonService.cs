using eshop_productapi.Business.ViewModels;
using eshop_productapi.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eshop_productapi.Interfaces.Services
{
    public interface IPersonService : IBaseService<Person>
    {
        Task<List<PersonModel>> GetAllAsync();

        Task<PersonModel> GetAsync(int id);

        Task<bool> AddAsync(Person model);

        Task<bool> UpdateAsync(Person model);

        Task<bool> DeleteAsync(int id);

        Task<string> ResetPassword(ChangePassword model);

        Task<bool> ResetForgotPassword(string aspNetUserId, Guid resetId);

        Task<string> ForgotPassword(ResetPasswordViewModel model);

        bool IsResetIdExist(Guid resetId);

        int GetRoleIdBaseonUserid(string id);
    }
}