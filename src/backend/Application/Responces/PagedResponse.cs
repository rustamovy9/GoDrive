using Domain.Common;

namespace Application.Responces;

public record PagedResponse<T> : BaseFilter
{
    public int TotalPages { get; init; }
    public int TotalRecords { get; init; }
    public T? Data { get; init; }

    private PagedResponse(int pageNumber, int pageSize, int totalRecords, T? data) : base(pageSize, pageNumber)
    {
        Data = data;
        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
    }

    public static PagedResponse<T> Create(int pageNumber, int pageSize, int totalRecords, T? data)
        => new(pageNumber, pageSize, totalRecords, data);
}