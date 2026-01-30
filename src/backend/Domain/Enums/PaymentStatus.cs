namespace Domain.Enums;

public enum PaymentStatus
{
    NotRequired = 0,     // платёж через платформу не нужен
    Pending = 1,         // ожидается договорённость
    AgreedOffline = 2,   // договорились вне системы
    PaidOffline = 3     // оплачено напрямую владельцу
}