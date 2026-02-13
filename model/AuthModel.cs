using System.ComponentModel.DataAnnotations;

public class LoginModel
{
    public required string Token { get; set; }

    public DateTime ExpiresAt { get; set; }
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
