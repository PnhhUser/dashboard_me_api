using System.ComponentModel.DataAnnotations;

public class AuthModel
{
    public required string AccessToken { get; set; }

    public required string RefreshToken { get; set; }

    public DateTime ExpiresAt { get; set; }
}

public class AuthUserModel
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public RoleEnum Role { get; set; }
}


public class RefreshTokenDTO
{
    public required string RefreshToken { get; set; }
}

public class LogoutDTO
{
    public required string RefreshToken { get; set; }
}

public class LoginDTO
{
    [Required]
    [MaxLength(50)]
    [MinLength(3)]
    public required string Username { get; set; }

    [Required]
    [MinLength(6)]
    [MaxLength(100)]
    public required string Password { get; set; }
}

public class RegisterDTO
{
    [Required]
    [MaxLength(50)]
    [MinLength(3)]
    public required string Username { get; set; }

    [Required]
    [MinLength(6)]
    [MaxLength(100)]
    public required string Password { get; set; }
}
