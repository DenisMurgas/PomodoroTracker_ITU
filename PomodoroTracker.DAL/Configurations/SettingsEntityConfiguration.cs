// Autor: xpeterm01

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.DAL.Configurations;

public class SettingsEntityConfiguration : IEntityTypeConfiguration<SettingsEntity>
{
    public void Configure(EntityTypeBuilder<SettingsEntity> builder)
    {
        builder.HasOne(s => s.ActiveTimerProfile)
            .WithMany()
            .HasForeignKey(s => s.ActiveTimerProfileId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
