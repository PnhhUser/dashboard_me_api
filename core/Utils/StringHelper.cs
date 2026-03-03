namespace Core.Utils
{
    /// <summary>
    /// Simple string utilities used across the application.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Normalize a username before storing or comparing by trimming whitespace
        /// and converting to lower‑case.
        /// </summary>
        public static string NormalizeUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return string.Empty;

            return username.Trim().ToLowerInvariant();
        }
    }
}