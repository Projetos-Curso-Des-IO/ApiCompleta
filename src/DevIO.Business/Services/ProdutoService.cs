using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Business.Models.Validations;

namespace DevIO.Business.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository,
                              INotificador notificador) : base(notificador)
        {
            _produtoRepository = produtoRepository;
        }




        public async Task Adicionar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;

            await _produtoRepository.Adicionar(produto);
        }






        public async Task Atualizar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;

            await _produtoRepository.Atualizar(produto);
        }






        public async Task<bool> Remover(Guid id)
        {
			var produto = await _produtoRepository.ObterPorId(id);

			if (produto != null) 
            {
				await _produtoRepository.Remover(id);
				return true;
			}
            return NotificarERetornar($"Produto com {id} não encontrado para remoção.");
			
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