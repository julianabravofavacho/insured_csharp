using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi_Coris.DataContext;
using WebApi_Coris.Models;
using WebApi_Coris.Models.Dtos;

namespace WebApi_Coris.Service.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task LogoutAsync(string token);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<UserModel> _hasher;
        private readonly IConfiguration _config;

        public AuthenticationService(
            ApplicationDbContext context,
            IPasswordHasher<UserModel> hasher,
            IConfiguration config)
        {
            _context = context;
            _hasher = hasher;
            _config = config;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto req)
        {

            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.email == req.Email && u.is_active)
                ?? throw new UnauthorizedAccessException("Usuário ou senha inválidos");

            var verify = _hasher.VerifyHashedPassword(user, user.password_hash, req.Password);
            if (verify != PasswordVerificationResult.Success)
                throw new UnauthorizedAccessException("Usuário ou senha inválidos");

            var secret = _config.GetValue<string>("Jwt:Secret")!;
            var minutes = _config.GetValue<int>("Jwt:TokenValidityMinutes", 60);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiresAt = DateTime.Now.AddMinutes(minutes);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.email)
            };
            var jwtToken = new JwtSecurityToken(
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var abilitiesObj = user.is_adm
                ? new List<string> { "*" }
                : new List<string> { "show" };

            var abilitiesJson = JsonSerializer.Serialize(abilitiesObj);

            var pat = new PersonalAccessTokenModel
            {
                tokenable_type = nameof(UserModel),
                tokenable_id = user.id,
                name = "default",
                token = jwt,
                abilities = abilitiesJson,
                expires_at = expiresAt,
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };
            _context.PersonalAccessTokens.Add(pat);
            await _context.SaveChangesAsync();

            return new LoginResponseDto(jwt, expiresAt, abilitiesObj);
        }
        public async Task LogoutAsync(string token)
        {
            var pat = await _context.PersonalAccessTokens
                .SingleOrDefaultAsync(p => p.token == token);
            if (pat != null)
            {
                _context.PersonalAccessTokens.Remove(pat);
                await _context.SaveChangesAsync();
            }
        }
    }
}
