using ApiComp.Controllers;
using ApiComp.ViewModels;

namespace ApiComp.Extenssions
{
	public interface IUploadArquivo
	{
		//Task<bool> UploadArquivoAlternativo(IFormFile arquivo, ProdutoImgViewModel produtoView);
		Task<ProdutoImgViewModel> UploadArquivoAlternativo(IFormFile arquivo, ProdutoImgViewModel produtoView);
	}
}
