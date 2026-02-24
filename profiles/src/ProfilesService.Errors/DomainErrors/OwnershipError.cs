namespace ProfilesService.Errors.DomainErrors;

public sealed class OwnershipError() : ErrorWithStatus(
    errorCode: "OwnershipError",
    message: "Initiator is not an owner of target organization");