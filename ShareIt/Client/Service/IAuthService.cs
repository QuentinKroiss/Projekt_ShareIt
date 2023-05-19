using ShareIt.Shared;

namespace ShareIt.Client.Service
{
    public interface IAuthService
    {
        Task<RegisterResult> Register(RegisterModel registerModel);
        Task<LoginResult> Login(LoginModel loginModel);
        Task<DeleteUserResult> Delete(DeleteUserModel deleteUserModel);
        Task Logout();
    }
}
