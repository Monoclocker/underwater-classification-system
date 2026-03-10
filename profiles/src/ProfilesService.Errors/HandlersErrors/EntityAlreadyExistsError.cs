namespace ProfilesService.Errors.HandlersErrors;

public sealed class EntityAlreadyExistsError() : ErrorWithStatus(
    errorCode: "EntityAlreadyExists",
    message: "Entity is already exists.");