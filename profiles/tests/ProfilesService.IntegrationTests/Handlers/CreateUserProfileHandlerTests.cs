using FluentResults;
using Microsoft.EntityFrameworkCore;
using ProfilesService.Domain.Entities;
using ProfilesService.Errors.HandlersErrors;
using ProfilesService.Infrastructure.Database;
using ProfilesService.Handlers.Commands.CreateUserProfile;
using ProfilesService.IntegrationTests.Fixtures;

namespace ProfilesService.IntegrationTests.Handlers;

public sealed class CreateUserProfileHandlerTests : IClassFixture<DatabaseFixture>, IAsyncDisposable
{
    private readonly ProfilesDbContext _context;
    private readonly CreateUserProfileCommandHandler _handler;
    
    public CreateUserProfileHandlerTests(DatabaseFixture fixture)
    {
        _context = fixture.BuildContext();
        _handler = new CreateUserProfileCommandHandler(_context);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnResultWithEntityAlreadyExistsError_IfUniqueViolationHappened()
    {
        CreateUserProfileCommand command = new(
            Guid.NewGuid(),
            "test", 
            "Test", 
            Guid.NewGuid().ToString());

        var profile = UserProfile.Create(command.UserId, command.GetPersonalInformation());

        _context.Add(profile);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();
        
        Result result = await _handler.HandleAsync(command);
        
        Assert.True(result.HasError<EntityAlreadyExistsError>());
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnOkResultAndAddNewProfileIntoDatabase()
    {
        CreateUserProfileCommand command = new(
            Guid.NewGuid(),
            "test",
            "Test",
            "test");
        
        Result result = await _handler.HandleAsync(command);
        
        Assert.True(result.IsSuccess);
        Assert.True(await _context.Profiles.AnyAsync(x => x.Id == command.UserId));
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}