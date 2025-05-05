using FluentValidation;
using WebApi_Coris.Models.Dtos;

namespace WebApi_Coris.Validators
{
    public class InsuredDtoValidator: AbstractValidator<CreateInsuredDto>
    {
        public InsuredDtoValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("O nome é obrigatório.");

            RuleFor(x => x.cpf_cnpj)
                .NotEmpty().WithMessage("O CPF/CNPJ é obrigatório.")
                .Must(CpfCnpjValidator.IsValid)
                .WithMessage("Informe um CPF ou CNPJ válido.");

            RuleFor(x => x.email)
               .NotEmpty().WithMessage("O e-mail é obrigatório.")
               .EmailAddress().WithMessage("Informe um e-mail válido.");

            RuleFor(x => x.cell_phone)
                .NotEmpty().WithMessage("O celular é obrigatório.")
                .Matches(@"^\([1-9]{2}\)\s?9?[6-9]\d{3}\-?\d{4}$")
                .WithMessage("Informe um celular válido. Ex: (11) 91234-5678");
        }
    }
}
