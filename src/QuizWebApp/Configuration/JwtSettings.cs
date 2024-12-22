namespace QuizWebApp.Configuration;

public class JwtSettings
{
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required string Secretkey { get; set; }
}