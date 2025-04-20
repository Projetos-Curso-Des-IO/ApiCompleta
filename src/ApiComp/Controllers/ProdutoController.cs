using ApiComp.ViewModels;
using AutoMapper;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Business.Notificacoes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiComp.Controllers
{
    [Route("api/produto")]
    [ApiController]
    public class ProdutoController : MainController
    {
        #region dependecy
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;
        private readonly INotificador _notificador;
        #endregion


        #region cto
        public ProdutoController(IProdutoRepository produtoRepository, 
                                 IProdutoService produtoService, 
                                 IMapper mapper, 
                                 INotificador notificador) : base(notificador)
        {
            _produtoRepository = produtoRepository;
            _produtoService = produtoService;
            _mapper = mapper;
            _notificador = notificador;
        }
		#endregion



		#region actions
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProdutoImgViewModel>>> ObterTodos()
		{
			var _produto = await _produtoService.BuscarTodosProdutos();

			if (_produto == null)
				return CustomResponse();

			var _produtoView = _mapper.Map<IEnumerable<ProdutoImgViewModel>>(_produto);

			return Ok(_produtoView);
		}





		[HttpGet("{id:Guid}")]
		public async Task<ActionResult<ProdutoImgViewModel>> ObterPorId(Guid id)
		{
			var _produto = await _produtoService.ObterProdutoPorId(id);

			if (_produto == null) 
				return CustomResponse();

			var _produtoView = _mapper.Map<ProdutoImgViewModel>(_produto);

			return Ok(_produtoView);
		}






		[HttpPost("adicionar")]
		[RequestSizeLimit(52428800)]
		public async Task<ActionResult<ProdutoViewModel>> CriarProduto(ProdutoImgViewModel produtoViewModel)
		{
			if (!ModelState.IsValid) return CustomResponse(ModelState);

			var imagemPrefix = Guid.NewGuid() + "_";
			if (!await UploadArquivoAlternativo(produtoViewModel.ImagemUpload, imagemPrefix))
			{
				return CustomResponse(produtoViewModel);
			}

			var arquivo = produtoViewModel.ImagemUpload.FileName;

			produtoViewModel.Imagem = imagemPrefix + arquivo;

			await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

			return Ok(CustomResponse(produtoViewModel));
		}



		

		[HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProdutoImgViewModel>> ExcluirProduto(Guid id)
        {
			var produtoRemovido =  await _produtoService.Remover(id);
            if (produtoRemovido)
            {
				return NoContent();
			}
            return CustomResponse();
        }
		#endregion






		#region methods
		private async Task<bool> UploadArquivoAlternativo(IFormFile arquivo, string imgPrefix)
		{
			if (arquivo == null || arquivo.Length == 0)
			{
				NotificarErro("Forneça imagem para este produto!");
				return false;
			}

			var filePath = Path.Combine
						(
							Directory.GetCurrentDirectory(), 
							"wwwroot/app/demo-webapi/src/assets", 
							imgPrefix + arquivo.FileName
						);

			if (System.IO.File.Exists(filePath))
			{
				NotificarErro("Já existe um arquivo com este nome!");
				return false;
			}

			//FileStream canal de escrita para arquivo fisico no disco
			using var stream = new FileStream(filePath, FileMode.Create);
			await arquivo.CopyToAsync(stream);
			return true;
		}

		#endregion
	}
}
