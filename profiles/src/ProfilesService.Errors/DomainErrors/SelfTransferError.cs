namespace ProfilesService.Errors.DomainErrors;

public class SelfTransferError() : ErrorWithStatus(
    errorCode: "SelfTransferError",
    message: "New owner must be different from current owner");
    