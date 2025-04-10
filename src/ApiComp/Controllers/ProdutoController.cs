using ApiComp.ViewModels;
using AutoMapper;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
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
            var _produtoView =  _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosFornecedores());
            if(!_produtoView.Any()) return NotFound("Lista vazia");
            
            return Ok(CustomResponse(_produtoView));
        }




        [HttpPost]
        public async Task<ActionResult<ProdutoViewModel>> CriarProduto(ProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));
            return Ok(CustomResponse(produtoViewModel));
        }




        #endregion






    }
}
