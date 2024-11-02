using Application.Identity;
using Application.Models;
using System.Security.Claims;

namespace Application.ServiceContracts;

public interface IJwtService
{
    AuthenticationResponse CreateJwtToken(ApplicationUser user);
    ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
}