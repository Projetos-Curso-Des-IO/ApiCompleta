﻿using ApiComp.ControllerPrincipal;
using ApiComp.ViewModels;
using AutoMapper;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Business.Notificacoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;
using static ApiComp.Extenssions.CustomAuthorize;

namespace ApiComp.V1.Controllers
{

	[Authorize]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/fornecedor")]
	[Route("api/fornecedores")]
	public class FornecedoresController : MainController
	{
		#region dependency
		private readonly IFornecedorRepository _fornecedorRepository;
		private readonly IEnderecoRepository _enderecoRepository;
		private readonly IFornecedorService _fornecedorService;
		private readonly IMapper _mapper;
		private readonly INotificador _notificador;
		#endregion

		#region Ctor
		public FornecedoresController(IFornecedorRepository fornecedorRepository,
									   IEnderecoRepository enderecoRepository,	
									   IMapper mapper, IFornecedorService fornecedorService,
									   INotificador notificador,
									   IUser user) : base(notificador, user)
		{
			_fornecedorRepository = fornecedorRepository;
			_enderecoRepository = enderecoRepository;
			_mapper = mapper;
			_fornecedorService = fornecedorService;
			_notificador = notificador;
		}
		#endregion


		#region ObterTodos
		[ClaimsAuthorize("Fornecedor", "ObterTodos")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<FornecedorViewModel>>> ObterTodos()
        {
			var _fornecedoresView = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
			if(!_fornecedoresView.Any()) return NotFound("Lista vazia.");

			return Ok(_fornecedoresView);
        }
		#endregion


		#region ObterPorId
		[HttpGet("recuperarPorId/{id:Guid}")]
		public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
		{
			var _fornecedores = await _fornecedorRepository.ObterPorId(id);
			if (_fornecedores == null) return NotFound($"Id: {id}");

			var _fornecedorView = _mapper.Map <FornecedorViewModel>(_fornecedores);
			return Ok(CustomResponse(_fornecedorView));
		}
		#endregion


		#region ObterFornecedorProdutosEndereco
		[HttpGet("obterFornecedorProdutosEndereco/{id:Guid}")]
		public async Task<ActionResult<FornecedorViewModel>> ObterFornecedorProdutosEndereco(Guid id)
		{
			var _fornecedor = await _fornecedorRepository.ObterFornecedorProdutosEndereco(id);
			if (_fornecedor == null) return NotFound($"Id: {id}");

			var _forncedorView = _mapper.Map<FornecedorViewModel> (_fornecedor);
			return Ok(CustomResponse(_forncedorView));
		}
		#endregion


		#region ObterEnderecoPorFornec
		[HttpGet("obterEnderecoPorFornecedor/{id:guid}")]
		public async Task<ActionResult<EnderecoViewModel>> ObterEnderecoPorFornec(Guid id)
		{
			var _endereco = await _enderecoRepository.ObterEnderecoPorFornecedor(id);
			if (_endereco == null) return NotFound($"Id: {id}");

			var _enderecoView = _mapper.Map<EnderecoViewModel>(_endereco);
			return Ok(CustomResponse(_enderecoView));
		}
		#endregion


		#region CriarFornecedor
		[ClaimsAuthorize("Fornecedor", "CriarFornecedor")]
		[HttpPost]
		public async Task<ActionResult<FornecedorViewModel>> CriarFornecedor(FornecedorViewModel fornecedorView)
		{
			if (!ModelState.IsValid) return CustomResponse(ModelState);
			
			await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorView));
			return Ok(CustomResponse(fornecedorView));
		}
		#endregion


		#region AtulizarFonecedor
		[ClaimsAuthorize("Fornecedor", "AtulizarFonecedor")]
		[HttpPut("{id:Guid}")]
		public async Task<ActionResult<FornecedorViewModel>> AtulizarFonecedor(Guid id, FornecedorViewModel fornecedorView)
		{
			if (id != fornecedorView.Id) return BadRequest();
			if (!ModelState.IsValid) return CustomResponse(ModelState);
			
			await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(fornecedorView));
			return Ok(CustomResponse(fornecedorView));
		}
		#endregion


		#region AtualizarEndereco
		[ClaimsAuthorize("Endereco", "AtualizarEndereco")]
		[HttpPut("atualizarEndereco/{id:guid}")]
		public async Task<ActionResult<EnderecoViewModel>> AtualizarEndereco(Guid id, EnderecoViewModel enderecoViewModel)
		{
			if (id != enderecoViewModel.FornecedorId) return BadRequest();
			if (!ModelState.IsValid) return CustomResponse(ModelState);

			await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(enderecoViewModel));
			return Ok(CustomResponse(enderecoViewModel));
		}
		#endregion


		#region DeletarFornecedor
		[ClaimsAuthorize("Fornecedor", "DeletarFornecedor")]
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
		private async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
		{
			var _fornecedor = await _fornecedorRepository.ObterFornecedorEndereco(id);
			var _fornecedorViewModel = _mapper.Map<FornecedorViewModel>(_fornecedor);

			return _fornecedorViewModel;
		}
		#endregion


	}
}

