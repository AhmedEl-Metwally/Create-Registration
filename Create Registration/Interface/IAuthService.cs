using Create_Registration.Modles;

namespace Create_Registration.Interface
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> GetTokenAsync(TokenRegisterModel model);
        Task<string> AddRolesAsync(AddRolesModel model);
    }
}
