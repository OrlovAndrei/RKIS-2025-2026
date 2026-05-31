using Domain.Entities.ProfileEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Config;

public class ProfileConfig : IEntityTypeConfiguration<Profile>
{
	public void Configure(EntityTypeBuilder<Profile> builder)
	{
		builder.HasKey(p => p.ProfileId);

        builder.Property(p => p.ProfileId)
            .HasColumnName("profile_id")
            .IsRequired();

        builder.Property(p => p.FirstName)
            .HasColumnName("first_name")
            .IsRequired();

        builder.Property(p => p.LastName)
            .HasColumnName("last_name")
            .IsRequired();

        builder.Property(p => p.DateOfBirth)
            .HasColumnName("date_of_birth")
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(p => p.PasswordHash)
            .HasColumnName("password_hash")
            .IsRequired();
	}
}