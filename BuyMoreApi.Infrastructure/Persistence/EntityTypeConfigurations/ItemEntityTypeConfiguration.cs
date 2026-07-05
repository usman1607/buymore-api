using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyMoreApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuyMoreApi.Infrastructure.Persistence.EntityTypeConfigurations
{
    public class ItemEntityTypeConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("items");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .ValueGeneratedOnAdd();

            builder.Property(i => i.Name)
                .HasColumnName("name")
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(i => i.Description)
                .HasColumnName("description")
                .HasColumnType("text");

            builder.Property(i => i.CostPrice)
                .HasColumnName("price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(i => i.SellingPrice)
                .HasColumnName("selling_price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(i => i.Quantity)
                .HasColumnName("quantity")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(i => i.Category)
                .HasColumnName("category")
                .HasColumnType("varchar(50)")
                .IsRequired();

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