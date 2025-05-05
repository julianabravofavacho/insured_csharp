using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Intuit.Ipp.Data;
using Intuit.Ipp.OAuth2PlatformClient;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApi_Coris.Models.Dtos
{
    public record UserDto(
    int id,
    string name,
    string email,
    string password_hash,
    bool? is_active = true,
    bool? is_adm = false,
    DateTime created_at = default,
    DateTime updated_at = default
)
    {
        public static UserDto FromModel(UserModel m)
            => new UserDto(
                m.id, m.name, m.email, m.password_hash,
                m.is_active, m.is_adm, m.created_at,
                m.updated_at
            );
    }
}
