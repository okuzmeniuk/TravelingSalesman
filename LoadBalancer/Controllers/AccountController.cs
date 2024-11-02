using Application.Identity;
using Application.Models;
using Application.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoadBalancer.Controllers;

[Route("api/account")]
[ApiController]
[AllowAnonymous]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtService _jwtService;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApplicationUser>> PostRegister(RegisterDTO registerDTO)
    {
        if (!ModelState.IsValid)
        {
            string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return Problem(errorMessage);
        }

        ApplicationUser user = new ApplicationUser()
        {
            Email = registerDTO.Email,
            UserName = registerDTO.Email,
        };

        IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);

            var authenticationResponse = _jwtService.CreateJwtToken(user);
            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpiration = authenticationResponse.RefreshTokenExpiration;
            await _userManager.UpdateAsync(user);

            return Ok(authenticationResponse);
        }
        else
        {
            string errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
            return Problem(errorMessage);
        }
    }

    [HttpGet]
    public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(email);
        return Ok(user is null);
    }

    [HttpPost("login")]
    public async Task<IActionResult> PostLogin(LoginDTO loginDTO)
    {
        if (!ModelState.IsValid)
        {
            string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return Problem(errorMessage);
        }

        var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user is null)
            {
                return NoContent();
            }

            await _signInManager.SignInAsync(user, false);

            var authenticationResponse = _jwtService.CreateJwtToken(user);
            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpiration = authenticationResponse.RefreshTokenExpiration;
            await _userManager.UpdateAsync(user);

            return Ok(authenticationResponse);
        }

        return Problem("Invalid email or password");
    }

    [HttpGet("logout")]
    public async Task<ActionResult<ApplicationUser>> GetLogout()
    {
        await _signInManager.SignOutAsync();
        return NoContent();
    }

    [HttpPost("generate-new-jwt-token")]
    public async Task<IActionResult> GenerateNewAccessToken(TokenModel tokenModel)
    {
        if (tokenModel is null)
        {
            return BadRequest("Invalid client request");
        }

        ClaimsPrincipal? principal = _jwtService.GetPrincipalFromJwtToken(tokenModel.Token);

        if (principal is null)
        {
            return BadRequest("Invalid jwt access token");
        }

        string? email = principal.FindFirstValue(ClaimTypes.Email);
        ApplicationUser? user = await _userManager.FindByEmailAsync(email);

        if (user is null || user.RefreshToken != tokenModel.RefreshToken || user.RefreshTokenExpiration <= DateTime.Now)
        {
            return BadRequest("Invalid refresh token");
        }

        AuthenticationResponse response = _jwtService.CreateJwtToken(user);
        user.RefreshToken = response.RefreshToken;
        user.RefreshTokenExpiration = response.RefreshTokenExpiration;
        await _userManager.UpdateAsync(user);

        return Ok(response);
    }
}