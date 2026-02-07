using System.ComponentModel.DataAnnotations;

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
        };
    }
}


public class CreateAccountDTO
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 100 characters")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 255 characters")]
    public string Password { get; set; } = null!;

    public ActiveEnum Active { get; set; } = ActiveEnum.Inactive;

    [Required(ErrorMessage = "Role is required")]
    public RoleEnum Role { get; set; }
}

public class EditAccountDTO
{
    [Required(ErrorMessage = "Account ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Account ID must be valid")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 100 characters")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 255 characters")]
    public string Password { get; set; } = null!;

    public ActiveEnum Active { get; set; }

    [Required(ErrorMessage = "Role is required")]
    public RoleEnum Role { get; set; }
}