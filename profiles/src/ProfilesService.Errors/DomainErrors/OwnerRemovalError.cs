namespace ProfilesService.Errors.DomainErrors;

public sealed class OwnerRemovalError() : ErrorWithStatus(
    errorCode: "OwnerRemovalError",
    message: "Cannot remove owner from organization when there are another members");