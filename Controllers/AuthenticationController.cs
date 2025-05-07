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
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            try
            {
                var loginResult = await _auth.LoginAsync(dto);
                return Ok(new
                {
                    sucesso = true,
                    dados = loginResult,
                    mensagem = string.Empty
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(new
                {
                    sucesso = false,
                    dados = (object?)null,
                    mensagem = ex.Message 
                });
            }
        }

        [HttpDelete("logout")]
        [Authorize(Policy = "ReadOnly")]
        public async Task<IActionResult> Logout()
        {
            var header = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(header)
                || !header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { success = false, message = "Invalid Authorization header" });
            }

            var token = header["Bearer ".Length..].Trim();

            try
            {
                await _auth.LogoutAsync(token);
                return Ok(new { success = true, message = "Logout efetuado com sucesso" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { success = false, message = "Token não encontrado" });
            }
        }
    }
}
