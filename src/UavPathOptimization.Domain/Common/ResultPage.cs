namespace UavPathOptimization.Domain.Common;

public class ResultPage<T>
{
    public int PageNumber { get; }

    public int PageSize { get; }

    public int TotalPages { get; }

    public int TotalCount { get; }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public IEnumerable<T> Items { get; }

    public ResultPage(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int) Math.Ceiling(totalCount / (double) pageSize);
        Items = items;
    }
}