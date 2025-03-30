using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using DevIO.Business.Models.Validations;

namespace DevIO.Business.Services
{
    public class FornecedorService : BaseService, IFornecedorService
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IEnderecoRepository _enderecoRepository;


		#region ctor
		public FornecedorService(IFornecedorRepository fornecedorRepository, 
                                 IEnderecoRepository enderecoRepository,
                                 INotificador notificador) : base(notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _enderecoRepository = enderecoRepository;
        }
		#endregion






		#region methods
		public async Task<bool> Adicionar(Fornecedor fornecedor)
        {
			#region validationn
			if (!ExecutarValidacao(new FornecedorValidation(), fornecedor) 
                || !ExecutarValidacao(new EnderecoValidation(), fornecedor.Endereco)) return false;
			#endregion


			if (_fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento).Result.Any())
            {
                Notificar("Já existe um fornecedor com este documento infomado.");
                return false;
            }


            await _fornecedorRepository.Adicionar(fornecedor);
            return true;
        }



        public async Task<bool> Atualizar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor)) return false;

         
            var fornecedorExistente = await _fornecedorRepository.ObterPorId(fornecedor.Id);
            if (fornecedorExistente == null)
            {
                Notificar("Fornecedor não encontrado para atualização.");
                return false;
            }

            // Verifica se o documento já existe em outro fornecedor
            if (_fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id).Result.Any())
            {
                Notificar("Já existe um fornecedor com este documento informado.");
                return false;
            }

            try
            {
                // Atualiza apenas as propriedades necessárias
                fornecedorExistente.Nome = fornecedor.Nome;
                fornecedorExistente.Documento = fornecedor.Documento;
                fornecedorExistente.TipoFornecedor = fornecedor.TipoFornecedor;
                fornecedorExistente.Ativo = fornecedor.Ativo;

                await _fornecedorRepository.Atualizar(fornecedorExistente);
                return true;
            }
            catch (Exception ex)
            {
                Notificar($"Erro ao atualizar fornecedor: {ex.Message}");
                return false;
            }
        }





        public async Task AtualizarEndereco(Endereco endereco)
        {
            if (!ExecutarValidacao(new EnderecoValidation(), endereco)) return;

            await _enderecoRepository.Atualizar(endereco);
        }








        public async Task<bool> Remover(Guid id)
        {
            if (_fornecedorRepository.ObterFornecedorProdutosEndereco(id).Result.Produtos.Any())
            {
                Notificar("O fornecedor possui produtos cadastrados!");
                return false;
            }

            var endereco = await _enderecoRepository.ObterEnderecoPorFornecedor(id);

            if (endereco != null)
            {
                await _enderecoRepository.Remover(endereco.Id);
            }

            await _fornecedorRepository.Remover(id);
            return true;
        }

		#endregion





		public void Dispose()
        {
            _fornecedorRepository?.Dispose();
            _enderecoRepository?.Dispose();
        }
    }
}