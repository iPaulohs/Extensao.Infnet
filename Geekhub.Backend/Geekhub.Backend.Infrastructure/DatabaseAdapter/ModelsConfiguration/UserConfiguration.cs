using Geekhub.Backend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Geekhub.Backend.Infrastructure.DatabaseAdapter.ModelsConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.ComplexProperty(x => x.Email, email =>
        {
            email.Property(x => x.Value)
                .HasColumnName("Email")
                .IsRequired();

            email.Property(x => x.Verified)
                .HasColumnName("EmailVerified")
                .IsRequired();

            email.Property(x => x.VerifiedAt)
                .HasColumnName("EmailVerifiedAt");
        });

        builder.ComplexProperty(x => x.Password, password =>
        {
            password.Property(x => x.Hash)
                .HasColumnName("PasswordHash")
                .IsRequired();
        });
    }
}
