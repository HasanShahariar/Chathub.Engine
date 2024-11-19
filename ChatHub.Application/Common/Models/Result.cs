

using ChatHub.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ChatHub.Application.Common.Models;

public interface IResult
{
    ICollection<string> Messages { get; }
    bool Succeeded { get; set; }
    bool HasError { get; set; }
}

public class Result : IResult
{
    public Result()
    {
        this.Messages = new HashSet<string>();
    }
    public ICollection<string> Messages { get; private set; }
    public bool HasError { get; set; }
    public bool Succeeded { get; set; }

    public static IResult Fail()
    {
        return new Result
        {
            Succeeded = false
        };
    }

    public static Task<IResult> FailAsync()
    {
        return Task.FromResult(Fail());
    }
    public static IResult Success()
    {
        return new Result
        {
            Succeeded = true
        };
    }

    public static Task<IResult> SuccessAsync()
    {
        return Task.FromResult(Success());
    }
}

public static class QueryableExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken) where T : class
    {
        CustomExtensions.IfNull(source, nameof(source));
        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        pageSize = pageSize == 0 ? 10 : pageSize;
        long count = await source.LongCountAsync();
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return PaginatedResult<T>.Success(items, count, pageNumber, pageSize);
    }
}


public class ListResult<T> : Result<IList<T>>
{
    public long Count { get; set; }
}
public class Result<T> : Result
{
    public T Data { get; set; }
}

public class PaginatedResult<T> : Result
{
    public List<T> Data { get; set; }

    public int Page { get; set; }

    public int TotalPages { get; set; }

    public long TotalCount { get; set; }

    public bool HasPreviousPage => Page > 1;

    public bool HasNextPage => Page < TotalPages;

    public PaginatedResult(List<T> data)
    {
        Data = data;
    }

    public PaginatedResult(bool succeeded, List<T> data = null, List<string> messages = null, long count = 0L, int page = 1, int pageSize = 10)
    {
        Data = data;
        Page = page;
        base.Succeeded = succeeded;
        TotalPages = (int)Math.Ceiling((double)count / (double)pageSize);
        TotalCount = count;
    }

    public static PaginatedResult<T> Failure(List<string> messages)
    {
        return new PaginatedResult<T>(succeeded: false, null, messages, 0L);
    }

    public static PaginatedResult<T> Success(List<T> data, long count, int page, int pageSize)
    {
        return new PaginatedResult<T>(succeeded: true, data, null, count, page, pageSize);
    }
}



