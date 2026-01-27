public class EditAccountDTO
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public ActiveEnum Active { get; set; }
    public RoleEnum Role { get; set; }
}