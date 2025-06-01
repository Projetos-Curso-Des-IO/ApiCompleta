using DevIO.Business.Intefaces;
using System.Security.Claims;

namespace ApiComp.Extenssions
{
	public class AspNetUser : IUser
	{

		private readonly IHttpContextAccessor _acessor;

		public AspNetUser(IHttpContextAccessor acessor)
		{
			_acessor = acessor;
		}

		public string Name => _acessor.HttpContext.User.Identity.Name;

		public IEnumerable<Claim> GetClaimsIdentity()
		{
			return _acessor.HttpContext.User.Claims;
		}

		public string GetUserEmail()
		{
			return IsAuthenticated() ? _acessor.HttpContext.User.GetUserEmail() : "";
		}

		public Guid GetUserId()
		{
			return IsAuthenticated() ? Guid.Parse(_acessor.HttpContext.User.GetUserId()) : Guid.Empty;
		}

		public bool IsAuthenticated()
		{
			return _acessor.HttpContext.User.Identity.IsAuthenticated;
		}

		public bool IsInRole(string role)
		{
			return _acessor.HttpContext.User.IsInRole(role);
		}
	}

	public static class ClaimsPrincipalExtensions
	{
		public static string GetUserId(this ClaimsPrincipal principal)
		{
			if (principal == null)
			{
				throw new ArgumentException(nameof(principal));
			}

			var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
			return claim?.Value;
		}


		public static string GetUserEmail(this ClaimsPrincipal principal)
		{
			if (principal == null)
			{
				throw new ArgumentException(nameof(principal));
			}

			var claim = principal.FindFirst(ClaimTypes.Email);
			return claim?.Value;
		}
	}
}
