using WebApi_Coris.Service.Abstractions;
using WebApi_Coris.Models;
using WebApi_Coris.DataContext;
using Microsoft.EntityFrameworkCore;

using Intuit.Ipp.Data;
using Microsoft.AspNetCore.Mvc;
using WebApi_Coris.Models.Dtos;

namespace WebApi_Coris.Service.InsuredService
{
    public class InsuredService : IInsuredInterface
    {
        private readonly ApplicationDbContext _context;

        public InsuredService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<InsuredDto>>> GetInsuredsAsync(CancellationToken ct = default)
        {
            // 1) Busca todos os segurados (as entidades)
            var entities = await _context.Insureds
                .AsNoTracking()
                .ToListAsync(ct);

            // 2) Se estiver vazio, devolve lista vazia e mensagem
            if (!entities.Any())
            {
                return ApiResponse<List<InsuredDto>>
                    .Success(new List<InsuredDto>(), "Nenhum segurado encontrado");
            }

            // 3) Mapeia para DTOs
            var dtos = entities
                .Select(InsuredDto.FromModel)
                .ToList();

            // 4) Retorna sucesso com a lista
            return ApiResponse<List<InsuredDto>>
                .Success(dtos);
        }


        public async Task<ApiResponse<InsuredDto>> GetInsuredByIdAsync(int id)
        {
            var entity = await _context.Insureds
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.id == id);

            if (entity is null)
                return ApiResponse<InsuredDto>.Fail("Segurado não encontrado");

            return ApiResponse<InsuredDto>.Success(
                InsuredDto.FromModel(entity)
            );
        }

        public async Task<ApiResponse<bool>> DeleteInsuredAsync(int id)
        {
            // 1) Tenta encontrar o segurado
            var insured = await _context.Insureds.FindAsync(id);
            if (insured is null)
                return ApiResponse<bool>.Fail("Segurado não encontrado");

            // 2) Remove e persiste
            _context.Insureds.Remove(insured);
            await _context.SaveChangesAsync();

            // 3) Retorna sucesso
            return ApiResponse<bool>.Success(true, "Segurado removido com sucesso");
        }

        public async Task<ApiResponse<InsuredDto>> CreateInsuredAsync(CreateInsuredDto dto)
        {
            if (dto == null)
                return ApiResponse<InsuredDto>.Fail("Informe os dados do segurado");

            var entity = new InsuredModel
            {
                name = dto.name,
                cpf_cnpj = dto.cpf_cnpj,
                email = dto.email,
                cell_phone = dto.cell_phone,
                postal_code = dto.postal_code,
                address = dto.address,
                address_line2 = dto.address_line2,
                neighborhood = dto.neighborhood,
                city = dto.city,
                state = dto.state,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow
            };

            _context.Insureds.Add(entity);
            await _context.SaveChangesAsync();

            // converte só o criado
            var dtoOut = InsuredDto.FromModel(entity);
            return ApiResponse<InsuredDto>
                .Success(dtoOut, "Segurado criado com sucesso");
        }

        public async Task<ApiResponse<InsuredDto>> UpdateInsuredAsync(UpdateInsuredDto dto)
        {
            var entity = await _context.Insureds.FindAsync(dto.id);
            if (entity is null)
                return ApiResponse<InsuredDto>.Fail("Segurado não encontrado");

            // só atualiza os campos não nulos
            if (dto.name is not null) entity.name = dto.name;
            if (dto.cpf_cnpj is not null) entity.cpf_cnpj = dto.cpf_cnpj;
            if (dto.email is not null) entity.email = dto.email;
            if (dto.cell_phone is not null) entity.cell_phone = dto.cell_phone;
            // e assim por diante...
            entity.updated_at = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var output = InsuredDto.FromModel(entity);
            return ApiResponse<InsuredDto>.Success(output, "Segurado atualizado");
        }

    }
}
