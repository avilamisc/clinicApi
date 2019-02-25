namespace Clinic.Core.Encryption
{
    public static class Hashing
    {
        public static string HashPassword(string password, int complexity = 15)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, GenerateRandomSalt(complexity));
        }

        public static bool VerifyPassword(string password, string validHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, validHash);
        }

        private static string GenerateRandomSalt(int complexity)
        {
            return BCrypt.Net.BCrypt.GenerateSalt(complexity);
        }
    }
}