using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.Entity;

namespace Infrastructure.Database.Config;

public class TodoTaskConfig : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.ToTable("tasks");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(t => t.Status)
            .HasColumnName("state")
            .IsRequired();

        builder.Property(t => t.Status)
            .HasConversion(
            v => v.ToString(), // Из модели в базу (Enum -> string)
            v => Enum.Parse<TodoStatus>(v) // Из базы в модель (string -> Enum)
        );

        builder.Property(t => t.ProfileId)
            .HasColumnName("profile_id")
            .IsRequired();

        builder.HasOne(t => t.Profile)
            .WithMany()
            .HasForeignKey(t => t.ProfileId)
            .HasForeignKey("profile")
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(t => t.Text)
            .HasColumnName("text")
            .IsRequired();

        builder.Property(t => t.LastUpdate)
            .HasColumnName("last_update")
            .IsRequired();

        builder.HasIndex(t => t.Id)
            .IsUnique();
        builder.HasIndex(t => t.ProfileId);
    }
}