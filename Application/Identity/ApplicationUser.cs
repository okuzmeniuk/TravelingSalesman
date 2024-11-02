using Microsoft.AspNetCore.Identity;

namespace Application.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
}
