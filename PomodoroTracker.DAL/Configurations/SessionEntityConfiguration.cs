// Autor: xpeterm01

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.DAL.Configurations;

public class SessionEntityConfiguration : IEntityTypeConfiguration<SessionEntity>
{
    public void Configure(EntityTypeBuilder<SessionEntity> builder)
    {
        builder.HasIndex(s => s.StartedAt);
    }
}
