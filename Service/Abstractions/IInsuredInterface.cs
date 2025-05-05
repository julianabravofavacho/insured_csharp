using WebApi_Coris.Models;
using WebApi_Coris.Models.Dtos;

namespace WebApi_Coris.Service.Abstractions
{
    public interface IInsuredInterface
    {
        Task<ApiResponse<List<InsuredDto>>> GetInsuredsAsync(CancellationToken ct = default);
        Task<ApiResponse<InsuredDto>> GetInsuredByIdAsync(int id);
        Task<ApiResponse<bool>> DeleteInsuredAsync(int id);
        Task<ApiResponse<InsuredDto>> UpdateInsuredAsync(UpdateInsuredDto dto);
        Task<ApiResponse<InsuredDto>> CreateInsuredAsync(CreateInsuredDto dto);
    }
}
