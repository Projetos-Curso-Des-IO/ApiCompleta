﻿using DevIO.Business.Intefaces;
using DevIO.Business.Notificacoes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApiComp.Controllers
{
    
    [ApiController]
    public abstract class MainController : ControllerBase
    {
	

		private readonly INotificador _notificador;

		#region cto
		protected MainController(INotificador notificador)
		{
			_notificador = notificador;
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
