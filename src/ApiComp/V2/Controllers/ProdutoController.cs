using ApiComp.ControllerPrincipal;
using ApiComp.Extenssions;
using ApiComp.ViewModels;
using AutoMapper;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Business.Notificacoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace ApiComp.V2.Controllers
{
	[Authorize]
	[ApiVersion("2.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/produto")]
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
								 IUploadArquivo uploadArquivo,
								 IUser user) : base(notificador, user)
        {
            _produtoRepository = produtoRepository;
            _produtoService = produtoService;
            _mapper = mapper;
            _notificador = notificador;
			_uploadArquivo = uploadArquivo;
        }
		#endregion


		#region Recuperar todos		
		[AllowAnonymous]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProdutoImgViewModel>>> ObterTodos()
		{
			var _produto = await _produtoService.BuscarTodosProdutos();

			if (_produto == null)
				return CustomResponse();

			var _produtoView = _mapper.Map<IEnumerable<ProdutoImgViewModel>>(_produto);

			var _prodtudoViewUsuario = new
			{
				produto = _produtoView,
				usuarioCadastro = User.GetUserId()
			};

			return Ok(_prodtudoViewUsuario);
		} //
        #endregion



	}
}
