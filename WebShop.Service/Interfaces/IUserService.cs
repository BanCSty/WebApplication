
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Domain.Entity;
using WebShop.Domain.Responce;
using WebShop.Domain.ViewModels.Account;

namespace WebShop.Services.Interfaces
{
    public interface IUserService
    {
        Task<IBaseResponse<LoginResponse>> AuthenticateAsync(string username, string password);
        Task<IBaseResponse<User>> RegisterAsync(RegisterViewModel model);
        Task<IBaseResponse<User>> GetUserByIdAsync(Guid userId);
        Task<IBaseResponse<User>> UpdateUserAsync(Guid id, UpdateUserModel user);
        Task<IBaseResponse<User>> GetUserByRefreshTokenAsync(string refreshToken);

    }
}
