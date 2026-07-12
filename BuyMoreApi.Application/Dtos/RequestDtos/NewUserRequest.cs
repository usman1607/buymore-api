using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyMoreApi.Domain.Enums;
using FluentValidation;

namespace BuyMoreApi.Application.Dtos.RequestDtos
{
    public class NewUserRequest
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public Role Role { get; set; }
    }

    public class NewUserRequestValidator : AbstractValidator<NewUserRequest>
    {
        public NewUserRequestValidator()
        {
            // First Name Validation
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

            // Last Name Validation
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            // Email Validation
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            // Password Validation (Complexity Rules)
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
                .Matches(@"[\^$*.\[\]{}()?""!@#%&/\\,><':;|_~`\-+=]").WithMessage("Password must contain at least one special character.");

            // Role Validation
            RuleFor(x => x.Role)
                .IsInEnum().WithMessage($"Role must be a valid enum value from: {string.Join(", ", Enum.GetValues(typeof(Role)))}.");
        }
    }
}