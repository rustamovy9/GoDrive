using Domain.Common;

namespace Application.Filters;

public record RoleFilter(
    string? Name) : BaseFilter;
