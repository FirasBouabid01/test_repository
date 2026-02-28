using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<UserPermission> UserPermissions => Set<UserPermission>();
    public DbSet<TrainingLevel> TrainingLevels => Set<TrainingLevel>();
    public DbSet<LeaderProgress> LeaderProgresses => Set<LeaderProgress>();
    public DbSet<LevelHistory> LevelHistories => Set<LevelHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // UserRole Many-to-Many
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        // RolePermission Many-to-Many
        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });
        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);
        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId);

        // UserPermission Many-to-Many
        modelBuilder.Entity<UserPermission>()
            .HasKey(up => new { up.UserId, up.PermissionId });
        modelBuilder.Entity<UserPermission>()
            .HasOne(up => up.User)
            .WithMany(u => u.UserPermissions)
            .HasForeignKey(up => up.UserId);
        modelBuilder.Entity<UserPermission>()
            .HasOne(up => up.Permission)
            .WithMany(p => p.UserPermissions)
            .HasForeignKey(up => up.PermissionId);

        // TrainingLevel Configuration
        modelBuilder.Entity<TrainingLevel>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasIndex(e => e.SequenceOrder);

            entity.HasOne(d => d.PrerequisiteLevel)
                .WithMany(p => p.DependentLevels)
                .HasForeignKey(d => d.PrerequisiteLevelId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // LeaderProgress Configuration
        modelBuilder.Entity<LeaderProgress>(entity =>
        {
            entity.HasIndex(e => e.UserId).IsUnique();

            entity.HasOne(d => d.User)
                .WithOne(p => p.LeaderProgress)
                .HasForeignKey<LeaderProgress>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.CurrentLevel)
                .WithMany(p => p.LeaderProgresses)
                .HasForeignKey(d => d.CurrentLevelId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // LevelHistory Configuration
        modelBuilder.Entity<LevelHistory>(entity =>
        {
            entity.HasOne(d => d.User)
                .WithMany(p => p.LevelHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.FromLevel)
                .WithMany(p => p.SourceHistoryRecords)
                .HasForeignKey(d => d.FromLevelId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.ToLevel)
                .WithMany(p => p.DestinationHistoryRecords)
                .HasForeignKey(d => d.ToLevelId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.AdminUser)
                .WithMany(p => p.AdministeredChanges)
                .HasForeignKey(d => d.ChangedBy)
                .OnDelete(DeleteBehavior.Restrict);
        });

        DbSeeder.Seed(modelBuilder);
    }
}
