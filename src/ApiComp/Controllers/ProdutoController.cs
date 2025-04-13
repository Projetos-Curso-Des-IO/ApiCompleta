using ApiComp.ViewModels;
using AutoMapper;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Business.Notificacoes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<ProdutoViewModel>>> ObterTodos()
        {
            var _produtoView =  _mapper.Map<IEnumerable<ProdutoViewModel>>
                (await _produtoRepository.ObterProdutosFornecedores());

            if(!_produtoView.Any()) return NotFound("Lista vazia");
            
            return Ok(CustomResponse(_produtoView));
        }



        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<ProdutoViewModel>> ObterPorId(Guid id)
        {
            var _produtoView = await ObterProdutoPorId(id);
                if (_produtoView.Value == null) return NotFound($"Produto não encontrado - ID: {id}");

            return CustomResponse(_produtoView);
        }




        [HttpPost]
        public async Task<ActionResult<ProdutoViewModel>> CriarProduto(ProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;
            if(!UploadArquivo(produtoViewModel.ImagemUpload, imagemNome))
            {
                return CustomResponse(produtoViewModel);
            }

            produtoViewModel.Imagem = imagemNome;
            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return Ok(CustomResponse(produtoViewModel));
        }






		#endregion






		#region methods
        public async Task<ActionResult<ProdutoViewModel>> ObterProdutoPorId(Guid id)
        {            
			var _produtoView = _mapper.Map<ProdutoViewModel>
                (await _produtoRepository.ObterPorId(id));          
            return _produtoView;
        }




		// Método para fazer upload de um arquivo de imagem codificado em base64
		private bool UploadArquivo(string arquivo, string imgNome)
        {
			// Converte a string base64 para um array de bytes (dados binários da imagem)
			var imagemDataByteArray = Convert.FromBase64String(arquivo);

			if (string.IsNullOrEmpty(arquivo))
			{
                NotificarErro("Forneça imagem para este produto!");
                return false;
			}

			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgNome);

			if (System.IO.File.Exists(filePath))
			{
                NotificarErro("Já existe um arquivo com este nome!");
				return false;
			}
            
            // Gerar arquivo = caminho + imgBytes
            System.IO.File.WriteAllBytes(filePath, imagemDataByteArray);

			return true;
        }




		#endregion
	}
}
