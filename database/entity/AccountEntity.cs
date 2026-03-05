public class AccountEntity : BaseEntity
{
    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public ActiveEnum Active { get; set; } = ActiveEnum.Inactive;

    public DateTime? LastTimeActive { get; set; }

    public bool IsOnline { get; set; }

    public RoleEnum Role { get; set; } = RoleEnum.User;

    public ICollection<RefreshTokenEntity> RefreshTokens { get; set; } = new List<RefreshTokenEntity>();
}