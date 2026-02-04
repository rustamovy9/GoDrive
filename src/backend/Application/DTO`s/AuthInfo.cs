using System.ComponentModel.DataAnnotations;

namespace Application.DTO_s;

public class LoginRequest
{
    public string UserName { get; set; } = string.Empty;

   public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    public string UserName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public DateTimeOffset DateOfBirth { get; set; }

     public string PhoneNumber { get; set; } = string.Empty;

     public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;
}

public class ChangePasswordRequest
{
    public string OldPassword { get; set; } = string.Empty;

    public string NewPassword { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;
}