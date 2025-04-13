using System.ComponentModel.DataAnnotations;

namespace ApiComp.ViewModels
{
	public class ProdutoViewModel
	{
		public Guid FornecedorId { get; set; }

		[Required(ErrorMessage = "O Nome é obrigatório.")]
		[StringLength(100, ErrorMessage = "O Nome deve ter no máximo 100 caracteres.")]
		public string Nome { get; set; }

		[StringLength(500, ErrorMessage = "A Descrição deve ter no máximo 500 caracteres.")]
		public string Descricao { get; set; }

		public string ImagemUpload { get; set; }

		public string Imagem { get; set; }

		[Required(ErrorMessage = "O Valor é obrigatório.")]
		[Range(0.01, double.MaxValue, ErrorMessage = "O Valor deve ser maior que zero.")]
		public decimal Valor { get; set; }


		[ScaffoldColumn(false)]
		public DateTime DataCadastro { get; set; }
		public bool Ativo { get; set; }

		[ScaffoldColumn(false)]
		public string NomeFornecedor { get; set; }

	}
}
