using System.ComponentModel.DataAnnotations;

namespace ApiComp.ViewModels
{
	public class FornecedorViewModel
	{
		[Key]
		public Guid Id { get; set; }

		[Required(ErrorMessage = "O campo Nome é obrigatório.")]
		[StringLength(100, ErrorMessage = "O Nome deve ter no máximo 100 caracteres.")]
		public string Nome { get; set; }

		[Required(ErrorMessage = "O campo Documento é obrigatório.")]
		[StringLength(14, MinimumLength = 11, ErrorMessage = "O Documento deve ter entre 11 e 14 caracteres.")]
		public string Documento { get; set; }

		[Required(ErrorMessage = "O Tipo do Fornecedor é obrigatório.")]
		[Range(0, 1, ErrorMessage = "O Tipo do Fornecedor deve ser 0 (Pessoa Física) ou 1 (Pessoa Jurídica).")]
		public int TipoFornecedor { get; set; }

		public bool Ativo { get; set; }

		public EnderecoViewModel? Endereco { get; set; }
		public IEnumerable<ProdutoImgViewModel>? Produto { get; set; }

    }
}
