using FluentResults;
using ProfilesService.Domain.Entities;
using ProfilesService.Domain.ValueObjects;
using ProfilesService.Errors.DomainErrors;

namespace ProfilesService.UnitTests;

public sealed class OrganizationUnitTests
{
    [Fact]
    public void OnCreation_UserShouldBeOwner()
    {
        Guid userId = Guid.NewGuid();
        var defaultOrganizationInformation = CreateOrganizationInformation();

        var organization = Organization.Create(userId, defaultOrganizationInformation);

        OrganizationMember? requiredUser = organization
            .OrganizationMembers
            .FirstOrDefault(x => x.MemberId == userId);
        
        Assert.True(requiredUser is { IsOwner: true });
    }

    [Fact]
    public void UpdateOrganizationName_ShouldReturnOwnershipError_IfInitiatorIsNotAnOwner()
    {
        Guid ownerId = Guid.NewGuid();
        Guid initiatorId = Guid.NewGuid();
        
        var organization = Organization.Create(ownerId, CreateOrganizationInformation());
        
        Result result = organization.UpdateOrganizationName(initiatorId, "newName");
        
        Assert.True(result.HasError<OwnershipError>());
    }

    [Fact]
    public void UpdateOrganizationName_ShouldUpdateNameAndReturnOk_IfInitiatorIsOwner()
    {
        Guid ownerId = Guid.NewGuid();
        string newName = "newOrganizationName";
        
        var organization = Organization.Create(ownerId, CreateOrganizationInformation());
        
        Result result = organization.UpdateOrganizationName(ownerId, newName);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(newName, organization.Information.OrganizationName);
    }

    [Fact]
    public void UpdateOrganizationEmail_ShouldReturnOwnershipError_IfInitiatorIsNotAnOwner()
    {
        Guid ownerId = Guid.NewGuid();
        Guid initiatorId = Guid.NewGuid();
        
        var organization = Organization.Create(ownerId, CreateOrganizationInformation());
        
        Result result = organization.UpdateOrganizationEmail(initiatorId, "newEmail");
        
        Assert.True(result.HasError<OwnershipError>());
    }

    [Fact]
    public void UpdateOrganizationEmail_ShouldUpdateEmailAndReturnOk_IfInitiatorIsOwner()
    {
        Guid ownerId = Guid.NewGuid();
        string newEmail = "newEmail@email.com";
        
        var organization = Organization.Create(ownerId, CreateOrganizationInformation());
        
        Result result = organization.UpdateOrganizationEmail(ownerId, newEmail);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(newEmail, organization.Information.Email);
    }

    [Fact]
    public void AddMember_ShouldReturnMemberExistsError_IfMemberAlreadyExists()
    {
        Guid ownerId = Guid.NewGuid();

        var organization = Organization.Create(ownerId, CreateOrganizationInformation());
        
        Result result = organization.AddMember(ownerId);
        
        Assert.True(result.HasError<MemberExistsError>());
    }

    [Fact]
    public void AddMember_ShouldReturnOkAndAddNewRegularMember_IfMemberDoesNotExists()
    {
        Guid ownerId = Guid.NewGuid();
        Guid newMemberId = Guid.NewGuid();
        
        var organization = Organization.Create(ownerId, CreateOrganizationInformation());
        
        Result result = organization.AddMember(newMemberId);
        
        OrganizationMember? requiredUser = organization
            .OrganizationMembers
            .FirstOrDefault(m => m.MemberId == newMemberId);
        
        Assert.True(result.IsSuccess);
        Assert.True(requiredUser is { IsOwner: false });
    }

    [Fact]
    public void RemoveMember_ShouldReturnMemberNotFound_IfMemberDoesNotExists()
    {
        Guid ownerId = Guid.NewGuid();
        Guid removeMemberId = Guid.NewGuid();
        
        var organization = Organization.Create(ownerId, CreateOrganizationInformation());
        
        Result result = organization.RemoveMember(removeMemberId);
        
        Assert.True(result.HasError<MemberNotFoundError>());
    }

    [Fact]
    public void RemoveMember_ShouldReturnOwnerRemovalError_IfOwnerWantToLeftWithMembersInOrganization()
    {
        Guid ownerId = Guid.NewGuid();
        Guid anotherMemberId = Guid.NewGuid();
        
        var organization = Organization.Create(ownerId, CreateOrganizationInformation());
        organization.AddMember(anotherMemberId);
        
        Result result = organization.RemoveMember(ownerId);
        
        Assert.True(result.HasError<OwnerRemovalError>());
    }

    [Fact]
    public void RemoveMember_ShouldReturnOkAndRemoveMember()
    {
        Guid ownerId = Guid.NewGuid();
        Guid removeMemberId = Guid.NewGuid();
        
        var organization = Organization.Create(ownerId, CreateOrganizationInformation());
        organization.AddMember(removeMemberId);
        
        Result result = organization.RemoveMember(removeMemberId);

        OrganizationMember? requiredMember = organization
            .OrganizationMembers
            .FirstOrDefault(m => m.MemberId == removeMemberId);
        
        Assert.True(result.IsSuccess);
        Assert.Null(requiredMember);
    }

    [Fact]
    public void TransferOwnership_ShouldReturnMemberNotFoundError_IfInitiatorWasNotFound()
    {
        Guid ownerId = Guid.NewGuid();
        Guid initiatorId = Guid.NewGuid();
        
        var organization = Organization.Create(ownerId, CreateOrganizationInformation());
        
        Result result = organization.TransferOwnership(initiatorId, Guid.NewGuid());
        
        Assert.True(result.HasError<MemberNotFoundError>());
    }

