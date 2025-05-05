namespace WebApi_Coris.Models.Dtos
{
    public record CreateUserDto(
        string name,
        string email,
        string password_hash,
        bool is_active = true,
        bool is_adm = false
    );
}
