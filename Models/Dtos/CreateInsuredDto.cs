namespace WebApi_Coris.Models.Dtos
{
    public record CreateInsuredDto(
        string name,
        string cpf_cnpj,
        string email,
        string cell_phone,
        string? postal_code = null,
        string? address = null,
        string? address_line2 = null,
        string? neighborhood = null,
        string? city = null,
        string? state = null
    );
}
