using System.Text.RegularExpressions;

namespace Application.Validations.Helpers;

public static class FluentValidationHelpers
{
    public static bool IsPhoneNumberValid(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber)) return false;

        // Normalize common separators while keeping leading + if present.
        var normalized = Regex.Replace(phoneNumber, @"[\s\-()]", "");
        return Regex.IsMatch(normalized, @"^\+?[1-9]\d{7,14}$");
    }
}
