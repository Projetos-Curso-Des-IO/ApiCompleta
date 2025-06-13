using DevIO.Business.Intefaces;
using DevIO.Business.Notificacoes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApiComp.ControllerPrincipal
{
    
    [ApiController]
    public abstract class MainController : ControllerBase
    {
	

		private readonly INotificador _notificador;
		public readonly IUser AppUser;

		protected Guid UsuarioId { get; set; }
		public bool UsuarioAutenticado { get; set; }

		#region cto
		protected MainController(INotificador notificador, IUser appUser)
		{
			_notificador = notificador;
			AppUser = appUser;


			if (appUser.IsAuthenticated())
			{
				UsuarioId = appUser.GetUserId();
				UsuarioAutenticado = true;
			}
		}
		#endregion


		protected bool OperacaoValida()
		{
			return !_notificador.TemNotificacao();
		}


		protected ActionResult CustomResponse(object result = null)
		{
			if (OperacaoValida())
			{
				return Ok(new
				{
					success = true,
					data = result
				});
			}


			if (_notificador.ObterNotificacoes()
							.Any(static n => n.Mensagem.Contains("encontrado(a)", StringComparison.OrdinalIgnoreCase)))
			{
				return NotFound(new
				{
					success = false,
					errors = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
				});
			}


			return BadRequest(new
			{
				success = false,
				errors = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
			});
		}


		protected ActionResult CustomResponse(ModelStateDictionary modelState)
		{
			if(!ModelState.IsValid) NotificarErroModelInvalida(modelState);
			return CustomResponse();
		}


		protected void NotificarErroModelInvalida(ModelStateDictionary modelState)
		{
			var errors = modelState.Values.SelectMany(e => e.Errors);
			foreach (var erro in errors)
			{
				var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
				NotificarErro(errorMsg);
			}
		}


		protected void NotificarErro(string mensagem)
		{
			_notificador.Handle(new Notificacao(mensagem));
		}




	}
}
