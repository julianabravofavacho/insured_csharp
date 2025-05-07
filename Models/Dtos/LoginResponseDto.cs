namespace WebApi_Coris.Models.Dtos
{
    public record LoginResponseDto(string Token, DateTime ExpiresAt, List<string> Abilities);
}
