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
			CreateMap<ProdutoImgViewModel, Produto>().ReverseMap();

            CreateMap<Produto, ProdutoViewModel>()
				.ForMember(dest => dest.NomeFornecedor, opt => opt.MapFrom(src => src.Fornecedor.Nome));


			CreateMap<Produto, ProdutoImgViewModel>()
				.ForMember(dest => dest.NomeFornecedor, opt => opt.MapFrom(src => src.Fornecedor.Nome));
		}

	}
}
