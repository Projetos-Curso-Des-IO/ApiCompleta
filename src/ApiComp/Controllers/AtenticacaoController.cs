using ApiComp.Extenssions;
using ApiComp.ViewModels;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
									IOptions<AppSettings> appSettings) : base(notificador)
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
				return CustomResponse(GerarJwt());
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
				return CustomResponse(GerarJwt());
			}
			if (result.IsLockedOut)
			{
				NotificarErro("Usuário temporariamente bloqueado por tentativas inválidas");
			}

			NotificarErro("Usuário ou senha incorretos");

			return CustomResponse(loginView);
		}
		#endregion



		#region GerarJwt
		private string GerarJwt()
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
			var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
			{
				Issuer = _appSettings.Emissor,
				Audience = _appSettings.ValidoEm,
				Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			});

			var encodedToken = tokenHandler.WriteToken(token);
			return encodedToken;
		}
		#endregion


	}
}
