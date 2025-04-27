using ApiComp.Controllers;
using ApiComp.ViewModels;

namespace ApiComp.Extenssions
{
	public interface IUploadArquivo
	{
		Task<ProdutoImgViewModel> UploadArquivoAlternativo(IFormFile arquivo, ProdutoImgViewModel produtoView);
	}
}
