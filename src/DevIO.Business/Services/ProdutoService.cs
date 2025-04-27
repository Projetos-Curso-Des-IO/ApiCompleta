using AutoMapper;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Business.Models.Validations;
using DevIO.Business.Notificacoes;

namespace DevIO.Business.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;
		private readonly INotificador _notificador;
		

		public ProdutoService(IProdutoRepository produtoRepository,
							  INotificador notificador,
							  IMapper mapper) : base(notificador)
		{
			_produtoRepository = produtoRepository;
			_mapper = mapper;
			_notificador = notificador;
		}






		public async Task<IEnumerable<Produto>?> BuscarTodosProdutos()
		{
			{
				var _produtoRecuperado = await _produtoRepository.ObterProdutosFornecedores();
				if (!_produtoRecuperado.Any())
				{
					NotificarErro($"Nenhum produto foi encontrado(a) no momento.");
					return null;
				}

				return _produtoRecuperado;
			}
		}




		public async Task<Produto>?ObterProdutoPorId(Guid id)
		{
			var _produtoRecuperado = await _produtoRepository.ObterProdutoFornecedor(id);
			if (_produtoRecuperado==null)
			{
				NotificarErro($"Produto não encontrado(a) no momento. ID: {id}");
				return null;
			}

			return _produtoRecuperado;
		}








		public async Task Adicionar(Produto produto)
		{
			if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;
			if(produto == null)
			{
				NotificarErro($"Fornecedor inválido!");
			}

			await _produtoRepository.Adicionar(produto);
		}





		public async Task<bool> Atualizar(Guid id,Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return false;

			if (id == Guid.Empty) return NotificarERetornar($"Id inválido: {id}. Verifique!");

			var produtoExistente = await _produtoRepository.ObterProdutoFornecedor(id);
			if (produtoExistente == null)
				return NotificarERetornar("Produto não encontrado(a) para atualização.");

			if (produto == null) return NotificarERetornar($"Produto nulo {produto}. Verifique!");

			produtoExistente.FornecedorId = produto.FornecedorId;
			produtoExistente.Nome = produto.Nome; 
			produtoExistente.Descricao = produto.Descricao;
			produtoExistente.Ativo = produto.Ativo;
			produtoExistente.Valor = produto.Valor;

            await _produtoRepository.Atualizar(produtoExistente);

			return true;
		}




		public async Task<bool> Remover(Guid id)
		{
			if (id == Guid.Empty) return NotificarERetornar($"ID inválido. {id}");

			var produto = await _produtoRepository.ObterPorId(id);

			if (produto != null)
			{
				await _produtoRepository.Remover(id);
				return true;
			}
			return NotificarERetornar($"Produto com ID: {id} não encontrado(a) para remoção.");

		}




		//protected Task<Produto> ValidarProdutoNulo(Produto produto)
		//{
		//	if(produto == null) return 
		//	var produtoExistente = 
		//	return 
		//}



		protected void NotificarErro(string mensagem)
		{
			_notificador.Handle(new Notificacao(mensagem));
		}


		private bool NotificarERetornar(string mensagem)
		{
			Notificar(mensagem);
			return false;
		}


		public void Dispose()
        {
            _produtoRepository?.Dispose();
        }

	}
}