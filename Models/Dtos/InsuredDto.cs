using Intuit.Ipp.Data;
using Intuit.Ipp.OAuth2PlatformClient;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApi_Coris.Models.Dtos
{
    public record InsuredDto(
    int id,
    string name,
    string cpf_cnpj,
    string email,
    string cell_phone,
    string? postal_code = null,
    string? address = null,
    string? address_line2 = null,
    string? neighborhood = null,
    string? city = null,
    string? state = null,
    DateTime created_at = default,
    DateTime updated_at = default
)
    {
        public static InsuredDto FromModel(InsuredModel m)
            => new InsuredDto(
                m.id, m.name, m.cpf_cnpj, m.email, m.cell_phone,
                m.postal_code, m.address, m.address_line2,
                m.neighborhood, m.city, m.state,
                m.created_at, m.updated_at
            );
    }
}
