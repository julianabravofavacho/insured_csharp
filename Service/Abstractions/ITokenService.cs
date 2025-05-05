using System.Security.Claims;
using WebApi_Coris.Models;
namespace WebApi_Coris.Service.Abstractions
{
    public interface ITokenService
    {
        string GenerateToken(UserModel user);
    }
}