    [Fact]
    public void TransferOwnership_ShouldReturnMemberNotFoundError_IfNewOwnerWasNotFound()
    {
        Guid ownerId = Guid.NewGuid();
        Guid newOwnerId = Guid.NewGuid();
        
        var organization = Organization.Create(ownerId, CreateOrganizationInformation());
        
        Result result = organization.TransferOwnership(ownerId, newOwnerId);
        
        Assert.True(result.HasError<MemberNotFoundError>());
    }

    [Fact]
    public void TransferOwnership_ShouldReturnOwnershipError_IfInitiatorIsNotOwner()
    {
        Guid ownerId = Guid.NewGuid();
        Guid initiatorId = Guid.NewGuid();
        
        var organization = Organization.Create(ownerId, CreateOrganizationInformation());
        organization.AddMember(initiatorId);
        
        Result result = organization.TransferOwnership(initiatorId, ownerId);
        
        Assert.True(result.HasError<OwnershipError>());
    }

    [Fact]
    public void TransferOwnership_ShouldReturnSelfTransferError_IfInitiatorIdEqualsToNewOwnerId()
    {
        Guid ownerId = Guid.NewGuid();

        var organization = Organization.Create(ownerId, CreateOrganizationInformation());
        
        Result result = organization.TransferOwnership(ownerId, ownerId);
        
        Assert.True(result.HasError<SelfTransferError>());
    }

    [Fact]
    public void TransferOwnership_ShouldReturnOkAndProceedCorrectOwnershipTransfer()
    {
        Guid ownerId = Guid.NewGuid();
        Guid newOwnerId = Guid.NewGuid();
        
        var organization = Organization.Create(ownerId, CreateOrganizationInformation());
        organization.AddMember(newOwnerId);
        
        Result result = organization.TransferOwnership(ownerId, newOwnerId);
        
        OrganizationMember? oldOwner = organization
            .OrganizationMembers
            .FirstOrDefault(x => x.MemberId == ownerId);

        OrganizationMember? newOwner = organization
            .OrganizationMembers
            .FirstOrDefault(x => x.MemberId == newOwnerId);
        
        Assert.True(result.IsSuccess);
        Assert.True(oldOwner is { IsOwner: false });
        Assert.True(newOwner is { IsOwner: true });
    }

    [Fact]
    public void MakePublic_ShouldReturnOwnershipError_IfInitiatorIsNotAnOwner()
    {
        Guid ownerId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        
        var organization = Organization.Create(ownerId, CreateOrganizationInformation());
        
        Result result = organization.MakePublic(userId);
        
        Assert.True(result.HasError<OwnershipError>());
    }

    [Fact]
    public void MakePublic_ShouldReturnOkAndMakeOrganizationPublic()
    {
        Guid ownerId = Guid.NewGuid();
        
        var organization = Organization.Create(ownerId, CreateOrganizationInformation(isPublic: false));
        
        Result result = organization.MakePublic(ownerId);
        
        Assert.True(result.IsSuccess);
        Assert.True(organization.Information.IsPublic);
    }

    [Fact]
    public void MakePrivate_ShouldReturnOwnershipError_IfInitiatorIsNotAnOwner()
    {
        Guid ownerId = Guid.NewGuid();
        
        var organization = Organization.Create(ownerId, CreateOrganizationInformation());
        
        Result result = organization.MakePrivate(Guid.NewGuid());
        
        Assert.True(result.HasError<OwnershipError>());
    }

    [Fact]
    public void MakePrivate_ShouldReturnOkAndMakeOrganizationPrivate()
    {
        Guid ownerId = Guid.NewGuid();
        
        var organization = Organization.Create(ownerId, CreateOrganizationInformation(isPublic: true));
        
        Result result = organization.MakePrivate(ownerId);
        
        Assert.True(result.IsSuccess);
        Assert.False(organization.Information.IsPublic);
    }
    
    [Fact]
    public void UpdateImage_ShouldReturnOwnershipError_IfInitiatorIsNotAnOwner()
    {
        Guid ownerId = Guid.NewGuid();
        
        var organization = Organization.Create(ownerId, CreateOrganizationInformation());
        
        Result result = organization.UpdateImage(Guid.NewGuid(), new ImageRef("test"));
        
        Assert.True(result.HasError<OwnershipError>());
    }

    [Fact]
    public void UpdateImage_ShouldSetImageRef_IfImageRefProvided()
    {
        Guid ownerId = Guid.NewGuid();
        
        var organization = Organization.Create(ownerId, CreateOrganizationInformation());
        
        Result result = organization.UpdateImage(ownerId, new ImageRef("test"));
        
        Assert.True(result.IsSuccess);
        Assert.True(organization.Image is { Link: "test" });
    }

    [Fact]
    public void UpdateImage_ShouldRemoveImageRef_IfNullWasProvided()
    {
        Guid ownerId = Guid.NewGuid();

        var organization = Organization.Create(ownerId, CreateOrganizationInformation());

        organization.UpdateImage(ownerId, new ImageRef("test"));

        Result result = organization.UpdateImage(ownerId, null);
        
        Assert.True(result.IsSuccess);
        Assert.Null(organization.Image);
    }

    private static OrganizationInformation CreateOrganizationInformation(
        string organizationName = "testOrganization",
        string email = "test@email.com",
        bool isPublic = true)
    {
        return new OrganizationInformation(
            organizationName,
            email,
            isPublic);
    }
}