using System.Net;

namespace ClinicApi.Models
{
    public class ApiResponse
    {
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }
        public object Result { get; set; }

        public ApiResponse(
            HttpStatusCode statusCode = HttpStatusCode.OK,
            string errorMessage = null,
            object result = null)
        {
            StatusCode = (int)statusCode;
            ErrorMessage = errorMessage;
            Result = result;
        }

        public static ApiResponse Ok(object result = null) =>
            new ApiResponse { StatusCode = (int)HttpStatusCode.OK, Result = result };

        public static ApiResponse ValidationError(string validationMessage) =>
            new ApiResponse { StatusCode = (int)HttpStatusCode.OK, ErrorMessage = validationMessage };
    }
}