using Application.DTO_s;
using Domain.Entities;

namespace Application.Extensions.Mappers;

public static class ReviewMapper
{
    public static ReviewReadInfo ToRead(this Review review)
    {
        return new ReviewReadInfo(
            Id: review.Id,
            CarId: review.CarId,
            Rating: review.Rating,
            Comment: review.Comment,
            UserName: review.User.UserName,
            CreatedAt: review.CreatedAt
        );
    }

    public static Review ToEntity(this ReviewCreateInfo createInfo,int userId)
    {
        return new Review
        {
            UserId = userId,
            CarId = createInfo.CarId,
            Rating = createInfo.Rating,
            Comment = createInfo.Comment,
        };
    }

    public static Review ToEntity(this Review entity, ReviewUpdateInfo updateInfo)
    {
        if (updateInfo.Comment is not null)
            entity.Comment = updateInfo.Comment;
        
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        entity.Version++;
        
        return entity;
    }
}