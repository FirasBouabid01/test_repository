import axiosService from './axiosService' 

export interface RoleDto {
  id: string
  name: string
  permissions: string[]
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

export interface CreateRoleDto {
  name: string
  permissionIds?: string[]
}

export interface UpdateRoleDto {
  name: string
  permissionIds?: string[]
}

export class RoleService {
  static async getRoles(
    pageNumber?: number,
    pageSize?: number,
    searchTerm?: string,
    sortBy?: string,
    sortDescending?: boolean
  ) {
    const response = await axiosService.get<PaginatedResponse<RoleDto>>('/roles', {
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

  static async getRoleById(roleId: string) {
    const response = await axiosService.get<RoleDto>(`/roles/${roleId}`)
    return response.data
  }

  static async createRole(dto: CreateRoleDto) {
    const response = await axiosService.post<RoleDto>('/roles', dto)
    return response.data
  }

  static async updateRole(roleId: string, dto: UpdateRoleDto) {
    const response = await axiosService.put<RoleDto>(`/roles/${roleId}`, dto)
    return response.data
  }

  static async deleteRole(roleId: string) {
    const response = await axiosService.delete<{ message: string }>(`/roles/${roleId}`)
    return response.data
  }
}
