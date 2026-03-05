namespace VeryMinimalAPI.Common.API.Result;

public record DataResult<T>(T? Data)
{
    public IEnumerable<string>? Errors { get; set; }
}