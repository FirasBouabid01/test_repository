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
            new Permission { Id = UserDeletePermissionId, Name = "User.Delete" }
        );

        // RolePermissions (Admin has all)
        modelBuilder.Entity<RolePermission>().HasData(
            new RolePermission { RoleId = AdminRoleId, PermissionId = UserCreatePermissionId },
            new RolePermission { RoleId = AdminRoleId, PermissionId = UserUpdatePermissionId },
            new RolePermission { RoleId = AdminRoleId, PermissionId = UserDeletePermissionId }
        );
    }
}