using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.Entity;

namespace Infrastructure.Database.Config;

public class ProfileConfig : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("profiles");
        
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(p => p.Login)
            .HasColumnName("login")
            .IsRequired();

        builder.Property(p => p.FirstName)
            .HasColumnName("first_name");

        builder.Property(p => p.LastName)
            .HasColumnName("last_name");

        builder.Property(p => p.DateOfBirth)
            .HasColumnName("date_of_birth")
            .IsRequired();

        builder.Property(p => p.PasswordHash)
            .HasColumnName("password_hash")
            .IsRequired();

        builder.HasIndex(p => p.Id)
            .IsUnique();
        builder.HasIndex(p => new { p.Login });
    }
}