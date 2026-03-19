namespace Application.Localization;

public static class TextKeys
{
    public static class General
    {
        public const string ValidationExceptionTitle = "General.ValidationExceptionTitle";
        public const string UnexpectedErrorTitle = "General.UnexpectedErrorTitle";
    }

    public static class Auth
    {
        public const string LoginFailed = "Auth.LoginFailed";
        public const string UserRegistered = "Auth.UserRegistered";
        public const string UserNotRegistered = "Auth.UserNotRegistered";
        public const string AccountDeleted = "Auth.AccountDeleted";
        public const string AccountNotDeleted = "Auth.AccountNotDeleted";
        public const string UserDeleted = "Auth.UserDeleted";
        public const string UserNotDeleted = "Auth.UserNotDeleted";
        public const string PasswordUpdated = "Auth.PasswordUpdated";
        public const string PasswordNotUpdated = "Auth.PasswordNotUpdated";
        public const string UserIdNotFound = "Auth.UserIdNotFound";
    }

    public static class Ai
    {
        public const string NoCarsAvailable = "Ai.NoCarsAvailable";
        public const string NoCarsAvailableShort = "Ai.NoCarsAvailableShort";
        public const string Error = "Ai.Error";
    }

    public static class Errors
    {
        public const string NotFoundDefault = "Errors.NotFoundDefault";
        public const string BadRequestDefault = "Errors.BadRequestDefault";
        public const string ForbiddenDefault = "Errors.ForbiddenDefault";
        public const string AlreadyExistDefault = "Errors.AlreadyExistDefault";
        public const string ConflictDefault = "Errors.ConflictDefault";
        public const string InternalServerErrorDefault = "Errors.InternalServerErrorDefault";

        public const string PhoneExists = "Errors.PhoneExists";
        public const string UserNotFound = "Errors.UserNotFound";
        public const string RoleNotFound = "Errors.RoleNotFound";
        public const string UserHasRole = "Errors.UserHasRole";
        public const string UserMissingRole = "Errors.UserMissingRole";
        public const string RoleExists = "Errors.RoleExists";
        public const string SystemRoleDeleteForbidden = "Errors.SystemRoleDeleteForbidden";
        public const string ReviewNotFound = "Errors.ReviewNotFound";
        public const string ReviewOnlyAfterCompleted = "Errors.ReviewOnlyAfterCompleted";
        public const string ReviewAlreadyExists = "Errors.ReviewAlreadyExists";
        public const string InvalidCredentials = "Errors.InvalidCredentials";
        public const string UserAlreadyExists = "Errors.UserAlreadyExists";
        public const string InvalidBirthDate = "Errors.InvalidBirthDate";
        public const string PasswordIncorrect = "Errors.PasswordIncorrect";
        public const string PasswordsMismatch = "Errors.PasswordsMismatch";
        public const string CarPriceNotFound = "Errors.CarPriceNotFound";
        public const string CarNotFound = "Errors.CarNotFound";
        public const string CarPriceExists = "Errors.CarPriceExists";
        public const string RegistrationNumberExists = "Errors.RegistrationNumberExists";
        public const string CannotUpdateArchivedCar = "Errors.CannotUpdateArchivedCar";
        public const string OnlyBlockedCarDelete = "Errors.OnlyBlockedCarDelete";
        public const string BookingNotFound = "Errors.BookingNotFound";
        public const string InvalidBookingPeriod = "Errors.InvalidBookingPeriod";
        public const string CarAlreadyBooked = "Errors.CarAlreadyBooked";
        public const string CarPriceNotSet = "Errors.CarPriceNotSet";
        public const string InvalidBookingDuration = "Errors.InvalidBookingDuration";
        public const string UpdateOnlyPendingBookingInfo = "Errors.UpdateOnlyPendingBookingInfo";
        public const string InvalidBookingStatusTransition = "Errors.InvalidBookingStatusTransition";
        public const string ReasonRequired = "Errors.ReasonRequired";
        public const string DeleteOnlyPendingBooking = "Errors.DeleteOnlyPendingBooking";
        public const string LocationExists = "Errors.LocationExists";
        public const string NotificationNotFound = "Errors.NotificationNotFound";
        public const string AmountMustBeGreaterThanZero = "Errors.AmountMustBeGreaterThanZero";
        public const string BookingAlreadyPaid = "Errors.BookingAlreadyPaid";
        public const string PaymentNotFound = "Errors.PaymentNotFound";
        public const string PaymentCannotBeChanged = "Errors.PaymentCannotBeChanged";
        public const string DocumentNotFound = "Errors.DocumentNotFound";
        public const string DocumentAlreadyVerified = "Errors.DocumentAlreadyVerified";
        public const string ImageNotFound = "Errors.ImageNotFound";
        public const string CannotUploadImageForBlockedCar = "Errors.CannotUploadImageForBlockedCar";
        public const string MaxImagesExceeded = "Errors.MaxImagesExceeded";
        public const string CategoryNameRequired = "Errors.CategoryNameRequired";
        public const string CategoryExists = "Errors.CategoryExists";
        public const string CategoryNotFound = "Errors.CategoryNotFound";
        public const string CategoryNameExists = "Errors.CategoryNameExists";
        public const string RentalCompanyNotFound = "Errors.RentalCompanyNotFound";
        public const string LocationNotFound = "Errors.LocationNotFound";
        public const string OwnerAlreadyHasCompany = "Errors.OwnerAlreadyHasCompany";
        public const string CannotDeleteCompanyWithCars = "Errors.CannotDeleteCompanyWithCars";
    }

