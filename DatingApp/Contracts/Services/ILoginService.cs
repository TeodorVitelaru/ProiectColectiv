using DatingApp.Dtos.User.Login;

namespace DatingApp.Contracts.Services
{
    public interface ILoginService
    {
        Task<TokenDto> GetLoginToken(LoginUserRequest request);
    }
}
