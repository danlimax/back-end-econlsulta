namespace api_econsulta.Models
{
    // User.cs
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public UserType Type { get; set; } // Doctor or Patient
}

public enum UserType
{
    Doctor,
    Patient
}


}