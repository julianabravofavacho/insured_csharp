using WebApi_Coris.Service.Abstractions;
using WebApi_Coris.Models;
using WebApi_Coris.DataContext;
using Microsoft.EntityFrameworkCore;
using WebApi_Coris.Models.Dtos;

namespace WebApi_Coris.Service.UserService
{
    public class UserService : IUserInterface
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher _hasher;

        public UserService(ApplicationDbContext context,
                           IPasswordHasher hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        public async Task<ApiResponse<List<UserDto>>> GetUsersAsync(CancellationToken ct = default)
        {
            // 1) Busca todos os segurados (as entidades)
            var entities = await _context.Users
                .AsNoTracking()
                .ToListAsync(ct);

            // 2) Se estiver vazio, devolve lista vazia e mensagem
            if (!entities.Any())
            {
                return ApiResponse<List<UserDto>>
                    .Success(new List<UserDto>(), "Nenhum segurado encontrado");
            }

            // 3) Mapeia para DTOs
            var dtos = entities
                .Select(UserDto.FromModel)
                .ToList();

            // 4) Retorna sucesso com a lista
            return ApiResponse<List<UserDto>>
                .Success(dtos);
        }


        public async Task<ApiResponse<UserDto>> GetUserByIdAsync(int id)
        {
            var entity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.id == id);

            if (entity is null)
                return ApiResponse<UserDto>.Fail("Segurado não encontrado");

            return ApiResponse<UserDto>.Success(
                UserDto.FromModel(entity)
            );
        }

        public async Task<ApiResponse<bool>> DeleteUserAsync(int id)
        {
            // 1) Tenta encontrar o segurado
            var user = await _context.Users.FindAsync(id);
            if (user is null)
                return ApiResponse<bool>.Fail("Segurado não encontrado");

            // 2) Remove e persiste
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            // 3) Retorna sucesso
            return ApiResponse<bool>.Success(true, "Segurado removido com sucesso");
        }

        public async Task<ApiResponse<UserDto>> CreateUserAsync(CreateUserDto dto)
        {
            if (dto == null)
                return ApiResponse<UserDto>.Fail("Informe os dados do segurado");

            var entity = new UserModel
            {
                name = dto.name,
                email = dto.email,
                is_active = dto.is_active,
                is_adm = dto.is_adm,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow,
                password_hash = _hasher.Hash(dto.password_hash)
            };          

            _context.Users.Add(entity);
            await _context.SaveChangesAsync();

            var dtoOut = UserDto.FromModel(entity);
            return ApiResponse<UserDto>.Success(dtoOut, "Segurado criado com sucesso");
        }

        public async Task<ApiResponse<UserDto>> UpdateUserAsync(UpdateUserDto dto)
        {
            var entity = await _context.Users.FindAsync(dto.id);
            if (entity is null)
                return ApiResponse<UserDto>.Fail("Segurado não encontrado");

            // só atualiza os campos não nulos
            if (dto.name is not null) entity.name = dto.name;
            if (dto.email is not null) entity.email = dto.email;
            if (dto.password_hash is not null) entity.email = dto.password_hash;
            if (dto.is_active) entity.is_active = dto.is_active;
            if (dto.is_adm) entity.is_adm = dto.is_adm;
            entity.updated_at = DateTime.Now;

            await _context.SaveChangesAsync();

            var output = UserDto.FromModel(entity);
            return ApiResponse<UserDto>.Success(output, "Segurado atualizado");
        }

        public Task<UserModel?> GetByEmailAsync(string email) =>
            _context.Users.SingleOrDefaultAsync(u => u.email == email && u.is_active);

    }
}
