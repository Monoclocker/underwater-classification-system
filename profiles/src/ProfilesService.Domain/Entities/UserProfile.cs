using ProfilesService.Domain.ValueObjects;

namespace ProfilesService.Domain.Entities;

public sealed class UserProfile
{
    public Guid Id { get; private init; }
    public UserPersonalInformation PersonalInformation { get; private set; }
    
    public ImageRef? Image { get; set; }
    
    #region EF
    
    #pragma warning disable
    private UserProfile() {}
    #pragma warning restore

    public List<OrganizationMember> OrganizationMembersNavigation { get; private init; } = [];
    
    #endregion
    
    private UserProfile(Guid id, UserPersonalInformation personalInformation)
    {
        Id = id;
        PersonalInformation = personalInformation;
    }

    public void UpdateShownName(string newShownName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newShownName);
        
        PersonalInformation = PersonalInformation with { ShownUserName = newShownName };
    }
    
    public void UpdateFirstName(string newFirstName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newFirstName);
        
        PersonalInformation = PersonalInformation with { FirstName = newFirstName };
    }

    public void UpdateLastName(string newLastName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newLastName);
        
        PersonalInformation = PersonalInformation with { LastName = newLastName };
    }

    public void UpdateEmail(string newEmail)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newEmail);
        
        PersonalInformation = PersonalInformation with { Email = newEmail };
    }

    public static UserProfile Create(Guid id, UserPersonalInformation information) => new(id, information);
}