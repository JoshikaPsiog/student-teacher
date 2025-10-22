using StudentTeacherApp.DTOs;

namespace StudentTeacherApp.Services
{
    public interface IAuthService
    {
        Task<string?> Register(RegisterDto dto);
        Task<string?> Login(LoginDto dto);
    }
}
