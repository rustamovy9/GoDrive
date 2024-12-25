namespace Application.DTO_s;

public interface IBaseReviewInfo
{
    public int Rating { get; init; }
    public string? Comment { get; init; }
}
public readonly record struct ReviewReadInfo(
    int UserId,
    int CarId,
    int Rating,
    string? Comment,
    DateTime ReviewDate,
    int Id) : IBaseReviewInfo;

public readonly record struct ReviewUpdateInfo(
    int Rating,
    string? Comment) : IBaseReviewInfo;
public readonly record struct ReviewCreateInfo(
    int UserId,
    int CarId,
    int Rating,
    string? Comment) : IBaseReviewInfo;