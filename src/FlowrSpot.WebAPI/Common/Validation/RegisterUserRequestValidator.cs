using FlowrSpot.Application.Repositories;
using FlowrSpot.Dtos;
using FluentValidation;

namespace FlowrSpot.WebAPI.Common.Validation
{
    public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        private readonly IUserRepository _userRepository;
        public RegisterUserRequestValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(p => p.Username)
                .NotEmpty().WithMessage("Username is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters.")
                .MustAsync(IsUsernameUnique).WithMessage("Username already exists.");

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Your password must have at least 8 characters.")
                .MaximumLength(30).WithMessage("Your password length must not exceed 30 characters.")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                .Matches(@"[\!\?\*\.\&\#\@\+\-]+").WithMessage("Your password must contain at least one symbol: !?*.&#@+-");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Email is required.")
                .NotNull()
                .EmailAddress()
                .MustAsync(IsEmailUnique).WithMessage("Email already exists.");
        }

        private async Task<bool> IsUsernameUnique(string username, CancellationToken cancellationToken)
        {
            return await _userRepository.IsUsernameUnique(username);
        }

        private async Task<bool> IsEmailUnique(string email, CancellationToken cancellationToken)
        {
            return await _userRepository.IsEmailUnique(email);
        }
    }
}
