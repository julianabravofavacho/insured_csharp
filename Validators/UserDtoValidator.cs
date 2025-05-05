using FluentValidation;
using WebApi_Coris.Models.Dtos;

namespace WebApi_Coris.Validators
{
    public class UserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("O nome é obrigatório.");

            RuleFor(x => x.email)
               .NotEmpty().WithMessage("O e-mail é obrigatório.")
               .EmailAddress().WithMessage("Informe um e-mail válido.");

            RuleFor(x => x.password_hash)
                .NotEmpty().WithMessage("A senha é obrigatória.");
        }
    }
}
