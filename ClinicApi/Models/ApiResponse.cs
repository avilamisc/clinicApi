using System.Net;

namespace ClinicApi.Models
{
    public class ApiResponse
    {
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }
        public object Data { get; set; }

        public ApiResponse(
            HttpStatusCode statusCode = HttpStatusCode.OK,
            string errorMessage = null,
            object result = null)
        {
            StatusCode = (int)statusCode;
            ErrorMessage = errorMessage;
            Data = result;
        }

        public static ApiResponse Ok(object result = null) =>
            new ApiResponse { StatusCode = (int)HttpStatusCode.OK, Data = result };

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
}