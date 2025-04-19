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