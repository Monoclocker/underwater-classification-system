using FluentResults;
using ProfilesService.Errors;

namespace ProfilesService.API;

public sealed record ApiResponse(string Code, string Message)
{
    public static ApiResponse FromResult(Result result)
    {
        IError error = result.Errors.First();
        
        return new ApiResponse(error.GetErrorCode(), error.Message);
    }
}