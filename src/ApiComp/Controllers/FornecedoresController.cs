using ApiComp.ViewModels;
using AutoMapper;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
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

		#region ctor
		public FornecedoresController(IFornecedorRepository fornecedorRepository,
									   IMapper mapper, IFornecedorService fornecedorService)
		{
			_fornecedorRepository = fornecedorRepository;
			_mapper = mapper;
			_fornecedorService = fornecedorService;
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


		[HttpGet("{id:Guid}")]
		public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
		{
			var fornecedores = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterPorId(id));
			if (fornecedores == null)
				return NotFound($"Id: {id}");

			return fornecedores;
		}



		[HttpGet("{id:Guid}")]
		public async Task<ActionResult<FornecedorViewModel>> ObterFornecedorProdutosEndereco(Guid id)
		{
			var fornecedor = _mapper.Map<ActionResult<FornecedorViewModel>>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));

			if (fornecedor == null)
				return NotFound($"Id: {id}");

			return fornecedor;
		}



		[HttpPost]
		public async Task<ActionResult<FornecedorViewModel>> CriarFornecedor(FornecedorViewModel fornecedorView)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var fornecedor = _mapper.Map<Fornecedor>(fornecedorView);

			var result = await _fornecedorService.Adicionar(fornecedor);

			if (!result)
				return BadRequest();

			return Ok(result);
		}



		[HttpPut("{id:Guid}")]
		public async Task<ActionResult<FornecedorViewModel>> AtulizarFonecedor(Guid id, FornecedorViewModel fornecedorView)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var fornecedor = _mapper.Map<Fornecedor>(fornecedorView);

			var result = await _fornecedorService.Atualizar(fornecedor);
			if (!result)
				return BadRequest();

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
			if (!result)
				return BadRequest($"Id: {id}");

			return Ok(fornecedor);
		}



	
		public async Task<ActionResult<FornecedorViewModel>> ObterFornecedorEndereco(Guid id)
		{
			return _mapper.Map<ActionResult<FornecedorViewModel>>(await _fornecedorRepository.ObterFornecedorEndereco(id));
		}



		#endregion
	}
}
