using ApiComp.ViewModels;
using AutoMapper;
using DevIO.Business.Models;

namespace ApiComp.Configuration
{
	public class AutoMapperConfig : Profile
	{
		public AutoMapperConfig()
		{
            CreateMap<Fornecedor, FornecedorViewModel>()
				.ForMember(dest => dest.Produto, opt => opt.MapFrom(src => src.Produtos)).ReverseMap();
            CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
            CreateMap<ProdutoViewModel, Produto>();
            CreateMap<Produto, ProdutoViewModel>()
				.ForMember(destinationMember: dest => dest.NomeFornecedor, memberOptions: opt => opt.MapFrom(mapExpression:src => src.Fornecedor.Nome));
		}
	}
}
