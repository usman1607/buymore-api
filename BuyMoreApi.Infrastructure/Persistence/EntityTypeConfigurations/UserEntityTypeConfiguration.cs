using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyMoreApi.Domain.Entities;
using BuyMoreApi.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BuyMoreApi.Infrastructure.Persistence.EntityTypeConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.FirstName)
                .HasColumnName("first_name")
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(u => u.LastName)
                .HasColumnName("last_name")
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(u => u.ProfilePictureUrl)
                .HasColumnName("profile_picture_url")
                .HasColumnType("varchar(255)");

            builder.Property(u => u.EncryptedPassword)
                .HasColumnName("encrypted_password")
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder.Property(u => u.WalletBalance)
                .HasColumnName("wallet_balance")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
                
            builder.Property(u => u.Role)
                .HasColumnName("role")
                .HasColumnType("varchar(50)")
                .HasConversion<EnumToStringConverter<Role>>()
                .IsRequired();

            builder.Property(u => u.PhoneNumber)
                .HasColumnName("phone_number")
                .HasColumnType("varchar(20)");
            
            builder.Property(u => u.Address)
                .HasColumnName("address")
                .HasColumnType("varchar(255)");

            builder.HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);      

            builder.Property(i => i.CreatedDate)
                .HasColumnName("created_date")
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            builder.Property(i => i.UpdatedDate)
                .HasColumnName("updated_date")
                .HasColumnType("timestamp with time zone");

            builder.Property(i => i.IsDeleted)
                .HasColumnName("is_deleted")
                .HasColumnType("boolean")
                .IsRequired();

            builder.Property(i => i.CreatedBy)
                .HasColumnName("created_by")
                .HasColumnType("varchar(100)")
                .IsRequired();
            
            builder.Property(i => i.UpdatedBy)
                .HasColumnName("updated_by")
                .HasColumnType("varchar(100)");      
        }
    }
}