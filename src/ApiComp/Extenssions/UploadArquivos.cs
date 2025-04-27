using ApiComp.Controllers;
using ApiComp.ViewModels;
using AutoMapper;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Business.Notificacoes;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApiComp.Extenssions
{
	public class UploadArquivos : IUploadArquivo
	{
		private readonly INotificador _notificador;
		private readonly IProdutoService _produtoService;
		private readonly IMapper _mapper;

		#region Ctor
		public UploadArquivos(
							INotificador notificador,
							IProdutoService produtoService,
							IMapper mapper
							)
		{
			_notificador = notificador;
			_produtoService = produtoService;
			_mapper = mapper;
		}
		#endregion


		public async Task<ProdutoImgViewModel> UploadArquivoAlternativo(IFormFile arquivo, ProdutoImgViewModel produtoView)
		{
			var imagPrefix = Guid.NewGuid() + "_";

			if (arquivo == null || arquivo.Length == 0)
			{
				_notificador.Handle(new Notificacao("Forneça imagem para este produto"));
				return null;
			}

			var _caminhoDoArquivo = Path.Combine
						(
							Directory.GetCurrentDirectory(),
							"wwwroot/app/demo-webapi/src/assets",
							imagPrefix + arquivo.FileName
						);

			if (System.IO.File.Exists(_caminhoDoArquivo))
			{
				_notificador.Handle(new Notificacao("Já existe um arquivo com este nome!"));
				return null;
			}

			//FileStram canal de escrita para arquivo fisico no disco\
			using var stream = new FileStream(_caminhoDoArquivo, FileMode.Create);
			await arquivo.CopyToAsync(stream);

			var arquivoNome = produtoView.ImagemUpload.FileName;
			produtoView.Imagem = imagPrefix + arquivoNome;
			
			return produtoView;
		}
	}
}
