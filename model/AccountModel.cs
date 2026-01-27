public class AccountModel
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public ActiveEnum Active { get; set; }
    public RoleEnum Role { get; set; }
    public DateTime? LastTimeActive { get; set; }
    public bool IsOnline { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public static AccountModel ToModel(AccountEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new AccountModel
        {
            Id = entity.Id,
            Username = entity.Username,
            Active = entity.Active,
            Role = entity.Role,
            LastTimeActive = entity.LastTimeActive,
            IsOnline = entity.IsOnline,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            DeletedAt = entity.DeletedAt
        };
    }
}
