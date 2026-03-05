namespace VeryMinimalAPI.Common.API.Result;

public record ProcessResult(bool IsSuccess, IEnumerable<string>? Messages)
{
    
}