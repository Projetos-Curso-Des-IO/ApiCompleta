using System.ComponentModel.DataAnnotations;

namespace ApiComp.ViewModels
{
	public class EnderecoViewModel
	{
		public Guid FornecedorId { get; set; }

		[Required(ErrorMessage = "O Logradouro é obrigatório.")]
		[StringLength(200, ErrorMessage = "O Logradouro deve ter no máximo 200 caracteres.")]
		public string Logradouro { get; set; }

		[Required(ErrorMessage = "O Número é obrigatório.")]
		[StringLength(10, ErrorMessage = "O Número deve ter no máximo 10 caracteres.")]
		public string Numero { get; set; }

		public string Complemento { get; set; }

		[Required(ErrorMessage = "O CEP é obrigatório.")]
		[StringLength(8, MinimumLength = 8, ErrorMessage = "O CEP deve ter exatamente 8 caracteres.")]
		public string Cep { get; set; }

		[Required(ErrorMessage = "O Bairro é obrigatório.")]
		[StringLength(100, ErrorMessage = "O Bairro deve ter no máximo 100 caracteres.")]
		public string Bairro { get; set; }

		[Required(ErrorMessage = "A Cidade é obrigatória.")]
		[StringLength(100, ErrorMessage = "A Cidade deve ter no máximo 100 caracteres.")]
		public string Cidade { get; set; }

		[Required(ErrorMessage = "O Estado é obrigatório.")]
		[StringLength(2, MinimumLength = 2, ErrorMessage = "O Estado deve ter exatamente 2 caracteres.")]
		public string Estado { get; set; }

		// Ao invés de referenciar um objeto, usamos apenas o nome do fornecedor
		public string NomeFornecedor { get; set; }
	}
}
