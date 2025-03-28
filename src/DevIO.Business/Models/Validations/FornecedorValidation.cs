using DevIO.Business.Models.Validations.Documentos;
using FluentValidation;

namespace DevIO.Business.Models.Validations
{
    public class FornecedorValidation : AbstractValidator<Fornecedor>
    {
        public FornecedorValidation()
		{
			RuleFor(f => f.Nome)
				.NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
				.Length(2, 100)
				.WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

			When(f => f.TipoFornecedor == TipoFornecedor.PessoaFisica, () =>
			{
				RuleFor(f => f.Documento)
					.Must(doc => doc.Length == CpfValidacao.TamanhoCpf)
					.WithMessage("O campo Documento precisa ter 11 caracteres e foi fornecido {PropertyValue}.")
					.Must(CpfValidacao.Validar)
					.WithMessage("O documento fornecido é inválido para Pessoa Física.");
			});

			When(f => f.TipoFornecedor == TipoFornecedor.PessoaJuridica, () =>
			{
				RuleFor(f => f.Documento)
					.Must(doc => doc.Length == CnpjValidacao.TamanhoCnpj)
					.WithMessage("O campo Documento precisa ter 14 caracteres e foi fornecido {PropertyValue}.")
					.Must(CnpjValidacao.Validar)
					.WithMessage("O documento fornecido é inválido para Pessoa Jurídica.");
			});
		}
	}
}