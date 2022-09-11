using Movie.Models;

namespace Movie.Repository.InterFace
{
    public interface IAuthInterface
    {
        Task<AuthDto> RegisterAsync(RegisterDto model);
        Task<AuthDto> GetTokenasync(TkoenRequestDto model);
        Task<string> AddRoleAsync(AddRoleDto model);

    }
}
