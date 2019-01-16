using System;

namespace Clinic.Core.DtoModels
{
    public class RefreshTokenDto
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int UserId { get; set; }
        public DateTime ExpiresUtc { get; set; }
    }
}
