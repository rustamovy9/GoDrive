using System.ComponentModel.DataAnnotations;

namespace Application.DTO_s;

public class LoginRequest
{
    [Required]
    [MaxLength(40), MinLength(4)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20), MinLength(4)]
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    [Required]
    [MaxLength(40), MinLength(4)]
    public string UserName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(40), MinLength(4)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(40), MinLength(4)]
    public string LastName { get; set; } = string.Empty;
    
    [Required] [DataType(DataType.DateTime)] public DateTimeOffset DateOfBirth { get; set; }

    [Required] [Phone] public string PhoneNumber { get; set; } = string.Empty;

    [Required] [EmailAddress] public string EmailAddress { get; set; } = string.Empty;

    [Required]
    [MaxLength(20), MinLength(4)]
    public string Password { get; set; } = string.Empty;

    [Required] [Compare(nameof(Password))] public string ConfirmPassword { get; set; } = string.Empty;
}

public class ChangePasswordRequest
{
    [Required]
    [MaxLength(20), MinLength(4)]
    public string OldPassword { get; set; } = string.Empty;

    [Required]
    [MaxLength(20), MinLength(4)]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(NewPassword))]
    public string ConfirmPassword { get; set; } = string.Empty;
}