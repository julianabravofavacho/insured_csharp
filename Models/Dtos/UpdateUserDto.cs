namespace WebApi_Coris.Models.Dtos
{
    public record UpdateUserDto(
        int id,
        string? name = null,
        string? email = null,
        string? password_hash = null,
        bool is_active = true,
        bool is_adm = false
    );
}
