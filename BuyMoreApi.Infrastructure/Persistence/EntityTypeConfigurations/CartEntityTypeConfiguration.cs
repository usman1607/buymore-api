using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BuyMoreApi.Domain.Entities;
using BuyMoreApi.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BuyMoreApi.Infrastructure.Persistence.EntityTypeConfigurations
{
    public class CartEntityTypeConfiguration: IEntityTypeConfiguration<Cart>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("carts");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.UserId)
                .HasColumnName("user_id")
                .HasColumnType("uuid")
                .IsRequired();

            builder.HasOne(c => c.User)
                   .WithOne(u => u.Cart)
                   .HasForeignKey<Cart>(c => c.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(c => c.UserId)
                   .IsUnique();

            builder.Property(c => c.Items)
                .HasJsonbConversion();

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