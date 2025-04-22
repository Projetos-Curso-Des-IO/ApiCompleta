using DevIO.Business.Models;

namespace DevIO.Business.Intefaces
{
    public interface IProdutoService : IDisposable
    {
        Task Adicionar(Produto produto);
		Task Atualizar(Produto produto);
        Task<bool> Remover(Guid id);
        Task<IEnumerable<Produto>>BuscarTodosProdutos();
        Task<Produto> ObterProdutoPorId(Guid id);

	}
}