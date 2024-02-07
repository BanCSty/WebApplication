
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebShop.DAL;
using WebShop.Domain.Entity;
using WebShop.Domain.Enum;
using WebShop.Domain.Responce;
using WebShop.Domain.ViewModels.Account;
using WebShop.Service.Helpers;
using WebShop.Services.Interfaces;
using Role = WebShop.Domain.Enum.Role;

namespace WebShop.Services.Implentations
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IOptions<JwtSettings> _jwtSettings;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserService> _logger;

        public UserService(AppDbContext context, ITokenService tokenService, IOptions<JwtSettings> tokenSettings, ILogger<UserService> logger)
        {
            _context = context;
            _tokenService = tokenService;
            _jwtSettings = tokenSettings;
            _logger = logger;
        }

        public async Task<IBaseResponse<LoginResponse>> AuthenticateAsync(string username, string password)
        {
            try
            {
                var hashPass = HashPasswordHelper.HashPassowrd(password);
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Login == username  && x.Password == hashPass);

                if (user == null)
                    return new BaseResponse<LoginResponse>()
                    {
                        StatusCode = StatusCode.BadRequest,
                        DescriptionError = "Incorrect username or password"
                    };

                var genJwtTokenModel = new GetJwtTokenModel() { Id = user.Id, Login = user.Login, Role = user.Role == "Admin" ? Role.Admin : Role.User};
                // Генерация JWT Access токена
                var accessToken = _tokenService.GenerateJwtToken(genJwtTokenModel).Result;

                // Генерация Refresh токена
                var refreshToken = _tokenService.GenerateRefreshToken().Result;

                //Удаление старого ResreshToken из БД
                var oldToken = await _context.RefreshTokens.Where(x => x.UserId == user.Id).ToListAsync();
                _context.RefreshTokens.RemoveRange(oldToken);
                await _context.SaveChangesAsync();



                //Сохранение токена в БД
                var refreshTokenRecord = new RefreshToken { UserId = user.Id, TokenRefresh = refreshToken, ExpirationDate = DateTime.UtcNow.AddDays(7) };
                await _context.RefreshTokens.AddAsync(refreshTokenRecord);
                await _context.SaveChangesAsync();

                return new BaseResponse<LoginResponse>()
                {
                    StatusCode = StatusCode.OK,
                    Data = new LoginResponse() {
                        JwtToken = accessToken,
                        RefreshToken = refreshToken,
                        Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.Value.ExpirationMinutes)
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during authentication");
                return new BaseResponse<LoginResponse>()
                {
                    Data = null,
                    DescriptionError = $"[Authenticate] : {ex}",
                    StatusCode = StatusCode.InternalServer
                };
            }          
        }

        public async Task<IBaseResponse<User>> GetUserByIdAsync(Guid userId)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);

                if (user == null)
                    return new BaseResponse<User>()
                    {
                        StatusCode = StatusCode.InternalServer,
                        DescriptionError = "User not found"
                    };

                return new BaseResponse<User>()
                {
                    StatusCode = StatusCode.OK,
                    Data = user
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    DescriptionError = $"[GetUserById] : {ex.Message}",
                    StatusCode = StatusCode.InternalServer
                };
            }
        }

        public async Task<IBaseResponse<User>> GetUserByRefreshTokenAsync(string refreshToken)
        {
            try
            {
                // Поиск записи Refresh токена в базе данных
                var refreshTokenRecord = await _context.RefreshTokens.SingleOrDefaultAsync(x => x.TokenRefresh == refreshToken);

                if (refreshTokenRecord == null || refreshTokenRecord.ExpirationDate < DateTime.UtcNow)
                    return new BaseResponse<User>()
                    {
                        DescriptionError = "Token not found",
                        StatusCode = StatusCode.TokenNotFound
                    };

                // Получить информацию о пользователе, связанную с найденным Refresh токеном
                var user = await _context.Users.FirstOrDefaultAsync(x=> x.Id == refreshTokenRecord.UserId);

                return new BaseResponse<User>()
                {
                    StatusCode = StatusCode.OK,
                    Data = user
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    DescriptionError = $"[GetUserByRefreshToken] : {ex.Message}",
                    StatusCode = StatusCode.InternalServer
                };
            }
        }

        public async Task<IBaseResponse<User>> RegisterAsync(RegisterViewModel model)
        {
            if (model == null || model.Password != model.PasswordConfirm)
                return new BaseResponse<User>()
                {
                    DescriptionError = "Password mismatch",
                    StatusCode = StatusCode.InternalServer
                };
            try
            {
                // Проверка, не существует ли уже пользователя с таким же именем
                var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Login == model.Login);
                if (existingUser != null)
                    return new BaseResponse<User>()
                    {
                        DescriptionError = "User with this login already exists",
                        StatusCode = StatusCode.InternalServer
                    };

                // Создание нового пользователя
                var newUser = new User
                {
                    Login = model.Login,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = HashPasswordHelper.HashPassowrd(model.Password),
                    Role = "User"
                };

                // Cоздания нового пользователя
                var result = await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                return new BaseResponse<User>()
                {
                    StatusCode = StatusCode.OK,
                    Data = newUser
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    DescriptionError = $"[Register] : {ex.Message}",
                    StatusCode = StatusCode.InternalServer
                };
            }
        }

        public async Task<IBaseResponse<User>> UpdateUserAsync(Guid id,UpdateUserModel model)
        {
            if (id == Guid.Empty || model == null)
                return new BaseResponse<User>()
                { 
                    StatusCode = StatusCode.BadRequest,
                    DescriptionError = "No data to change"
                };

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                    return new BaseResponse<User>()
                    {
                        StatusCode = StatusCode.BadRequest,
                        DescriptionError = "Id not found"
                    };

                var updateUser = new User()
                {
                    Id = user.Id,
                    Email = model.Email == null ? user.Email : model.Email,
                    Login = model.Login == null ? user.Login : model.Login,
                    FirstName = model.FirstName == null ? user.FirstName : model.FirstName,
                    LastName = model.LastName == null ? user.LastName : model.LastName,
                    Password = model.Password == null ? user.Password : model.Password
                };

                _context.Users.Update(updateUser);
                await _context.SaveChangesAsync();

                return new BaseResponse<User>()
                {
                    StatusCode = StatusCode.OK,
                    Data = updateUser
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    DescriptionError = $"[Update] : {ex.Message}",
                    StatusCode = StatusCode.InternalServer
                };
            }
        }
    }
}
