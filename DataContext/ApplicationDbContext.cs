using Microsoft.EntityFrameworkCore;
using WebApi_Coris.Models;

namespace WebApi_Coris.DataContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<InsuredModel> Insureds { get; set; }
        public DbSet<PersonalAccessTokenModel> PersonalAccessTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Usuários e Segurados continuam simples
            modelBuilder.Entity<UserModel>()
                        .ToTable("users");

            modelBuilder.Entity<InsuredModel>()
                        .ToTable("insureds");

            // Configuração completa da tabela personal_access_tokens
            modelBuilder.Entity<PersonalAccessTokenModel>(b =>
            {
                b.ToTable("personal_access_tokens");

                // PK e auto-increment
                b.HasKey(x => x.id);
                b.Property(x => x.id)
                 .ValueGeneratedOnAdd();

                // Colunas obrigatórias / limites
                b.Property(x => x.tokenable_type)
                 .HasColumnType("nvarchar(255)")
                 .HasMaxLength(255)
                 .IsRequired();

                b.Property(x => x.tokenable_id)
                 .IsRequired();

                b.Property(x => x.name)
                 .HasColumnType("nvarchar(255)")
                 .HasMaxLength(255)
                 .IsRequired();

                // Aqui aumentamos de 64 para 512
                b.Property(x => x.token)
                 .HasColumnType("nvarchar(512)")
                 .HasMaxLength(512)
                 .IsRequired();

                // Restantes das colunas
                b.Property(x => x.abilities)
                 .HasColumnType("nvarchar(max)");

                b.Property(x => x.last_used_at)
                 .HasColumnType("datetime2");

                b.Property(x => x.expires_at)
                 .HasColumnType("datetime2");

                b.Property(x => x.created_at)
                 .HasColumnType("datetime2")
                 .HasDefaultValueSql("GETUTCDATE()");

                b.Property(x => x.updated_at)
                 .HasColumnType("datetime2");

                // Índices
                b.HasIndex(x => x.token)
                 .IsUnique()
                 .HasDatabaseName("personal_access_tokens_token_unique");

                b.HasIndex(x => new { x.tokenable_type, x.tokenable_id })
                 .HasDatabaseName("personal_access_tokens_tokenable_type_tokenable_id_index");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
