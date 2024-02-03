using Auth.DAL;
using Auth.Entity;
using Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Services.Interfaces
{
    public interface IUserService
    {
        Task<IBaseResponse<LoginResponse>> AuthenticateAsync(string username, string password);
        Task<IBaseResponse<User>> RegisterAsync(RegisterModel model);
        Task<IBaseResponse<User>> GetUserByIdAsync(Guid userId);
        Task<IBaseResponse<User>> UpdateUserAsync(Guid id, UpdateUserModel user);
        Task<IBaseResponse<User>> GetUserByRefreshTokenAsync(string refreshToken);

    }
}
