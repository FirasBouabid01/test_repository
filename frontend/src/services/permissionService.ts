import axiosService from './axiosService'

export interface PermissionDto {
  id: string
  name: string
  roleCount: number
  userCount: number
}

export interface PaginatedResponse<T> {
  data: T[]
  pageNumber: number
  pageSize: number
  totalCount: number
  totalPages: number
  hasPreviousPage: boolean
  hasNextPage: boolean
}

export class PermissionService {
  static async getPermissions(
    pageNumber?: number,
    pageSize?: number,
    searchTerm?: string,
    sortBy?: string,
    sortDescending?: boolean
  ) {
    const response = await axiosService.get<PaginatedResponse<PermissionDto>>('/permissions', {
      params: {
        pageNumber: pageNumber ?? 1,
        pageSize: pageSize ?? 10,
        searchTerm,
        sortBy: sortBy ?? 'name',
        sortDescending: sortDescending ?? false,
      },
    })
    return response.data
  }

  static async assignRoleToUser(userId: string, roleId: string) {
    const response = await axiosService.post('/permissions/assign-role-to-user', {
      userId,
      roleId,
    })
    return response.data
  }

  static async removeRoleFromUser(userId: string, roleId: string) {
    const response = await axiosService.post('/permissions/remove-role-from-user', {
      userId,
      roleId,
    })
    return response.data
  }

  static async assignPermissionToUser(userId: string, permissionId: string) {
    const response = await axiosService.post('/permissions/assign-permission-to-user', {
      userId,
      permissionId,
    })
    return response.data
  }

  static async removePermissionFromUser(userId: string, permissionId: string) {
    const response = await axiosService.post('/permissions/remove-permission-from-user', {
      userId,
      permissionId,
    })
    return response.data
  }

  static async assignPermissionToRole(roleId: string, permissionId: string) {
    const response = await axiosService.post('/permissions/assign-permission-to-role', {
      roleId,
      permissionId,
    })
    return response.data
  }

  static async removePermissionFromRole(roleId: string, permissionId: string) {
    const response = await axiosService.post('/permissions/remove-permission-from-role', {
      roleId,
      permissionId,
    })
    return response.data
  }
}
