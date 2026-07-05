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
    public class PaymentEntityTypeConfiguration: IEntityTypeConfiguration<Payment>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("payments");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Reference)
                .HasColumnName("payment_reference")
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.HasIndex(p => p.Reference)
                .IsUnique();

            builder.Property(p => p.OrderId)
                .HasColumnName("order_id")
                .HasColumnType("uuid")
                .IsRequired();

            builder.Property(p => p.Amount)
                .HasColumnName("amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.Method)
                .HasColumnName("payment_method")
                .HasColumnType("varchar(100)")
                .HasConversion<EnumToStringConverter<PaymentMethod>>()
                .IsRequired();

            builder.Property(p => p.Status)
                .HasColumnName("payment_status")
                .HasColumnType("varchar(50)")
                .HasConversion<EnumToStringConverter<PaymentStatus>>()
                .IsRequired();

            builder.HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId)
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