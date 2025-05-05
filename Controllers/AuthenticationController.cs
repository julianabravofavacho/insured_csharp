using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi_Coris.Models.Dtos;
using WebApi_Coris.Service.AuthenticationService;

namespace WebApi_Coris.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class AuthenticationController : ControllerBase
        {
            private readonly IAuthenticationService _auth;
            public AuthenticationController(IAuthenticationService auth) => _auth = auth;

            [HttpPost("login")]
            [AllowAnonymous]
            public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto dto)
            {
                var res = await _auth.LoginAsync(dto);
                return Ok(res);
            }
        [HttpPost("logout")]
        [Authorize(Policy = "ReadOnly")]
        public async Task<IActionResult> Logout()
        {
            // 1) Extrai o header Authorization de forma segura
            if (!Request.Headers.TryGetValue("Authorization", out var authHeaderValues))
                return Unauthorized(new { message = "Authorization header missing" });

            var authHeader = authHeaderValues.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(authHeader))
                return Unauthorized(new { message = "Authorization header missing" });

            // 2) Separa esquema e token, ignorando casing
            var parts = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2 || !parts[0].Equals("Bearer", StringComparison.OrdinalIgnoreCase))
                return Unauthorized(new { message = "Authorization header malformed" });

            var token = parts[1].Trim();

            // 3) Chama o serviço de logout para remover o token do banco
            await _auth.LogoutAsync(token);
            return NoContent();
        }
}
}
