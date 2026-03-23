using Domain.Entities.TaskEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Config;

public class TaskStateConfig : IEntityTypeConfiguration<TaskState>
{
	public void Configure(EntityTypeBuilder<TaskState> builder)
	{
		builder.ToTable("task_states");

        builder.HasKey(s => s.StateId);

        builder.Property(s => s.StateId)
            .HasColumnName("state_id")
            .IsRequired();

        builder.Property(s => s.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(s => s.Description)
            .HasColumnName("description");

        builder.Property(s => s.CompletionIndex.Completion)
            .HasColumnName("completion")
            .IsRequired();
	}
}