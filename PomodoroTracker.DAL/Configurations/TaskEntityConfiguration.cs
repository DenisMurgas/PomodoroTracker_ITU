// Autor: xpeterm01

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.DAL.Configurations;

public class TaskEntityConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(200);

        builder.HasMany(t => t.Sessions)
            .WithOne(s => s.Task)
            .HasForeignKey(s => s.TaskId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
