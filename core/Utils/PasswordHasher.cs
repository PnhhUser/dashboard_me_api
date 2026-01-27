using BCrypt.Net;

public static class PasswordHasher
{
    /// <summary>
    /// Hash plain password
    /// </summary>
    public static string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// Verify plain password with hash
    /// </summary>
    public static bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
