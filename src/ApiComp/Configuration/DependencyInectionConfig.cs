using DevIO.Business.Intefaces;
using DevIO.Business.Notificacoes;
using DevIO.Business.Services;
using DevIO.Data.Context;
using DevIO.Data.Repository;

namespace ApiComp.Configuration
{
	public static class DependencyInectionConfig
	{
		public static IServiceCollection ResolverDependencias(this IServiceCollection services)
		{
			services.AddScoped<MeuDbContext>();
			services.AddScoped<IFornecedorRepository, FornecedorRepository>();
			services.AddScoped<IFornecedorService, FornecedorService>();
			services.AddScoped<IEnderecoRepository, EnderecoRepository>();
			services.AddScoped<INotificador, Notificador>();
			return services;
		}
	}
}
