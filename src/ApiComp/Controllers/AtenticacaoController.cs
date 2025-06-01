using ApiComp.Extenssions;
using ApiComp.ViewModels;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiComp.Controllers
{
	[Route("api/conta")]
	public class AtenticacaoController : MainController
	{

		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly AppSettings _appSettings;

		#region Ctor
		public AtenticacaoController(INotificador notificador,
									SignInManager<IdentityUser> signInManager,
									UserManager<IdentityUser> userManager,
									IOptions<AppSettings> appSettings,
									IUser user) : base(notificador, user)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_appSettings = appSettings.Value;
			
		}
		#endregion


		#region cadastrar-usuario
		[HttpPost("cadastrar-usuario")]
		public async Task<ActionResult> Registrar(RegistrarUsuarioViewModel registerUsuario)
		{
			if (!ModelState.IsValid) return CustomResponse(ModelState);

			var usuario = new IdentityUser
			{
				UserName = registerUsuario.Email,
				Email = registerUsuario.Email,
				EmailConfirmed = true
			};

			var result = await _userManager.CreateAsync(usuario, registerUsuario.Senha);
			if (result.Succeeded)
			{
				//autenticação automaticamente , false = não ira se manter após fechar o navegador
				await _signInManager.SignInAsync(usuario, false);
				return CustomResponse(await GerarJwt(usuario.Email));
			}

			foreach (var erro in result.Errors)
			{
				NotificarErro(erro.Description);
			}

			return CustomResponse(registerUsuario);
		}
		#endregion



		#region logar-usuario
		[HttpPost("logar-usuario")]
		public async Task<ActionResult> Logar(LoginViewModel loginView)
		{
			if (!ModelState.IsValid) return CustomResponse(ModelState);

			var result = await _signInManager.PasswordSignInAsync(loginView.Email, loginView.Senha, false, true);
			if (result.Succeeded)
			{
				var token = await GerarJwt(loginView.Email);

				var response = new
				{
					Token = token,
					Email = User.GetUserEmail(),
					Id = User.GetUserId(),
					Nome = User.Identity.Name
				};

				return CustomResponse(response);
			}


			if (result.IsLockedOut)
			{
				NotificarErro("Usuário temporariamente bloqueado por tentativas inválidas");
			}

			NotificarErro("Usuário ou senha incorretos");

			return CustomResponse(loginView);
		}
		#endregion



		#region GerarJwt-Padrao
		private async Task<string> GerarJwt(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			var claims = await _userManager.GetClaimsAsync(user);
			var userRoles = await _userManager.GetRolesAsync(user);

			claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
			claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Id));
			claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
			claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
			foreach (var userRole in userRoles)
			{
				claims.Add(new Claim("role", userRole));
			}


			var identityClaims = new ClaimsIdentity();
			identityClaims.AddClaims(claims);


			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
			var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
			{
				Issuer = _appSettings.Emissor,
				Audience = _appSettings.ValidoEm,
				Subject = identityClaims,
				Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			});

			var encodedToken = tokenHandler.WriteToken(token);
			return encodedToken;
		}


		private static long ToUnixEpochDate(DateTime date)
		{
			return (long)Math.Round((date.ToUniversalTime() -
									 new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
									 .TotalSeconds);
		}
		#endregion


	}
}
