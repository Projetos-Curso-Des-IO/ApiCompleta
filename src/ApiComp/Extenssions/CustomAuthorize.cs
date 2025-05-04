using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace ApiComp.Extenssions
{
	public class CustomAuthorize
	{



		#region 1 - Validação de Claims de usuários
		public static bool ValidarClaimsUsuarios(HttpContext context, string clainName, string claimValue)
		{
			return context.User.Identity.IsAuthenticated &&
				   context.User.Claims.Any(c => c.Type == clainName && c.Value.Contains(claimValue));
		}
		#endregion



		#region 2 - Atributo padrão restruturado para Autorizar via Claims
		public class ClaimsAuthorizeAttribute : TypeFilterAttribute
		{
			public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequisitoClaimFilter))
			{
				Arguments = new object[] { new Claim(claimName, claimValue) };
			}
		}
		#endregion



		#region 3 - Filtro que verifica se o usuário possui a Claim exigida
		public class RequisitoClaimFilter : IAuthorizationFilter
		{
			private readonly Claim _claim;

			public RequisitoClaimFilter(Claim claim)
			{
				_claim = claim;
			}


			public void OnAuthorization(AuthorizationFilterContext context)
			{
				//Verificar ser usuário esta autenticado
				if (!context.HttpContext.User.Identity.IsAuthenticated)
				{
					context.Result = new StatusCodeResult(401);
					return;
				}

				//Verificar se o usuário possui a Claim necessária
				if (!CustomAuthorize.ValidarClaimsUsuarios(context.HttpContext, _claim.Type, _claim.Value))
				{
					context.Result = new StatusCodeResult(403);
				}
			}
		}
		#endregion
	}
}
