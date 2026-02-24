namespace ProfilesService.Errors.DomainErrors;

public sealed class MemberNotFoundError(): ErrorWithStatus(
    errorCode: "MemberNotFoundError",
    message: "Organization member not found");