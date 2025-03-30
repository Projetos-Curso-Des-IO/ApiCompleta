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
                return NotificarERetornar("Já existe um fornecedor com este documento infomado.");
            

            await _fornecedorRepository.Adicionar(fornecedor);
            return true;
        }



        public async Task<bool> Atualizar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor)) return false;
         
            var fornecedorExistente = await _fornecedorRepository.ObterPorId(fornecedor.Id);
            if (fornecedorExistente == null) 
                return NotificarERetornar("Fornecedor não encontrado para atualização.");

            
            if (_fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id).Result.Any())
                return NotificarERetornar("Já existe um fornecedor com este documento informado.");

            try
            {
				await _fornecedorRepository.Atualizar(fornecedor);
				return true;
            }
            catch (Exception ex)
            {
                return NotificarERetornar($"Erro ao atualizar fornecedor: {ex.Message}");
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
                return NotificarERetornar("O fornecedor possui produtos cadastrados!");            

            var endereco = await _enderecoRepository.ObterEnderecoPorFornecedor(id);

            if (endereco != null)
            {
                await _enderecoRepository.Remover(endereco.Id);
            }

            await _fornecedorRepository.Remover(id);
            return true;
        }
		#endregion




		#region dispose
		public void Dispose()
        {
            _fornecedorRepository?.Dispose();
            _enderecoRepository?.Dispose();
        }
		#endregion




		#region NotificarERetornar
		private bool NotificarERetornar(string mensagem)
        {
            Notificar(mensagem);
			return false;
        }
		#endregion

	}
}