using Application.DTO_s;
using Domain.Entities;

namespace Application.Extensions.Mappers;

public static class CategoryMap
{
    public static CategoryReadInfo ToRead(this Category category)
    {
        return new CategoryReadInfo(
            Id: category.Id,
            Name: category.Name);
    }
    
    public static Category ToEntity(this CategoryCreateInfo createInfo)
    {
        return new Category
        {
            Name = createInfo.Name
        };
    }

    public static Category ToEntity(
        this Category entity,
        CategoryUpdateInfo updateInfo)
    {
        if (updateInfo.Name is not null)
            entity.Name = updateInfo.Name;

        entity.Version++;
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        return entity;
    }
    
}