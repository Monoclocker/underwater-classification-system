using FluentResults;

namespace ProfilesService.Errors;

public abstract class ErrorWithStatus : Error
{
    protected ErrorWithStatus(string message, string errorCode)
    {
        Message = message;
        Metadata.Add("ErrorCode", errorCode);
    }
}

public static class ErrorExtensions
{
    private const string DefaultErrorCode = "unknown";
    
    extension(IError error)
    {
        public string GetErrorCode() => error.Metadata["ErrorCode"].ToString() ?? DefaultErrorCode;
    }
}