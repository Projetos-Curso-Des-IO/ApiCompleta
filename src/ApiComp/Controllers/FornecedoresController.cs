using ApiComp.ViewModels;
using AutoMapper;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Business.Notificacoes;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;

namespace ApiComp.Controllers
{

	[Route("api/fornecedores")]
	public class FornecedoresController : MainController
	{

        private readonly IFornecedorRepository _fornecedorRepository;
		private readonly IFornecedorService _fornecedorService;
		private readonly IMapper _mapper;
		private readonly INotificador _notificador;

		#region ctor
		public FornecedoresController(IFornecedorRepository fornecedorRepository,
									   IMapper mapper, IFornecedorService fornecedorService,
									   INotificador notificador)
		{
			_fornecedorRepository = fornecedorRepository;
			_mapper = mapper;
			_fornecedorService = fornecedorService;
			_notificador = notificador;
		}
		#endregion




		#region methods
		[HttpGet]
		public async Task<ActionResult<IEnumerable<FornecedorViewModel>>> ObterTodos()
        {
			//Banco → (Fornecedor) → AutoMapper → (FornecedorViewModel) → Retorno na API ✅
			var fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
			if(!fornecedores.Any())
				return NotFound("Lista vazia.");

			return Ok(fornecedores);
        }





		[HttpGet("recuperarPorId/{id:Guid}")]
		public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
		{
			var fornecedores = await _fornecedorRepository.ObterPorId(id);
			if (fornecedores == null)
				return NotFound($"Id: {id}");

			var fornecedorView = _mapper.Map <FornecedorViewModel>(fornecedores);
			return fornecedorView;
		}





		[HttpGet("obterFornecedorProdutosEndereco/{id:Guid}")]
		public async Task<ActionResult<FornecedorViewModel>> ObterFornecedorProdutosEndereco(Guid id)
		{
			var fornecedor = await _fornecedorRepository.ObterFornecedorProdutosEndereco(id);
			if (fornecedor == null)
				return NotFound($"Id: {id}");

			var forncedorView = _mapper.Map<FornecedorViewModel> (fornecedor);
			return forncedorView;
		}



		[HttpPost]
		public async Task<ActionResult<FornecedorViewModel>> CriarFornecedor(FornecedorViewModel fornecedorView)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var fornecedor = _mapper.Map<Fornecedor>(fornecedorView);
			var result = await _fornecedorService.Adicionar(fornecedor);


			var msgNotificacao = _notificador.ObterNotificacoes();
			if (!result)
				return BadRequest(msgNotificacao);

			var forneceroView = _mapper.Map<FornecedorViewModel>(fornecedor);

			return Ok(fornecedorView);
		}



		[HttpPut("{id:Guid}")]
		public async Task<ActionResult<FornecedorViewModel>> AtulizarFonecedor(Guid id, FornecedorViewModel fornecedorView)
		{
			if (id != fornecedorView.Id) return BadRequest();

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var fornecedor = _mapper.Map<Fornecedor>(fornecedorView);
			var result = await _fornecedorService.Atualizar(fornecedor);

			var msgNotificacao = _notificador.ObterNotificacoes();
			if (!result)
				return BadRequest(msgNotificacao);

			return Ok(fornecedor);
		}



		[HttpDelete("{id:Guid}")]
		public async Task<ActionResult<FornecedorViewModel>> DeletarFornecedor(Guid id)
		{

			if (id == null)
				return BadRequest(id);

			var fornecedor = await ObterFornecedorEndereco(id);
			if (fornecedor == null)
				return NotFound($"Id: {id}");

			var result = await _fornecedorService.Remover(id);

			var msgNotificacao = _notificador.ObterNotificacoes();
			if (!result)
				return BadRequest(msgNotificacao);

			return Ok(fornecedor);
		}



	
		public async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
		{
			var _fornecedor = await _fornecedorRepository.ObterFornecedorEndereco(id);
			var _fornecedorViewModel = _mapper.Map<FornecedorViewModel>(_fornecedor);

			return _fornecedorViewModel;
		}



		#endregion
	}
}
