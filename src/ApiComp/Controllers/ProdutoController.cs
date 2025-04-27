using ApiComp.Extenssions;
using ApiComp.ViewModels;
using AutoMapper;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Business.Notificacoes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
		private readonly IUploadArquivo _uploadArquivo;
        #endregion


        #region Ctor
        public ProdutoController(IProdutoRepository produtoRepository, 
                                 IProdutoService produtoService, 
                                 IMapper mapper, 
                                 INotificador notificador,
								 IUploadArquivo uploadArquivo) : base(notificador)
        {
            _produtoRepository = produtoRepository;
            _produtoService = produtoService;
            _mapper = mapper;
            _notificador = notificador;
			_uploadArquivo = uploadArquivo;
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

			var produto = await _uploadArquivo.
				UploadArquivoAlternativo(produtoViewModel.ImagemUpload, produtoViewModel);

			if (produto == null) return BadRequest(CustomResponse());

			await _produtoService.Adicionar(_mapper.Map<Produto>(produto));


			return Ok(CustomResponse(produto));
		}
		#endregion


		#region AtualizarProduto
		[HttpPut("atualizar/{id:Guid}")]
		public async Task<ActionResult<ProdutoViewModel>> AtualizarProduto(Guid id, ProdutoImgViewModel produtoImgViewModel)
		{
			if (!ModelState.IsValid) return CustomResponse(ModelState);

			var produto = produtoImgViewModel;

			if (!produtoImgViewModel.Imagem.IsNullOrEmpty())
			{
				 produto = await _uploadArquivo.
					UploadArquivoAlternativo(produtoImgViewModel.ImagemUpload, produtoImgViewModel);
			}

			if(produto != null)
			{
				await _produtoService.Atualizar(id, _mapper.Map<Produto>(produto));
			}
			
			return Ok((CustomResponse(produtoImgViewModel)));
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


	}
}
