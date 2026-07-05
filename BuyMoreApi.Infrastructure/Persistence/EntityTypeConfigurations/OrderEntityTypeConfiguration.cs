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
    public class OrderEntityTypeConfiguration: IEntityTypeConfiguration<Order>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .ValueGeneratedOnAdd();

            builder.Property(o => o.UserId)
                .HasColumnName("user_id")
                .HasColumnType("uuid")
                .IsRequired();

            builder.Property(o => o.TotalAmount)
                .HasColumnName("total_amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.Status)
                .HasColumnName("status")
                .HasColumnType("varchar(50)")
                .HasConversion<EnumToStringConverter<OrderStatus>>()
                .IsRequired();

            builder.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Order>(o => o.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(o => o.Reference)
                .HasColumnName("reference")
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.HasIndex(o => o.Reference)
                .IsUnique();

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