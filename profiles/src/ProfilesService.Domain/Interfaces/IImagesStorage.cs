using FluentResults;
using ProfilesService.Domain.ValueObjects;

namespace ProfilesService.Domain.Interfaces;

public interface IImagesStorage
{
    Task<Result<ImageRef>> LoadAsync(Stream stream);
}