using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProfilesService.Domain.Entities;

namespace ProfilesService.Infrastructure.Database.Configurations;

internal sealed class OrganizationMembersConfiguration : IEntityTypeConfiguration<OrganizationMember>
{
    public void Configure(EntityTypeBuilder<OrganizationMember> builder)
    {
        builder.Property<Guid>("organization_id");
        
        builder.HasKey(nameof(OrganizationMember.MemberId), "organization_id");
        
        builder.HasOne(x => x.OrganizationNavigation)
            .WithMany(x => x.OrganizationMembers)
            .HasForeignKey("organization_id");
        
        builder.HasOne(x => x.MemberNavigation)
            .WithMany(x => x.OrganizationMembersNavigation)
            .HasForeignKey(x => x.MemberId);
    }
}