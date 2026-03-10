using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProfilesService.Domain.Entities;

namespace ProfilesService.Infrastructure.Database.Configurations;

internal sealed class ProfilesConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.PersonalInformation,
            navigationBuilder =>
            {
                navigationBuilder
                    .Property(x => x.ShownUserName)
                    .HasColumnName("shown_name");

                navigationBuilder
                    .HasIndex(x => x.ShownUserName)
                    .IsUnique();
                
                navigationBuilder
                    .Property(x => x.FirstName)
                    .HasColumnName("first_name");
                
                navigationBuilder
                    .Property(x => x.LastName)
                    .HasColumnName("last_name");
            });
        
        builder.OwnsOne(x => x.Image, 
            navigationBuilder => navigationBuilder
                .Property(x => x.Link)
                .HasColumnName("image_link"));

        
    }
}