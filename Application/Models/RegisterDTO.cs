using System.ComponentModel.DataAnnotations;

namespace Application.Models;

public class RegisterDTO
{
    [Required(ErrorMessage = "Email cannot be blank")]
    [EmailAddress(ErrorMessage = "Email should be in a proper format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password cannot be blank")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirmation password cannot be blank")]
    [Compare("Password", ErrorMessage = "Password and confirm password must be same")]
    public string ConfirmPassword { get; set; } = string.Empty;
}