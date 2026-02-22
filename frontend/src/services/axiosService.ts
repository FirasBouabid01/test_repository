import axios, { type AxiosInstance, type AxiosRequestConfig } from 'axios'

const API_BASE_URL = 'http://localhost:5000/api'

class AxiosService {
  private instance: AxiosInstance

  constructor() {
    this.instance = axios.create({
      baseURL: API_BASE_URL,
      headers: {
        'Content-Type': 'application/json',
      },
    })

    // Request interceptor - Add token to headers
    this.instance.interceptors.request.use(
      (config) => {
        const token = localStorage.getItem('token')
        if (token) {
          config.headers.Authorization = `Bearer ${token}`
        }
        return config
      },
      (error) => Promise.reject(error)
    )

    // Response interceptor - Handle errors
    this.instance.interceptors.response.use(
      (response) => response,
      (error) => {
        // Handle 401 Unauthorized
        if (error.response?.status === 401) {
          localStorage.removeItem('token')
          window.location.href = '/login'
        }

        // Handle errors
        return Promise.reject(error)
      }
    )
  }

  public getAxiosInstance(): AxiosInstance {
    return this.instance
  }

  public async get<T>(url: string, config?: AxiosRequestConfig) {
    return this.instance.get<T>(url, config)
  }

  public async post<T>(url: string, data?: unknown, config?: AxiosRequestConfig) {
    return this.instance.post<T>(url, data, config)
  }

  public async put<T>(url: string, data?: unknown, config?: AxiosRequestConfig) {
    return this.instance.put<T>(url, data, config)
  }

  public async delete<T>(url: string, config?: AxiosRequestConfig) {
    return this.instance.delete<T>(url, config)
  }

  public async patch<T>(url: string, data?: unknown, config?: AxiosRequestConfig) {
    return this.instance.patch<T>(url, data, config)
  }
}

export const axiosService = new AxiosService()
export default axiosService
