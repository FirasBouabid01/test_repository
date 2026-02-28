using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class DbSeeder
{
    // Fixed GUIDs (important!)
    private static readonly Guid AdminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid ModeratorRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    private static readonly Guid UserCreatePermissionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private static readonly Guid UserUpdatePermissionId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
    private static readonly Guid UserDeletePermissionId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");

    private static readonly Guid TrainingLevelReadId = Guid.Parse("d1111111-1111-1111-1111-111111111111");
    private static readonly Guid TrainingLevelCreateId = Guid.Parse("d2222222-2222-2222-2222-222222222222");
    private static readonly Guid TrainingLevelUpdateId = Guid.Parse("d3333333-3333-3333-3333-333311111111");
    private static readonly Guid TrainingLevelDeleteId = Guid.Parse("d4444444-4444-4444-4444-444422222222");

    public static void Seed(ModelBuilder modelBuilder)
    {
        // Roles
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = AdminRoleId, Name = "Admin" },
            new Role { Id = ModeratorRoleId, Name = "Moderator" }
        );

        // Permissions
        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = UserCreatePermissionId, Name = "User.Create" },
            new Permission { Id = UserUpdatePermissionId, Name = "User.Update" },
            new Permission { Id = UserDeletePermissionId, Name = "User.Delete" },
            new Permission { Id = TrainingLevelReadId, Name = "TrainingLevel.Read" },
            new Permission { Id = TrainingLevelCreateId, Name = "TrainingLevel.Create" },
            new Permission { Id = TrainingLevelUpdateId, Name = "TrainingLevel.Update" },
            new Permission { Id = TrainingLevelDeleteId, Name = "TrainingLevel.Delete" }
        );

        // RolePermissions (Admin has all)
        modelBuilder.Entity<RolePermission>().HasData(
            new RolePermission { RoleId = AdminRoleId, PermissionId = UserCreatePermissionId },
            new RolePermission { RoleId = AdminRoleId, PermissionId = UserUpdatePermissionId },
            new RolePermission { RoleId = AdminRoleId, PermissionId = UserDeletePermissionId },
            new RolePermission { RoleId = AdminRoleId, PermissionId = TrainingLevelReadId },
            new RolePermission { RoleId = AdminRoleId, PermissionId = TrainingLevelCreateId },
            new RolePermission { RoleId = AdminRoleId, PermissionId = TrainingLevelUpdateId },
            new RolePermission { RoleId = AdminRoleId, PermissionId = TrainingLevelDeleteId }
        );
    }
}