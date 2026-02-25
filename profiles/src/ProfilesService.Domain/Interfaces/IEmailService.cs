using FluentResults;

namespace ProfilesService.Domain.Interfaces;

public interface IEmailService
{
    Task<Result> UpdateEmailAsync(Guid userId, string newEmail);
    Task<Result> RequestVerificationEmailAsync(Guid userId);
}