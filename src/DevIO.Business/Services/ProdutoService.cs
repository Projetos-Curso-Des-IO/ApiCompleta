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
				NotificarErro($"Erro ao salvar fornecedor. Entre em contato com suporte!");
			}

			await _produtoRepository.Adicionar(produto);
		}





		public async Task Atualizar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;

            await _produtoRepository.Atualizar(produto);
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
			return NotificarERetornar($"Produto com ID: {id} não encontrado para remoção.");

		}






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