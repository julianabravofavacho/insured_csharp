using Intuit.Ipp.Data;
using WebApi_Coris.Models;
using WebApi_Coris.Models.Dtos;

namespace WebApi_Coris.Service.Abstractions
{
    public interface IUserInterface
    {
        Task<ApiResponse<List<UserDto>>> GetUsersAsync(CancellationToken ct = default);
        Task<ApiResponse<UserDto>> GetUserByIdAsync(int id);
        Task<ApiResponse<bool>> DeleteUserAsync(int id);
        Task<ApiResponse<UserDto>> UpdateUserAsync(UpdateUserDto dto);
        Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserDto dto);
        Task<UserModel?> GetByEmailAsync(string email);
    }
}
