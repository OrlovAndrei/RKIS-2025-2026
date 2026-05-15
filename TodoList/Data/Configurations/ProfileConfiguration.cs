using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("Profiles");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Login)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Password)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.BirthYear)
            .IsRequired();

        builder.HasIndex(p => p.Login)
            .IsUnique();

        builder.ToTable(t => t.HasCheckConstraint(
            "CK_Profile_BirthYear",
            "BirthYear >= 1900 AND BirthYear <= 2100"));
    }
}
