using FluentValidation;
using WebApi_Coris.Models.Dtos;

namespace WebApi_Coris.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x => x.id)
                .GreaterThan(0).WithMessage("Id do usuário é obrigatório.");

            RuleFor(x => x.name)
                .NotEmpty().WithMessage("O nome é obrigatório.");

        }
    }
}
