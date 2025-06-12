using System;

namespace backend.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static ApiResponse<T> CreateSuccess(T data, string message = "Operação realizada com sucesso")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> CreateError(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default(T)
            };
        }
    }

    // Versão sem generic para casos onde não há dados de retorno
    public class ApiResponse : ApiResponse<object>
    {
        public static ApiResponse CreateSuccess(string message = "Operação realizada com sucesso")
        {
            return new ApiResponse
            {
                Success = true,
                Message = message,
                Data = null
            };
        }

        public new static ApiResponse CreateError(string message)
        {
            return new ApiResponse
            {
                Success = false,
                Message = message,
                Data = null
            };
        }
    }
}