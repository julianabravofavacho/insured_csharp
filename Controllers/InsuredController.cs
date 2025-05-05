using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_Coris.Models;
using WebApi_Coris.Models.Dtos;
using WebApi_Coris.Service.Abstractions;
using WebApi_Coris.Service.InsuredService;
using WebApi_Coris.Service.UserService;

namespace WebApi_Coris.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuredController : ControllerBase
    {
        private readonly IInsuredInterface _insuredInterface;
        public InsuredController(IInsuredInterface insuredInterface)
        {
            _insuredInterface = insuredInterface;
        }

        [HttpGet]
        [Authorize(Policy = "ReadOnly")]
        public async Task<ActionResult<ApiResponse<List<InsuredDto>>>> GetInsuredsAsync(CancellationToken ct)
        {
            var result = await _insuredInterface.GetInsuredsAsync(ct);

            return Ok(result);
        }

        

        [HttpGet("{id}")]
        [Authorize(Policy = "ReadOnly")]
        public async Task<IActionResult> GetInsuredById(int id)
        {
            var result = await _insuredInterface.GetInsuredByIdAsync(id);

            if (!result.Sucesso)
                return NotFound(result);        

            return Ok(result);
        }


        [HttpPost]
        [Authorize(Policy = "FullAccess")]
        public async Task<IActionResult> Create([FromBody] CreateInsuredDto dto)
        {
            var result = await _insuredInterface.CreateInsuredAsync(dto);

            if (!result.Sucesso || result.Dados is null)
                return BadRequest(result);

            return CreatedAtAction(
                nameof(GetInsuredById),
                new { id = result.Dados.id },
                result.Dados
            );
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "FullAccess")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateInsuredDto dto)
        {
            if (id != dto.id)
                return BadRequest("Id no caminho e no corpo não conferem.");

            var result = await _insuredInterface.UpdateInsuredAsync(dto);
            if (!result.Sucesso)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "FullAccess")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _insuredInterface.DeleteInsuredAsync(id);

            if (!result.Sucesso)
                return BadRequest(result);

            return Ok(result);
        }


    }
}
