using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi_Coris.Models
{
    public class PersonalAccessTokenModel
    {
        [Key]
        [Column("id")]
        public long id { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("tokenable_type", TypeName = "nvarchar(255)")]
        public string tokenable_type { get; set; } = null!;

        [Required]
        [Column("tokenable_id")]
        public long tokenable_id { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("name", TypeName = "nvarchar(255)")]
        public string name { get; set; } = null!;

        [Required, MaxLength(512)]
        [Column("token", TypeName = "nvarchar(512)")]
        public string token { get; set; } = null!;

        [Column("abilities", TypeName = "nvarchar(max)")]
        public string? abilities { get; set; }

        [Column("last_used_at", TypeName = "datetime2")]
        public DateTime? last_used_at { get; set; }

        [Column("expires_at", TypeName = "datetime2")]
        public DateTime? expires_at { get; set; }

        [Column("created_at", TypeName = "datetime2")]
        public DateTime? created_at { get; set; }

        [Column("updated_at", TypeName = "datetime2")]
        public DateTime? updated_at { get; set; }
    }
}
