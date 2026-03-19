using Application.Contracts.Localization;
using Domain.Enums;

namespace Application.Localization;

public static class EnumLocalizationExtensions
{
    public static string ToLocalizedString(this CarStatus status, ITextLocalizer localizer)
        => localizer.Get($"Status.CarStatus.{status}");

    public static string ToLocalizedString(this BookingStatus status, ITextLocalizer localizer)
        => localizer.Get($"Status.BookingStatus.{status}");

    public static string ToLocalizedString(this PaymentStatus status, ITextLocalizer localizer)
        => localizer.Get($"Status.PaymentStatus.{status}");

    public static string ToLocalizedString(this DocumentVerificationStatus status, ITextLocalizer localizer)
        => localizer.Get($"Status.DocumentVerificationStatus.{status}");
}
