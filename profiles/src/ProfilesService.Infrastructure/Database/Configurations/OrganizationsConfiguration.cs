using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProfilesService.Domain.Entities;

namespace ProfilesService.Infrastructure.Database.Configurations;

internal sealed class OrganizationsConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Information,
            navigationBuilder =>
            {
                navigationBuilder
                    .Property(x => x.IsPublic)
                    .HasColumnName("is_public");
                
                navigationBuilder
                    .Property(x => x.Email)
                    .HasColumnName("email");
                
                navigationBuilder
                    .Property(x => x.OrganizationName)
                    .HasColumnName("organization_name");
            });
        builder.OwnsOne(x => x.Image,
            navigationBuilder => navigationBuilder
                .Property(x => x.Link)
                .HasColumnName("image_link"));
        
        builder.Navigation(x => x.OrganizationMembers)
            .HasField("_organizationMembers")
            .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);
    }
}