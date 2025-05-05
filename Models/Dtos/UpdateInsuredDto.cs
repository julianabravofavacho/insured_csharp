namespace WebApi_Coris.Models.Dtos
{
    public record UpdateInsuredDto(
        int id,
        string? name = null,
        string? cpf_cnpj = null,
        string? email = null,
        string? cell_phone = null,
        string? postal_code = null,
        string? address = null,
        string? address_line2 = null,
        string? neighborhood = null,
        string? city = null,
        string? state = null
    );
}
