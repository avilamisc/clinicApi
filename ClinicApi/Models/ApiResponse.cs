using System.Net;

namespace ClinicApi.Models
{
    public class ApiResponse
    {
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }

        public ApiResponse(
            HttpStatusCode statusCode = HttpStatusCode.OK,
            string errorMessage = null)
        {
            StatusCode = (int)statusCode;
            ErrorMessage = errorMessage;
        }

        public static ApiResponse Ok() =>
            new ApiResponse { StatusCode = (int)HttpStatusCode.OK };

        public static ApiResponse ValidationError(string validationMessage) =>
            new ApiResponse { StatusCode = (int)HttpStatusCode.OK, ErrorMessage = validationMessage };

        public static ApiResponse UnsupportedMediaType() =>
            new ApiResponse { StatusCode = (int)HttpStatusCode.UnsupportedMediaType };

        public static ApiResponse BadRequest(string errorMessage = null) =>
            new ApiResponse { StatusCode = (int)HttpStatusCode.BadRequest, ErrorMessage = errorMessage };

        public static ApiResponse InternalError(string errorMessage = null) =>
            new ApiResponse { StatusCode = (int)HttpStatusCode.InternalServerError, ErrorMessage = errorMessage };

        public static ApiResponse NotFound() =>
            new ApiResponse { StatusCode = (int)HttpStatusCode.NotFound };
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }

        public ApiResponse(
            HttpStatusCode statusCode = HttpStatusCode.OK,
            string errorMessage = null,
            T result = default(T)) : base(statusCode, errorMessage)
        {
            Data = result;
        }

        public static ApiResponse<T> Ok(T result) =>
            new ApiResponse<T> { StatusCode = (int)HttpStatusCode.OK, Data = result };

        public static new ApiResponse<T> ValidationError(string validationMessage) =>
            new ApiResponse<T> { StatusCode = (int)HttpStatusCode.OK, ErrorMessage = validationMessage };

        public static new ApiResponse<T> UnsupportedMediaType() =>
            new ApiResponse<T> { StatusCode = (int)HttpStatusCode.UnsupportedMediaType };

        public static new ApiResponse<T> BadRequest(string errorMessage = null) =>
            new ApiResponse<T> { StatusCode = (int)HttpStatusCode.BadRequest, ErrorMessage = errorMessage };

        public static new ApiResponse<T> InternalError(string errorMessage = null) =>
            new ApiResponse<T> { StatusCode = (int)HttpStatusCode.InternalServerError, ErrorMessage = errorMessage };

        public static new ApiResponse<T> NotFound() =>
            new ApiResponse<T> { StatusCode = (int)HttpStatusCode.NotFound };
    }
}