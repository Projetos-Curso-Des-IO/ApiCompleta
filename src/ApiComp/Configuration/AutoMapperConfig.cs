using ApiComp.ViewModels;
using AutoMapper;
using DevIO.Business.Models;

namespace ApiComp.Configuration
{
	public class AutoMapperConfig : Profile
	{
		public AutoMapperConfig()
		{
			CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();
			CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
			CreateMap<Produto, ProdutoViewModel>().ReverseMap();
		}
	}
}