    public static class Notifications
    {
        public const string CarCreatedTitle = "Notifications.CarCreatedTitle";
        public const string CarCreatedMessage = "Notifications.CarCreatedMessage";
        public const string CarDeletedTitle = "Notifications.CarDeletedTitle";
        public const string CarDeletedMessage = "Notifications.CarDeletedMessage";
        public const string CarStatusUpdatedTitle = "Notifications.CarStatusUpdatedTitle";
        public const string CarStatusUpdatedMessage = "Notifications.CarStatusUpdatedMessage";
        public const string BookingCreatedTitle = "Notifications.BookingCreatedTitle";
        public const string BookingCreatedMessage = "Notifications.BookingCreatedMessage";
        public const string NewBookingTitle = "Notifications.NewBookingTitle";
        public const string NewBookingMessage = "Notifications.NewBookingMessage";
        public const string BookingStatusUpdatedTitle = "Notifications.BookingStatusUpdatedTitle";
        public const string BookingStatusUpdatedMessage = "Notifications.BookingStatusUpdatedMessage";
        public const string BookingUpdatedTitle = "Notifications.BookingUpdatedTitle";
        public const string BookingUpdatedMessage = "Notifications.BookingUpdatedMessage";
        public const string PaymentStatusUpdatedTitle = "Notifications.PaymentStatusUpdatedTitle";
        public const string PaymentStatusUpdatedMessage = "Notifications.PaymentStatusUpdatedMessage";
        public const string PaymentStatusUpdatedOwnerMessage = "Notifications.PaymentStatusUpdatedOwnerMessage";
        public const string PaymentCreatedTitle = "Notifications.PaymentCreatedTitle";
        public const string PaymentCreatedMessage = "Notifications.PaymentCreatedMessage";
        public const string NewPaymentTitle = "Notifications.NewPaymentTitle";
        public const string NewPaymentMessage = "Notifications.NewPaymentMessage";
        public const string DocumentSentTitle = "Notifications.DocumentSentTitle";
        public const string DocumentSentMessage = "Notifications.DocumentSentMessage";
        public const string NewDocumentTitle = "Notifications.NewDocumentTitle";
        public const string NewDocumentMessage = "Notifications.NewDocumentMessage";
        public const string DocumentVerifiedTitle = "Notifications.DocumentVerifiedTitle";
        public const string DocumentVerifiedMessage = "Notifications.DocumentVerifiedMessage";
    }

