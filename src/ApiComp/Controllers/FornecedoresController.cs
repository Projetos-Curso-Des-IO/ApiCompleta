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
		#region dependency
		private readonly IFornecedorRepository _fornecedorRepository;
		private readonly IFornecedorService _fornecedorService;
		private readonly IMapper _mapper;
		private readonly INotificador _notificador;
		#endregion

		#region ctor
		public FornecedoresController(IFornecedorRepository fornecedorRepository,
									   IMapper mapper, IFornecedorService fornecedorService,
									   INotificador notificador) : base(notificador)
		{
			_fornecedorRepository = fornecedorRepository;
			_mapper = mapper;
			_fornecedorService = fornecedorService;
			_notificador = notificador;
		}
		#endregion


		#region Actions
		[HttpGet]
		public async Task<ActionResult<IEnumerable<FornecedorViewModel>>> ObterTodos()
        {
			var _fornecedoresView = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
			if(!_fornecedoresView.Any()) return NotFound("Lista vazia.");

			return Ok(CustomResponse(_fornecedoresView));
        }





		[HttpGet("recuperarPorId/{id:Guid}")]
		public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
		{
			var _fornecedores = await _fornecedorRepository.ObterPorId(id);
			if (_fornecedores == null) return NotFound($"Id: {id}");

			var _fornecedorView = _mapper.Map <FornecedorViewModel>(_fornecedores);
			return Ok(CustomResponse(_fornecedorView));
		}





		[HttpGet("obterFornecedorProdutosEndereco/{id:Guid}")]
		public async Task<ActionResult<FornecedorViewModel>> ObterFornecedorProdutosEndereco(Guid id)
		{
			var _fornecedor = await _fornecedorRepository.ObterFornecedorProdutosEndereco(id);
			if (_fornecedor == null) return NotFound($"Id: {id}");

			var _forncedorView = _mapper.Map<FornecedorViewModel> (_fornecedor);
			return Ok(CustomResponse(_forncedorView));
		}




		[HttpPost]
		public async Task<ActionResult<FornecedorViewModel>> CriarFornecedor(FornecedorViewModel fornecedorView)
		{
			if (!ModelState.IsValid) return CustomResponse(ModelState);
			
			await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorView));
			return Ok(CustomResponse(fornecedorView));
		}






		[HttpPut("{id:Guid}")]
		public async Task<ActionResult<FornecedorViewModel>> AtulizarFonecedor(Guid id, FornecedorViewModel fornecedorView)
		{
			if (id != fornecedorView.Id) return BadRequest();
			if (!ModelState.IsValid) return CustomResponse(ModelState);
			
			await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(fornecedorView));
			return Ok(CustomResponse(fornecedorView));
		}





		[HttpDelete("{id:Guid}")]
		public async Task<ActionResult<FornecedorViewModel>> DeletarFornecedor(Guid id)
		{
			if (id == Guid.Empty) return BadRequest("ID inválido.");
			if (await ObterFornecedorEndereco(id) == null) return NotFound($"Fornecedor com Id: {id} não encontrado.");

			await _fornecedorService.Remover(id);
			return NoContent();
		}
		#endregion


		#region	Private Methods
		public async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
		{
			var _fornecedor = await _fornecedorRepository.ObterFornecedorEndereco(id);
			var _fornecedorViewModel = _mapper.Map<FornecedorViewModel>(_fornecedor);

			return _fornecedorViewModel;
		}
		#endregion


	}
}
