namespace API.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }
    public int StatusCode { get; set; }

    public static ApiResponse<T> CreateSuccess(T? data, int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            StatusCode = statusCode
        };
    }

    public static ApiResponse<T> CreateError(string error, int statusCode)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Error = error,
            StatusCode = statusCode
        };
    }
}
