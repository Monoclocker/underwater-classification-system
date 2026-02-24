using FluentResults;
using ProfilesService.Domain.ValueObjects;
using ProfilesService.Errors.DomainErrors;

namespace ProfilesService.Domain.Entities;

public sealed class Organization
{
    private readonly List<OrganizationMember> _organizationMembers = [];
    
    public Guid Id { get; private init; }
    
    public OrganizationInformation Information { get; private set; }
    
    public ImageRef? Image { get; private set; }
    
    public IReadOnlyList<OrganizationMember> OrganizationMembers => _organizationMembers.AsReadOnly();
    
    #region EF
    #pragma warning disable
    private Organization() {}
    #pragma warning restore
    #endregion

    private Organization(Guid id, Guid ownerId, OrganizationInformation information)
    {
        Id = id;
        Information = information;
        
        _organizationMembers.Add(OrganizationMember.CreateAsOwner(ownerId));
    }

    public Result UpdateOrganizationName(Guid initiatorId, string newOrganizationName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newOrganizationName);
        
        if (!IsMemberOwner(initiatorId))
            return Result.Fail(new OwnershipError());
        
        Information = Information with { OrganizationName = newOrganizationName };

        return Result.Ok();
    }

    public Result UpdateOrganizationEmail(Guid initiatorId, string newOrganizationEmail)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newOrganizationEmail);
        
        if (!IsMemberOwner(initiatorId))
            return Result.Fail(new OwnershipError());
        
        Information = Information with { Email = newOrganizationEmail };

        return Result.Ok();
    }

    public Result AddMember(Guid newMemberId)
    {
        if (IsMemberExists(newMemberId)) 
            return Result.Fail(new MemberExistsError());
        
        _organizationMembers.Add(OrganizationMember.CreateAsRegularMember(newMemberId));
        
        return Result.Ok();
    }

    public Result RemoveMember(Guid memberId)
    {
        var member = GetOrganizationMember(memberId);

        if (member is null)
            return Result.Fail(new MemberNotFoundError());

        if (IsMemberOwner(member) && _organizationMembers.Count != 1)
            return Result.Fail(new OwnerRemovalError());
        
        _organizationMembers.Remove(member);
        
        return Result.Ok();
    }
    
    public Result TransferOwnership(Guid initiatorId, Guid newOwnerId)
    {
        var oldOwner = GetOrganizationMember(initiatorId);
        var newOwner = GetOrganizationMember(newOwnerId);
        
        if (oldOwner is null) 
            return Result.Fail(new MemberNotFoundError());
        
        if (newOwner is null) 
            return Result.Fail(new MemberNotFoundError());
        
        if (!IsMemberOwner(oldOwner))
            return Result.Fail(new OwnershipError());

        if (initiatorId == newOwnerId)
            return Result.Fail(new SelfTransferError());
        
        oldOwner.DemoteFromOwner();
        newOwner.PromoteToOwner();
        
        return Result.Ok();
    }

    public Result MakePublic(Guid initiatorId)
    {
        if (!IsMemberOwner(initiatorId))
            return Result.Fail(new OwnershipError());

        Information = Information with { IsPublic = true };

        return Result.Ok();
    }
    
    public Result MakePrivate(Guid initiatorId)
    {
        if (!IsMemberOwner(initiatorId))
            return Result.Fail(new OwnershipError());

        Information = Information with { IsPublic = false };

        return Result.Ok();
    }

    public Result UpdateImage(Guid initiatorId, ImageRef? newImage)
    {
        if (!IsMemberOwner(initiatorId))
            return Result.Fail(new OwnershipError());

        Image = newImage;
        
        return Result.Ok();
    }  
    
    private bool IsMemberExists(Guid memberId) => GetOrganizationMember(memberId) is not null;
    
    private OrganizationMember? GetOrganizationMember(Guid memberId) =>
        _organizationMembers.FirstOrDefault(m => m.MemberId == memberId);

    private bool IsMemberOwner(Guid memberId)
    {
        var member = GetOrganizationMember(memberId);

        return IsMemberOwner(member);
    }

    private bool IsMemberOwner(OrganizationMember? member) => member is { IsOwner: true };


    public static Organization Create(Guid ownerId, OrganizationInformation information)
    {
        return new(
            id: Guid.NewGuid(),
            ownerId: ownerId,
            information);
    } 
}
