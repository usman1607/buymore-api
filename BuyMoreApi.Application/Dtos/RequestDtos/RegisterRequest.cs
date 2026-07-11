using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace BuyMoreApi.Application.Dtos.RequestDtos
{
    public class RegisterRequest
    {
        //[Required(ErrorMessage = "First name is mandatory")]
        //[StringLength(20, MinimumLength = 3, ErrorMessage = "Must be between 3 and 20 characters")]
        public string FirstName { get; set; } = default!;
        //[Required(ErrorMessage = "Last name is mandatory")]
        //[StringLength(20, MinimumLength = 3, ErrorMessage = "Must be between 3 and 20 characters")]
        public string LastName { get; set; } = default!;
            
        //[Required]
        //[EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
    }
   

    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
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

            // Confirm Password Validation (Must match Password)
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Please confirm your password.")
                .Equal(x => x.Password).WithMessage("Passwords do not match.");

        }
    }

}