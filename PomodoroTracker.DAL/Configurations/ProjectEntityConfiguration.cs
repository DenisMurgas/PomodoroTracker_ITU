// Autor: xpeterm01

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.DAL.Configurations;

public class ProjectEntityConfiguration : IEntityTypeConfiguration<ProjectEntity>
{
    public void Configure(EntityTypeBuilder<ProjectEntity> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(100);

        builder.Property(p => p.Color)
            .HasMaxLength(9);

        builder.HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
