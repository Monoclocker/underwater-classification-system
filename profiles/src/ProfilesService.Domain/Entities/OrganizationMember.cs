namespace ProfilesService.Domain.Entities;

public sealed class OrganizationMember
{
    public Guid MemberId { get; private init; }
    public bool IsOwner { get; private set; }

    #region EF
    
    #pragma warning disable
    private OrganizationMember() {}
    #pragma warning restore
    
    public UserProfile MemberNavigation { get; private set; } = null!;
    public Organization OrganizationNavigation { get; private set; } = null!;
    
    #endregion

    private OrganizationMember(Guid memberId, bool isOwner)
    {
        MemberId = memberId;
        IsOwner = isOwner;
    }

    public void PromoteToOwner() => IsOwner = true;
    
    public void DemoteFromOwner() => IsOwner = false;

    public static OrganizationMember CreateAsOwner(Guid memberId) => new(memberId, true);

    public static OrganizationMember CreateAsRegularMember(Guid memberId) => new(memberId, false);
}