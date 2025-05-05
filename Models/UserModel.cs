using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApi_Coris.Models
{
    public class UserModel
    {
        [Key]
        public int id { get; set; }
        public required string name { get; set; }
        public required string email { get; set; }
        public required string password_hash { get; set; }
        public bool is_active { get; set; } = true;
        [DefaultValue(false)]
        public bool is_adm { get; set; } = false;
        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public DateTime updated_at { get; set; } = DateTime.UtcNow;
    }
}
