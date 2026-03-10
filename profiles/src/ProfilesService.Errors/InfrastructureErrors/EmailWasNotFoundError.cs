namespace ProfilesService.Errors.InfrastructureErrors;

public class EmailWasNotFoundError() : ErrorWithStatus(
    errorCode: "EmailWasNotFound",
    message: "Email for user was not found or is in incorrect state");