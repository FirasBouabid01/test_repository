# Quick Reference Guide - API Endpoints & Frontend Usage

## API Base URL
- **Development:** `http://localhost:5000/api`
- Update this in [src/services/axiosService.ts](src/services/axiosService.ts) if needed

## Authentication
All endpoints (except login/register) require JWT token in Authorization header:
```
Authorization: Bearer {token}
```
This is automatically handled by the Axios service interceptor.

---

## Users Management

### Get All Users (Paginated)
```
GET /users?pageNumber=1&pageSize=10&searchTerm=&isAdmin=false&sortBy=username&sortDescending=false
Response: PaginatedResponse<UserDto>
```

### Get Single User
```
GET /users/{userId}
Response: UserDto
```

### Create User
```
POST /users
Body: CreateUserCommand
{
  "username": "john_doe",
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "password": "Password123!",
  "dateOfBirth": "1990-01-01T00:00:00Z",
  "phoneNumber": "+1234567890",
  "address": "123 Main St"
}
Response: { id: "guid" }
```

### Update User
```
PUT /users/{userId}
Body: UpdateUserDto
{
  "firstName": "John",
  "lastName": "Smith",
  "email": "john.smith@example.com",
  "dateOfBirth": "1990-01-01T00:00:00Z",
  "phoneNumber": "+1234567890",
  "address": "123 Main St",
  "isAdmin": true
}
Response: UserDto
```

### Delete User
```
DELETE /users/{userId}
Response: { message: "User deleted successfully." }
```

### Frontend Usage (UserService)
```typescript
import { UserService } from '@/services/userService'

// Get paginated users
const response = await UserService.getUsers(
  pageNumber = 1,
  pageSize = 10,
  searchTerm = 'john',
  isAdmin = false,
  sortBy = 'email',
  sortDescending = false
)

// Get single user
const user = await UserService.getUserById(userId)

// Update user
const updated = await UserService.updateUser(userId, {
  firstName: 'Jane',
  email: 'jane@example.com',
  // ... other fields
})

// Delete user
await UserService.deleteUser(userId)
```

---

## Roles Management

### Get All Roles (Paginated)
```
GET /roles?pageNumber=1&pageSize=10&searchTerm=&sortBy=name&sortDescending=false
Response: PaginatedResponse<RoleDto>
```

### Get Single Role
```
GET /roles/{roleId}
Response: RoleDto
```

### Create Role
```
POST /roles
Body: CreateRoleDto
{
  "name": "Editor",
  "permissionIds": ["guid1", "guid2"]
}
Response: RoleDto
```

### Update Role
```
PUT /roles/{roleId}
Body: UpdateRoleDto
{
  "name": "Editor",
  "permissionIds": ["guid1", "guid2"]
}
Response: RoleDto
```

### Delete Role
```
DELETE /roles/{roleId}
Note: Only works if no users are assigned to this role
Response: { message: "Role deleted successfully." }
```

### Frontend Usage (RoleService)
```typescript
import { RoleService } from '@/services/roleService'

// Get paginated roles
const response = await RoleService.getRoles(
  pageNumber = 1,
  pageSize = 10,
  searchTerm = 'admin'
)

// Create role
const newRole = await RoleService.createRole({
  name: 'Moderator',
  permissionIds: [...]
})

// Update role
const updated = await RoleService.updateRole(roleId, {
  name: 'Senior Moderator',
  permissionIds: [...]
})

// Delete role
await RoleService.deleteRole(roleId)
```

---

## Permissions Management

### Get All Permissions (Paginated, Read-only)
```
GET /permissions?pageNumber=1&pageSize=10&searchTerm=&sortBy=name&sortDescending=false
Response: PaginatedResponse<PermissionDto>
```

**Note:** Permissions are read-only. They can only be managed through database seeding or admin operations.

### Frontend Usage (PermissionService)
```typescript
import { PermissionService } from '@/services/permissionService'

const response = await PermissionService.getPermissions(
  pageNumber = 1,
  pageSize = 10,
  searchTerm = 'user'
)
```

---

## Role & Permission Assignment

### Assign Role to User
```
POST /permissions/assign-role-to-user
Body: { userId: "guid", roleId: "guid" }
Response: { message: "Role assigned successfully." }
```

### Remove Role from User
```
POST /permissions/remove-role-from-user
Body: { userId: "guid", roleId: "guid" }
Response: { message: "Role removed successfully." }
```

### Assign Permission to User (Direct)
```
POST /permissions/assign-permission-to-user
Body: { userId: "guid", permissionId: "guid" }
Response: { message: "Permission assigned successfully." }
```

