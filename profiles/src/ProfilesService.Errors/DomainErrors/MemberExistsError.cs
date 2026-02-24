namespace ProfilesService.Errors.DomainErrors;

public sealed class MemberExistsError() : ErrorWithStatus(
    errorCode: "MemberExistsError",
    message: "Member already exists in target organization");