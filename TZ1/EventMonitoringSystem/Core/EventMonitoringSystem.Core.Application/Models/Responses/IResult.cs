namespace EventMonitoringSystem.Core.Application.Models.Responses;

public interface IResult
{
    bool IsSucceeded { get; set; }
}

public interface IResult<out T> : IResult
{
    T Data { get; }
}
