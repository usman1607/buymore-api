using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuyMoreApi.Infrastructure.Extensions
{
    public static class PropertyBuilderExtensions
    {
        public static PropertyBuilder<T> HasJsonbConversion<T>(this PropertyBuilder<T> propertyBuilder)
        where T : class, new()
        {
            var comparer = new ValueComparer<T>(
                (l, r) => JsonSerializer.Serialize(l, (JsonSerializerOptions?)null) ==
                        JsonSerializer.Serialize(r, (JsonSerializerOptions?)null),

                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null).GetHashCode(),

                v => JsonSerializer.Deserialize<T>(
                        JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                        (JsonSerializerOptions?)null)!
            );

            propertyBuilder
                .HasColumnType("jsonb")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<T>(v, (JsonSerializerOptions?)null)!
                );

            propertyBuilder.Metadata.SetValueComparer(comparer);

            return propertyBuilder;
        }
    }
}