### Remove Permission from User
```
POST /permissions/remove-permission-from-user
Body: { userId: "guid", permissionId: "guid" }
Response: { message: "Permission removed successfully." }
```

### Assign Permission to Role
```
POST /permissions/assign-permission-to-role
Body: { roleId: "guid", permissionId: "guid" }
Response: { message: "Permission assigned to role successfully." }
```

### Remove Permission from Role
```
POST /permissions/remove-permission-from-role
Body: { roleId: "guid", permissionId: "guid" }
Response: { message: "Permission removed from role successfully." }
```

### Frontend Usage (PermissionService)
```typescript
import { PermissionService } from '@/services/permissionService'

// Assign role to user
await PermissionService.assignRoleToUser(userId, roleId)

// Assign permission to user
await PermissionService.assignPermissionToUser(userId, permissionId)

// Assign permission to role
await PermissionService.assignPermissionToRole(roleId, permissionId)

// Remove operations
await PermissionService.removeRoleFromUser(userId, roleId)
await PermissionService.removePermissionFromUser(userId, permissionId)
await PermissionService.removePermissionFromRole(roleId, permissionId)
```

---

## Frontend Pages

| Route | Component | Access | Description |
|-------|-----------|--------|-------------|
| `/` | HomeView | Public | Home page |
| `/login` | LoginView | Public | Login page |
| `/register` | RegisterView | Public | Registration |
| `/dashboard` | DashboardView | Auth | User dashboard |
| `/admin/dashboard` | AdminDashboard | Admin | Admin dashboard |
| `/profile` | ProfileView | Auth | User profile |
| `/users` | UsersView | Admin | User management |
| `/roles` | RolesView | Admin | Role management |
| `/permissions` | PermissionsView | Admin | Permission viewing (read-only) |
| `/assignments` | AssignmentsView | Admin | Role/Permission assignment |

---

## Internationalization (i18n)

### Change Language
```typescript
import { useI18n } from 'vue-i18n'

const { locale } = useI18n()

// Change to Arabic
locale.value = 'ar'

// Change to English
locale.value = 'en'
```

### Use Translations in Templates
```vue
<template>
  <h1>{{ $t('menu.dashboard') }}</h1>
  <button>{{ $t('common.save') }}</button>
</template>
```

### Use Translations in Scripts
```typescript
import { useI18n } from 'vue-i18n'

const { t } = useI18n()

const title = t('users.title')
const confirmMessage = t('common.confirm')
```

### Supported Languages
- **English** (`en`) - Default
- **Arabic** (`ar`)

---

## Axios Service Usage

The Axios service automatically:
1. Injects JWT token from localStorage
2. Handles 401 errors by redirecting to login
3. Uses the correct API base URL

### Direct Usage (if needed)
```typescript
import { axiosService } from '@/services/axiosService'

// GET request
const response = await axiosService.get<UserDto>('/users/123')
const data = response.data

// POST request
const response = await axiosService.post<UserDto>('/users', userData)

// PUT request
const response = await axiosService.put<UserDto>('/users/123', updatedData)

// DELETE request
const response = await axiosService.delete('/users/123')
```

---

## Common Filter/Sort Options

### User Sorting
- `username` (default)
- `email`
- `firstname`
- `lastname`
- `isadmin`

### Role Sorting
- `name` (default)

### Permission Sorting
- `name` (default)

### Sort Direction
- `sortDescending: false` (ascending, default)
- `sortDescending: true` (descending)

---

## Error Handling

The Axios service automatically catches errors. In components:

```typescript
try {
  const response = await UserService.getUsers()
  // Handle success
} catch (error) {
  console.error('Error:', error)
  // Handle error
}
```

Common HTTP Status Codes:
- **200** - Success
- **400** - Bad request / Validation error
- **401** - Unauthorized (token missing/invalid)
- **403** - Forbidden (insufficient permissions)
- **404** - Not found
- **500** - Server error

---

## Database Migrations

Run pending migrations:
```bash
cd backend
dotnet ef database update --project Infrastructure --startup-project API
```

The `20260218000000_AddIsAdminToUser` migration adds the `IsAdmin` column to the Users table.

---

## Notes

- All timestamps are in UTC
- Pagination is 1-based (first page is 1, not 0)
- Search is case-insensitive
- Role deletion is prevented if users are assigned
- Permission management is read-only from API (managed via database)
- The IsAdmin field is separate from roles - it's an independent flag
- All API responses follow the PaginatedResponse pattern with data, page info, and totals
