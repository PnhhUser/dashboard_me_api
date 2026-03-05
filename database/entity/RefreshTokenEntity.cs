public class RefreshTokenEntity : BaseEntity
{
    public string Token { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public bool IsRevoked { get; set; }

    public int AccountId { get; set; }
    public AccountEntity Account { get; set; } = null!;
}