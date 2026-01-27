public class AccountEntity : BaseEntity
{
    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public ActiveEnum Active { get; set; } = ActiveEnum.Inactive; // Xác định account khóa - không khóa

    public DateTime? LastTimeActive { get; set; }

    public bool IsOnline { get; set; } // Xác định trạng online - offline

    public RoleEnum Role { get; set; } = RoleEnum.User;
}