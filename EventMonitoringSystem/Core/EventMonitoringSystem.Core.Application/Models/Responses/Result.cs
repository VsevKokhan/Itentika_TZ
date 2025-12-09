namespace EventMonitoringSystem.Core.Application.Models.Responses;

public class Result : IResult
{
    public Result()
    {
    }
    
    public bool IsSucceeded { get; set; }

    public static IResult Fail()
    {
        return new Result { IsSucceeded = false };
    }
    
    public static Task<IResult> FailAsync()
    {
        return Task.FromResult(Fail());
    }

    public static IResult Success()
    {
        return new Result { IsSucceeded = true };
    }
    
    public static Task<IResult> SuccessAsync()
    {
        return Task.FromResult(Success());
    }
    
}

public class ErrorResult<T> : Result<T>
{
    public string Source { get; set; }

    public string Exception { get; set; }

    public int ErrorCode { get; set; }
    public string StackTrace { get; set; }
}

public class Result<T> : Result, IResult<T>
{
    public Result()
    {
    }

    public T Data { get; set; }

    public new static Result<T> Fail()
    {
        return new() { IsSucceeded = false };
    }
    
    public new static Task<Result<T>> FailAsync()
    {
        return Task.FromResult(Fail());
    }
    
    public static ErrorResult<T> ReturnError()
    {
        return new() { IsSucceeded = false, ErrorCode = 500 };
    }

    public static Task<ErrorResult<T>> ReturnErrorAsync()
    {
        return Task.FromResult(ReturnError());
    }
    
    public static Result<T> Success(T data)
    {
        return new() { IsSucceeded = true, Data = data };
    }

    public static Task<Result<T>> SuccessAsync(T data)
    {
        return Task.FromResult(Success(data));
    }

}