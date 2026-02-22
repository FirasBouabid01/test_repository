import axiosService from './axiosService'

export interface UserDto {
  id: string
  username: string
  email: string
  firstName: string
  lastName: string
  dateOfBirth: string
  phoneNumber: string
  address: string
  isAdmin: boolean
  roles: string[]
  permissions: string[]
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

export interface UpdateUserDto {
  firstName: string
  lastName: string
  email: string
  dateOfBirth: string
  phoneNumber: string
  address: string
  isAdmin: boolean
}

export class UserService {
  static async getUsers(
    pageNumber?: number,
    pageSize?: number,
    searchTerm?: string,
    isAdmin?: boolean,
    sortBy?: string,
    sortDescending?: boolean
  ) {
    const response = await axiosService.get<PaginatedResponse<UserDto>>('/users', {
      params: {
        pageNumber: pageNumber ?? 1,
        pageSize: pageSize ?? 10,
        searchTerm,
        isAdmin,
        sortBy: sortBy ?? 'username',
        sortDescending: sortDescending ?? false,
      },
    })
    return response.data
  }

  static async getUserById(userId: string) {
    const response = await axiosService.get<UserDto>(`/users/${userId}`)
    return response.data
  }

  static async updateUser(userId: string, dto: UpdateUserDto) {
    const response = await axiosService.put<UserDto>(`/users/${userId}`, dto)
    return response.data
  }

  static async deleteUser(userId: string) {
    const response = await axiosService.delete<{ message: string }>(`/users/${userId}`)
    return response.data
  }

  static async getUserProfile() {
    const response = await axiosService.get<UserDto>('/users/profile')
    return response.data
  }
}
