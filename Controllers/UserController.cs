using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi_Coris.Models;
using WebApi_Coris.Models.Dtos;
using WebApi_Coris.Service.Abstractions;

namespace WebApi_Coris.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserInterface _userInterface;
        public UserController(IUserInterface userInterface)
        {
            _userInterface = userInterface;
        }

        [HttpGet]
        [Authorize(Policy = "ReadOnly")]
        public async Task<ActionResult<ApiResponse<List<UserDto>>>> GetUsersAsync(CancellationToken ct)
        {
            var result = await _userInterface.GetUsersAsync(ct);

            return Ok(result);
        }



        [HttpGet("{id}")]
        [Authorize(Policy = "ReadOnly")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userInterface.GetUserByIdAsync(id);

            if (!result.Sucesso)
                return NotFound(result);

            return Ok(result);
        }


        [HttpPost]
        [Authorize(Policy = "FullAccess")]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            var result = await _userInterface.CreateUserAsync(dto);

            if (!result.Sucesso || result.Dados is null)
                return BadRequest(result);

            return CreatedAtAction(
                nameof(GetUserById),
                new { id = result.Dados.id },
                result.Dados
            );
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "FullAccess")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateUserDto dto)
        {
            if (id != dto.id)
                return BadRequest("Id no caminho e no corpo não conferem.");

            var result = await _userInterface.UpdateUserAsync(dto);
            if (!result.Sucesso)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "FullAccess")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userInterface.DeleteUserAsync(id);

            if (!result.Sucesso)
                return BadRequest(result);

            return Ok(result);
        }


    }
}
