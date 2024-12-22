// namespace Application.DTO_s;
//
// public interface IBaseCarInfo
// {
//     public string Name { get; init; }
//     public string Code { get; init; }
// }
//
// public readonly record struct CarReadInfo(
//     string Name,
//     string Code,
//     Guid Id) : IBaseSpecializationInfo;
//
// public readonly record struct CarUpdateInfo(
//     string Name,
//     string Code,
//     bool IsActive) : IBaseCarInfo;
//
// public readonly record struct CarCreateInfo(
//     string Name,
//     string Code) : IBaseSpecializationInfo;