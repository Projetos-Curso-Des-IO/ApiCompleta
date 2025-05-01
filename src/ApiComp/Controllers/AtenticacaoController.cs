using ApiComp.ViewModels;
using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiComp.Controllers
{
	[Route("api/conta")]
	public class AtenticacaoController : MainController
	{

		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly UserManager<IdentityUser> _userManager;

		#region Ctor
		public AtenticacaoController(INotificador notificador, 
									SignInManager<IdentityUser> signInManager, 
									UserManager<IdentityUser> userManager) : base(notificador)
		{
			_signInManager = signInManager;
			_userManager = userManager;
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
				return CustomResponse(registerUsuario);
			}

			var erros = result.Errors.Select(e => e.Description).ToList();
			var mensagesErros = TraduzirMensagens(erros);

			foreach (var msg in erros)
			{
				NotificarErro(msg);
			}

			return CustomResponse(registerUsuario);
		}
		#endregion


		#region TraduzirMensagens
		public List<string> TraduzirMensagens(List<string> msgs)
		{
			var listaTraduzidas = new List<string>();
			foreach (var error in msgs)
			{
				if (error.Equals("Passwords must have at least one non alphanumeric character."))
				{
					listaTraduzidas.Add("A senha deve conter ao menos um caractere não alfanumérico.");
				}
				else if (error.Equals("Passwords must have at least one uppercase ('A'-'Z')."))
				{
					listaTraduzidas.Add("A senha deve conter ao menos uma letra maiúscula.");
				}
				else
				{
					listaTraduzidas.Add(error);
				}
			}
			return listaTraduzidas;
		}
		#endregion
	}
}
