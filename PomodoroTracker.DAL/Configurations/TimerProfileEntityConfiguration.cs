// Autor: xpeterm01

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.DAL.Configurations;

public class TimerProfileEntityConfiguration : IEntityTypeConfiguration<TimerProfileEntity>
{
    public void Configure(EntityTypeBuilder<TimerProfileEntity> builder)
    {
        builder.Property(p => p.Name)
            .HasMaxLength(50);
    }
}
