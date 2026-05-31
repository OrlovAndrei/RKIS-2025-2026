using Domain.Entities.TaskEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Config;

public class TodoTaskConfig : IEntityTypeConfiguration<TodoTask>
{
    public void Configure(EntityTypeBuilder<TodoTask> builder)
    {
        builder.ToTable("tasks");

        builder.HasKey(t => t.TaskId);

        builder.Property(t => t.TaskId)
            .HasColumnName("task_id")
            .IsRequired();

        builder.Property(t => t.State.CompletionIndex)
            .HasColumnName("state_id")
            .IsRequired();

        builder.HasOne(t => t.State)
            .WithMany()
            .HasForeignKey("state_id")
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(t => t.Priority.Level)
            .HasColumnName("priority_level")
            .IsRequired();

        builder.HasOne(t => t.Priority)
            .WithMany()
            .HasForeignKey("priority_level")
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(t => t.ProfileId)
            .HasColumnName("profile_id")
            .IsRequired();
        
        builder.Property(t => t.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(t => t.Description)
            .HasColumnName("description")
            .HasMaxLength(1000);

        builder.Property(t => t.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(t => t.Deadline)
            .HasColumnName("deadline");

        
    }
}