    public static class Validation
    {
        public const string CarIdGreaterThanZero = "Validation.CarIdGreaterThanZero";
        public const string BrandRequiredMax100 = "Validation.BrandRequiredMax100";
        public const string ModelRequiredMax100 = "Validation.ModelRequiredMax100";
        public const string RegistrationNumberRequiredMax50 = "Validation.RegistrationNumberRequiredMax50";
        public const string PickupLocationIdGreaterThanZero = "Validation.PickupLocationIdGreaterThanZero";
        public const string DropOffLocationIdGreaterThanZero = "Validation.DropOffLocationIdGreaterThanZero";
        public const string PickupAndDropOffDifferent = "Validation.PickupAndDropOffDifferent";
        public const string StartDateTimeNotPast = "Validation.StartDateTimeNotPast";
        public const string EndDateTimeInFuture = "Validation.EndDateTimeInFuture";
        public const string StartDateTimeBeforeEndDateTime = "Validation.StartDateTimeBeforeEndDateTime";
        public const string BookingDurationMin1Hour = "Validation.BookingDurationMin1Hour";
        public const string CommentMax500 = "Validation.CommentMax500";
        public const string YearValidProductionYear = "Validation.YearValidProductionYear";
        public const string CategoryIdGreaterThanZero = "Validation.CategoryIdGreaterThanZero";
        public const string LocationIdGreaterThanZero = "Validation.LocationIdGreaterThanZero";
        public const string RentalCompanyIdGreaterThanZero = "Validation.RentalCompanyIdGreaterThanZero";
        public const string CarImageRequired = "Validation.CarImageRequired";
        public const string FileValidImageUnder5Mb = "Validation.FileValidImageUnder5Mb";
        public const string AvailableFromNotPast = "Validation.AvailableFromNotPast";
        public const string AvailableToInFuture = "Validation.AvailableToInFuture";
        public const string AvailableFromBeforeAvailableTo = "Validation.AvailableFromBeforeAvailableTo";
        public const string AvailabilityDurationMin30Minutes = "Validation.AvailabilityDurationMin30Minutes";
        public const string PricePerDayGreaterThanZero = "Validation.PricePerDayGreaterThanZero";
        public const string DocumentTypeValid = "Validation.DocumentTypeValid";
        public const string DocumentImageRequired = "Validation.DocumentImageRequired";
        public const string CategoryNameRequiredMax100 = "Validation.CategoryNameRequiredMax100";
        public const string CategoryNameNotEmptyMax100 = "Validation.CategoryNameNotEmptyMax100";
        public const string CompanyNameRequiredMax200 = "Validation.CompanyNameRequiredMax200";
        public const string ContactInfoMax500 = "Validation.ContactInfoMax500";
        public const string CountryRequiredMax100 = "Validation.CountryRequiredMax100";
        public const string CityRequiredMax100 = "Validation.CityRequiredMax100";
        public const string LatitudeRange = "Validation.LatitudeRange";
        public const string LongitudeRange = "Validation.LongitudeRange";
        public const string RatingRange1To5 = "Validation.RatingRange1To5";
        public const string CommentNotEmptyMax500 = "Validation.CommentNotEmptyMax500";
        public const string OldPasswordRequired = "Validation.OldPasswordRequired";
        public const string OldPasswordMin6 = "Validation.OldPasswordMin6";
        public const string OldPasswordMax50 = "Validation.OldPasswordMax50";
        public const string NewPasswordRequired = "Validation.NewPasswordRequired";
        public const string NewPasswordMin6 = "Validation.NewPasswordMin6";
        public const string NewPasswordMax50 = "Validation.NewPasswordMax50";
        public const string NewPasswordDifferentFromOld = "Validation.NewPasswordDifferentFromOld";
        public const string ConfirmRequired = "Validation.ConfirmRequired";
        public const string PasswordsDoNotMatch = "Validation.PasswordsDoNotMatch";
        public const string LoginUserNameOrEmailRequired = "Validation.LoginUserNameOrEmailRequired";
        public const string PasswordRequired = "Validation.PasswordRequired";
        public const string PasswordMin6 = "Validation.PasswordMin6";
        public const string PasswordMax50 = "Validation.PasswordMax50";
        public const string UserNameValidChars = "Validation.UserNameValidChars";
        public const string FirstNameRequired = "Validation.FirstNameRequired";
        public const string FirstNameMax50 = "Validation.FirstNameMax50";
        public const string LastNameRequired = "Validation.LastNameRequired";
        public const string LastNameMax50 = "Validation.LastNameMax50";
        public const string DateOfBirthNotFuture = "Validation.DateOfBirthNotFuture";
        public const string EmailRequired = "Validation.EmailRequired";
        public const string EmailInvalid = "Validation.EmailInvalid";
        public const string PhoneNumberInvalid = "Validation.PhoneNumberInvalid";
        public const string PasswordNotSameAsUserName = "Validation.PasswordNotSameAsUserName";
        public const string AddressMax200 = "Validation.AddressMax200";
        public const string AvatarInvalidImage = "Validation.AvatarInvalidImage";
    }
}

