using System;

namespace GlobalResponse
{
    public class GlobalApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public GlobalApiResponse(T data, bool success = true)
        {
            Success = success;
            Data = data;
        }

    }
    public static class ApiResponse
    {
        public static GlobalApiResponse<T> Success<T>(T data) => new(data);
        public static GlobalApiResponse<object?> Success() => new(null);
        public static GlobalApiResponse<object?> Failure(List<string> errors) => new(null, false) { Errors = errors };

    }
}


