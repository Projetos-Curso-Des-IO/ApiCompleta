using ApiComp.ViewModels;
using AutoMapper;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiComp.Controllers
{
	[Route("api/fornecedores")]
	public class FornecedoresController : MainController
	{

        private readonly IFornecedorRepository _fornecedorRepository;
		private readonly IMapper _mapper;

		public FornecedoresController(IFornecedorRepository fornecedorRepository,
									   IMapper mapper)
		{
			this._fornecedorRepository = fornecedorRepository;
			this._mapper = mapper;
		}



		public async Task<IEnumerable<FornecedorViewModel>> ObterTodos()
        {
			//Banco → (Fornecedor) → AutoMapper → (FornecedorViewModel) → Retorno na API ✅
			var fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());

			return fornecedores;
        }
	}
}
