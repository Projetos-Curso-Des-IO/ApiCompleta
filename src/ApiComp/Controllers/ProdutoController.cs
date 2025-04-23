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


        #region Ctor
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


		#region Recuperar todos
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProdutoImgViewModel>>> ObterTodos()
		{
			var _produto = await _produtoService.BuscarTodosProdutos();

			if (_produto == null)
				return CustomResponse();

			var _produtoView = _mapper.Map<IEnumerable<ProdutoImgViewModel>>(_produto);

			return Ok(_produtoView);
		}
        #endregion


        #region Recuperar por id
        [HttpGet("{id:Guid}")]
		public async Task<ActionResult<ProdutoImgViewModel>> ObterPorId(Guid id)
		{
			var _produto = await _produtoService.ObterProdutoPorId(id);

			if (_produto == null) 
				return CustomResponse();

			var _produtoView = _mapper.Map<ProdutoImgViewModel>(_produto);

			return Ok(_produtoView);
		}
        #endregion


        #region Adicionar
        [HttpPost("adicionar")]
		[RequestSizeLimit(52428800)]
		public async Task<ActionResult<ProdutoViewModel>> CriarProduto(ProdutoImgViewModel produtoViewModel)
		{
			if (!ModelState.IsValid) return CustomResponse(ModelState);

			if (!await UploadArquivoAlternativo(produtoViewModel.ImagemUpload, produtoViewModel))
			{
				return CustomResponse(produtoViewModel);
			}

			return Ok(CustomResponse(produtoViewModel));
		}
        #endregion


        #region Remover
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


        #region UploadArquivo
        private async Task<bool> UploadArquivoAlternativo(IFormFile arquivo, ProdutoImgViewModel produtoView)
		{

			var imgPrefixo = Guid.NewGuid() + "_";

			if (arquivo == null || arquivo.Length == 0)
			{
				NotificarErro("Forneça imagem para este produto!");
				return false;
			}


			var filePath = Path.Combine
						(
							Directory.GetCurrentDirectory(), 
							"wwwroot/app/demo-webapi/src/assets", 
							imgPrefixo + arquivo.FileName
						);

			if (System.IO.File.Exists(filePath))
			{
				NotificarErro("Já existe um arquivo com este nome!");
				return false;
			}


			//FileStream canal de escrita para arquivo fisico no disco
			using var stream = new FileStream(filePath, FileMode.Create);
			await arquivo.CopyToAsync(stream);


			var arquivoComNome = produtoView.ImagemUpload.FileName;
			produtoView.Imagem = imgPrefixo + arquivoComNome;

			if (produtoView != null)
			{
				await _produtoService.Adicionar(_mapper.Map<Produto>(produtoView));
			}

			return true;
		}
		#endregion
	}
}
