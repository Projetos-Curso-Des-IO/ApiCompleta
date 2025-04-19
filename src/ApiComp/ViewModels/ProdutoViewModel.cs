using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace ApiComp.ViewModels
{
	public class ProdutoViewModel
	{
		
		public Guid FornecedorId { get; set; }

		[Required(ErrorMessage = "O Nome é obrigatório.")]
		[StringLength(100, MinimumLength = 3, ErrorMessage = "O Nome deve ter no minímo 3 caracteres e máximo 100.")]
		public string Nome { get; set; }

		[StringLength(500, MinimumLength = 4, ErrorMessage = "A Descrição deve ter no minímo 4 caracteres e máximo 500.")]
		public string Descricao { get; set; }

		[Required(ErrorMessage = "Imagem do produto é obrigatório.")]
		public string ImagemUpload { get; set; }

		public string? Imagem { get; set; }

		[Required(ErrorMessage = "O Valor é obrigatório.")]
		[Range(0.01, double.MaxValue, ErrorMessage = "O Valor deve ser maior que zero.")]
		public decimal Valor { get; set; }


		[ScaffoldColumn(false)]
		public DateTime DataCadastro { get; set; }
		public bool Ativo { get; set; }

		[ScaffoldColumn(false)]
		public string? NomeFornecedor { get; set; }

	}
}
