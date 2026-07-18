using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace BuyMoreApi.Application.Dtos.RequestDtos
{
    public class SearchItemRequest : PaginationRequest
    {
        public string? Category { get; set; }
        public PriceRange PriceRange { get; set; } = PriceRange.Default;
    }

    public record PriceRange(decimal MinPrice, decimal MaxPrice)
    {
        public static PriceRange Default => new(0, 0);
    }

    public class SearchItemRequestValidator : AbstractValidator<SearchItemRequest>
    {
        public SearchItemRequestValidator()
        {
            RuleFor(x => x.PriceRange)
                .NotNull()
                .ChildRules(range =>
                {
                    range.RuleFor(x => x.MinPrice)
                        .GreaterThanOrEqualTo(0)
                        .WithMessage("Minimum price cannot be a negative value.");

                    range.RuleFor(x => x.MaxPrice)
                        .GreaterThanOrEqualTo(0)
                        .WithMessage("Maximum price cannot be a negative value.");

                    range.RuleFor(x => x.MaxPrice)
                        .GreaterThanOrEqualTo(x => x.MinPrice)
                        .When(x => x.MaxPrice > 0)
                        .WithMessage("Maximum price cannot be less than minimum price.");
                });
        }
    }

}