using System.ComponentModel.DataAnnotations;

namespace WebApi_Coris.Models
{
    public class InsuredModel
    {
        [Key]
        public int id { get; set; }
        public required string name { get; set; }
        public required string cpf_cnpj { get; set; }
        public required string email { get; set; }
        public required string cell_phone { get; set; }
        public string? postal_code { get; set; } = null;
        public string? address { get; set; } =null;
        public string? address_line2 { get; set; } = null;
        public string? neighborhood { get; set; } = null;
        public string? city { get; set; } = null;
        public string? state { get; set; } = null;
        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public DateTime updated_at { get; set; } = DateTime.UtcNow;
    }
}
