namespace VeryMinimalAPI.Common.API.Result;

public record ListDataResult<T>(IEnumerable<T>? Data, int Total)
{
    public IEnumerable<string>? Errors { get; set; }
}