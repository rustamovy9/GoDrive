using Application.DTO_s;
using Domain.Entities;

namespace Application.Extensions.Mappers;

public static class ReviewMapper
{
    public static ReviewReadInfo ToRead(this Review review)
    {
        return new ReviewReadInfo(
            review.UserId,
            review.CarId,
            review.Rating,
            review.Comment,
            review.ReviewDate,
            review.Id
        );
    }

    public static Review ToEntity(this ReviewCreateInfo createInfo)
    {
        return new Review
        {
            UserId = createInfo.UserId,
            CarId = createInfo.CarId,
            Rating = createInfo.Rating,
            Comment = createInfo.Comment,
            ReviewDate = DateTime.UtcNow 
        };
    }

    public static Review ToEntity(this Review entity, ReviewUpdateInfo updateInfo)
    {
        entity.Rating = updateInfo.Rating;
        entity.Comment = updateInfo.Comment;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        entity.Version++;
        return entity;
    }
}